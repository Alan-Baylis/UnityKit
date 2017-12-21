using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace UnityKit {
    public class ObjProxy {

        Type _cls;
        Dictionary<string, List<List<FieldInfo>>> _clsFieldTree = new Dictionary<string, List<List<FieldInfo>>>();
        public ObjProxy(Type cls, List<string> fieldNames) {
            _cls = cls;
            foreach (var name in fieldNames) {
                var tree = new List<List<FieldInfo>>();
                var list = new List<FieldInfo>();
                GetFieldLink(cls, name, list, tree);
                if (tree.Count > 0) {
                    _clsFieldTree.Add(name, tree);
                }
            }
        }

        Table _table;
        int _row;

        public void SetContext(Table table, int row) {
            this._table = table;
            this._row = row;
        }

        static void GetFieldLink(Type type, string fieldName, List<FieldInfo> list, List<List<FieldInfo>> tree) {
            var fields = type.GetFields().ToList().FindAll(x => x.IsPublic);
            FieldInfo matchField = null;
            foreach (var field in fields) {
                if (field.Name == fieldName) {
                    list.Add(field);
                    tree.Add(list);
                    matchField = field;
                    break;
                }
            }

            foreach (var field in fields) {
                if (field.FieldType.IsBasicType() || matchField == field) continue;
                var subList = new List<FieldInfo>(list);
                if (matchField != null) subList.RemoveAt(subList.Count - 1);
                subList.Add(field);
                if (field.FieldType.IsListType()) {
                    Type itemType;
                    field.FieldType.GetListParamType(out itemType);
                    GetFieldLink(itemType, fieldName, subList, tree);
                } else if (field.FieldType.IsMapType()) {
                    Type KeyType;
                    Type valType;
                    field.FieldType.GetMapParamType(out KeyType, out valType);
                    GetFieldLink(valType, fieldName, subList, tree);
                } else {
                    GetFieldLink(field.FieldType, fieldName, subList, tree);
                }
            }
        }

        public object obj { get; private set; }
        public object New() {
            obj = Activator.CreateInstance(_cls);
            return obj;
        }

        public void SetField(string fieldName, string fieldValue, int offsetIndex) {
            if (!_clsFieldTree.ContainsKey(fieldName)) return;
            var fieldTree = _clsFieldTree[fieldName];
            foreach (var fieldList in fieldTree) {
                var last = fieldList.Count - 1;
                var target = obj;
                for (int i = 0; i <= last; i++) {
                    var field = fieldList[i];
                    var cls = field.FieldType;
                    var name = field.Name;

                    if (field.FieldType.IsBasicType()) {
                        field.SetValue(target, cls.Parse(fieldValue));
                    } else if (field.FieldType.IsListType()) {
                        IList list = field.GetValue(target) as IList;
                        if (list == null) {
                            list = Activator.CreateInstance(field.FieldType) as IList;
                            field.SetValue(target, list);
                        }

                        Type valCls;
                        field.FieldType.GetListParamType(out valCls);

                        if (valCls.IsListType() || valCls.IsMapType()) {
                            throw new Exception("List 内不能嵌套 list or dict");
                        } else if (valCls.IsBasicType()) {
                            var val = valCls.Parse(fieldValue);
                            if (list.Count <= offsetIndex) {
                                list.Add(val);
                            } else {
                                list[offsetIndex] = val;
                            }
                        } else {
                            if (list.Count <= offsetIndex) {
                                var val = Activator.CreateInstance(valCls);
                                list.Add(val);
                                target = val;
                            } else {
                                target = list[offsetIndex];
                            }
                        }
                    } else if (field.FieldType.IsMapType()) {
                        IMap map = field.GetValue(target) as IMap;
                        if (map == null) {
                            map = Activator.CreateInstance(field.FieldType) as IMap;
                            field.SetValue(target, map);
                        }

                        Type keyCls;
                        Type valCls;
                        field.FieldType.GetMapParamType(out keyCls, out valCls);

                        if (valCls.IsListType() || valCls.IsMapType()) {
                            throw new Exception("dict 内不能嵌套 list or dict");
                        } else if (valCls.IsBasicType()) {
                            var key = keyCls.Parse(fieldValue);
                            var val = valCls.Parse(fieldValue);
                            if (map.Contains(key)) {
                                map[key] = val;
                            } else {
                                map.Add(key, val);
                            }
                            break;
                        } else {

                            var primaryKey = field.GetCustomAttributes(false).ToList().Find(x => x is PrimaryKeyAttribute) as PrimaryKeyAttribute;
                            string keyName = null;
                            if (primaryKey != null) {
                                keyName = primaryKey.Value;
                            }

                            if (string.IsNullOrEmpty(keyName)) {
                                UnityEngine.Debug.LogWarning("Not PrimaryKeyAttribute. Use first field instead.");
                                var firstField = valCls.GetFields().ToList().Find(x => x.IsPublic);
                                if (firstField == null) {
                                    throw new Exception("No Public Field");
                                }
                                keyName = firstField.Name;
                            }

                            object key = null;
                            int ncols = _table.ncols;
                            for (int c = 0; c < ncols; c++) {
                                if (_table[0, c] == keyName) {
                                    if (string.IsNullOrEmpty(_table[_row + offsetIndex, c])) {
                                        //不能使用空字符串作为字典的可以
                                        throw new Exception(string.Format("Null string can not be used as a dictionary key. table:{0} row:{1} col:{2}", _table.name, _row + offsetIndex, c));
                                    }
                                    key = keyCls.Parse(_table[_row + offsetIndex, c]);
                                    break;
                                }
                            }

                            if (key == null) {
                                throw new Exception("Map primary key is not found.");
                            }

                            object valObj;
                            if (map.Contains(key)) {
                                valObj = map[key];
                            } else {
                                valObj = Activator.CreateInstance(valCls);
                                map.Add(key, valObj);
                            }
                            target = valObj;
                        }

                    } else {
                        var obj = field.GetValue(target);
                        if (obj == null) {
                            obj = Activator.CreateInstance(field.FieldType);
                            field.SetValue(target, obj);
                        }

                        target = obj;
                    }
                }
            }
        }
    }
}