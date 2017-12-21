using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnityKit {

    /// <summary>
    /// 表格读取接口
    /// </summary>
    public interface ITableReader {
        Table Read(string excel);
    }

    public class Serializer {

        ITableReader _reader;
        XLSSConfig _config;
        public Serializer(XLSSConfig config, ITableReader reader) {
            _config = config;
            _reader = reader;
        }

        public List<Type> GetSerializableClasses() {
            List<Type> types = new List<Type>();
            foreach (var f in Directory.GetFiles(_config.ScriptPath, "*.cs")) {
                var name = Path.GetFileNameWithoutExtension(f);
                var code = File.ReadAllText(f);
                var namespaceMatch = new Regex("namespace [0-9a-zA-Z._]+").Match(code);
                var fullname = namespaceMatch.Success ? namespaceMatch.Value.Replace("namespace", "").Replace(" ","") + "."+ name : name;
                var type = this.GetType().Assembly.GetType(fullname);
                if (type != null) {
                    var attr = type.GetAttribute<XLSSAttribute>();
                    if (attr != null) {
                        types.Add(type);
                    }
                }                
            }
            return types;
        }

        public void SerializeAll() {
            foreach (var type in GetSerializableClasses()) {
                Serialize(type);
            }
        }

        public void Serialize<T>(Stream stream, T obj) {
            var binFormatter = new BinaryFormatter();
            binFormatter.Serialize(stream, obj);
        }

        public void Serialize<T>() {
            var classtype = typeof(T);
            var xlss = classtype.GetAttribute<XLSSAttribute>();
            if (xlss == null) {
                UnityEngine.Debug.LogWarning(string.Format("Can't Serialize {0}", classtype.FullName));
                return;
            }

            var excelfile = Path.Combine(_config.ExcelPath, xlss.file);
            if (!File.Exists(excelfile)) return;
            var table = _reader.Read(excelfile);

            if (xlss.type == XLSSType.Script) {
                var codestr = ScriptSerialize<T>(table);
                var filename = Path.Combine(_config.ScriptPath, classtype.Name) + ".cs";
                File.WriteAllText(filename, codestr);
            } else {
                var obj = BinarySerialize<T>(table);
                var filename = Path.Combine(_config.BineryPath, typeof(T).Name) + ".bytes";
                using (var fs = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write)) {
                    Serialize(fs, obj);
                }
            }
        }

        public void Serialize(Type classtype) {
            var xlss = classtype.GetAttribute<XLSSAttribute>();
            if (xlss == null) {
                UnityEngine.Debug.LogWarning(string.Format("Can't Serialize {0}", classtype.FullName));
                return;
            }

            var excelfile = Path.Combine(_config.ExcelPath, xlss.file);
            if (!File.Exists(excelfile)) return;
            var table = _reader.Read(excelfile);

            if (xlss.type == XLSSType.Script) {
                var codestr = ScriptSerialize(table,classtype);
                var filename = Path.Combine(_config.ScriptPath, classtype.Name) + ".cs";
                File.WriteAllText(filename, codestr);
            } else {
                var obj = BinarySerialize(table,classtype);
                var filename = Path.Combine(_config.BineryPath, classtype.Name) + ".bytes";
                using (var fs = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write)) {
                    Serialize(fs, obj);
                }
            }
        }

        object BinarySerialize<T>(Table table) {
            Type classtype = typeof(T);
            int nrows = table.nrows;
            int ncols = table.ncols;
            var xlss = classtype.GetAttribute<XLSSAttribute>();
            var fieldNames = new List<string>();
            for (int i = 0; i < ncols; i++) {
                if (string.IsNullOrEmpty(table[0, i])) {
                    continue;
                }
                fieldNames.Add(table[0, i]);
            }

            var objProxy = new ObjProxy(classtype, fieldNames);

            if (xlss.type == XLSSType.List) {
                var list = new List<T>();

                for (int row = 1; row < nrows; row++) {
                    var key = table[row, 0];
                    if (!string.IsNullOrEmpty(key)) {
                        list.Add((T)objProxy.New());
                        objProxy.SetContext(table, row);
                        ReadItem(table, row, objProxy);
                    }
                }
                return list;
            } else if(xlss.type == XLSSType.Map) {
                Type keyCls = typeof(int);
                Type rootCls = classtype;
                var primaryKeyAttr = rootCls.GetAttribute<PrimaryKeyAttribute>();
                string keyName = null;

                if (primaryKeyAttr != null) {
                    var keyField = rootCls.GetFields().ToList().Find(x => x.Name == primaryKeyAttr.Value);
                    if (keyField != null) {
                        keyCls = keyField.FieldType;
                        keyName = keyField.Name;
                    }
                } else {
                    var keyField = rootCls.GetFields().ToList().Find(x => x.IsPublic);
                    if (keyField != null) {
                        keyCls = keyField.FieldType;
                        keyName = keyField.Name;
                    }
                }

                int keyCol = 0;
                for (var c = 0; c < ncols; c++) {
                    if (table[0, c] == keyName) {
                        keyCol = c;
                    }
                }

                if (keyCls == null) {
                    throw new Exception(string.Format("Key of map is not found. table:{0}", table.name));
                }

                var fullname = string.Format("System.Collections.Generic.Map`2[[{0}],[{1}]]", keyCls.AssemblyQualifiedName, classtype.AssemblyQualifiedName);
                var maptype = Type.GetType(fullname);
                IMap map = Activator.CreateInstance(maptype) as IMap;

                for (int row = 1; row < nrows; row++) {
                    var headColValue = table[row, 0];
                    if (!string.IsNullOrEmpty(headColValue)) {

                        if (string.IsNullOrEmpty(table[row, keyCol])) {
                            throw new Exception(string.Format("Null string can not be used as a dictionary key. table:{0} row:{1} col:{2}", table.name, row, keyCol));
                        }

                        map.Add(keyCls.Parse(table[row, keyCol]), objProxy.New());
                        objProxy.SetContext(table, row);
                        ReadItem(table, row, objProxy);
                    }
                }
                return map;
            } else if (xlss.type == XLSSType.Obj) {
                objProxy.New();
                for (int r = 1; r < nrows; r++) {
                    for (int c = 0; c < 2; c++) {
                        var key = table[r, 0];
                        var value = table[r, 1];
                        objProxy.SetField(key, value, r);
                    }
                }
                return objProxy.obj;
            }
            return null;
        }

        object BinarySerialize(Table table,Type classtype) {
            int nrows = table.nrows;
            int ncols = table.ncols;
            var xlss = classtype.GetAttribute<XLSSAttribute>();
            var fieldNames = new List<string>();
            for (int i = 0; i < ncols; i++) {
                if (string.IsNullOrEmpty(table[0,i])) {
                    continue;
                }
                fieldNames.Add(table[0, i]);
            }

            var objProxy = new ObjProxy(classtype, fieldNames);

            if (xlss.type == XLSSType.List) {
                var fullname = string.Format("System.Collections.Generic.List`1[[{0}]]",classtype.AssemblyQualifiedName);
                var listtype = Type.GetType(fullname);
                var list = Activator.CreateInstance(listtype) as IList;
                
                for (int row = 1; row < nrows; row++) {
                    var key = table[row, 0];
                    if (!string.IsNullOrEmpty(key)) {
                        list.Add(objProxy.New());
                        objProxy.SetContext(table, row);
                        ReadItem(table, row, objProxy);
                    }
                }
                return list;
            } else if(xlss.type == XLSSType.Map){
                
                Type keyCls = null;
                Type rootCls = classtype;
                var primaryKeyAttr = rootCls.GetAttribute<PrimaryKeyAttribute>();
                string keyName = null;

                if (primaryKeyAttr != null) {
                    var keyField = rootCls.GetFields().ToList().Find(x => x.Name == primaryKeyAttr.Value);
                    if (keyField != null) {
                        keyCls = keyField.FieldType;
                        keyName = keyField.Name;
                    }
                } else {
                    var keyField = rootCls.GetFields().ToList().Find(x => x.IsPublic);
                    if (keyField != null) {
                        keyCls = keyField.FieldType;
                        keyName = keyField.Name;
                    }
                }

                int keyCol = 0;
                for (var c = 0;c < ncols; c++) {
                    if (table[0,c] == keyName) {
                        keyCol = c;
                    }
                }

                if (keyCls == null) {
                    throw new Exception(string.Format("Key of map is not found. table:{0}",table.name));
                }

                var fullname = string.Format("System.Collections.Generic.Map`2[[{0}],[{1}]]",keyCls.AssemblyQualifiedName, classtype.AssemblyQualifiedName);
                var maptype = Type.GetType(fullname);
                IMap map = Activator.CreateInstance(maptype) as IMap;

                for (int row = 1; row < nrows; row++) {
                    var headColValue = table[row, 0];
                    if (!string.IsNullOrEmpty(headColValue)) {

                        if (string.IsNullOrEmpty(table[row, keyCol])) {
                            throw new Exception(string.Format("Null string can not be used as a dictionary key. table:{0} row:{1} col:{2}", table.name, row, keyCol));
                        }

                        map.Add(keyCls.Parse(table[row, keyCol]), objProxy.New());
                        objProxy.SetContext(table, row);
                        ReadItem(table, row, objProxy);
                    }
                }
                return map;
            } else if(xlss.type == XLSSType.Obj){
                objProxy.New();
                for (int r = 1; r < nrows; r++) {
                    for (int c = 0; c < 2; c++) {
                        var key = table[r,0];
                        var value = table[r, 1];
                        objProxy.SetField(key, value, r);
                    }
                }
                return objProxy.obj;
            }
            return null;
        }

        void ReadItem(Table table, int row, ObjProxy objProxy) {
            int nrows = table.nrows;
            int ncols = table.ncols;
            int offset = 0;
            int headCount = 0;
            for (int r = row; r < nrows; r++) {
                if (!string.IsNullOrEmpty(table[r, 0])) {
                    headCount += 1;
                }

                if (headCount > 1) {
                    break;
                }

                for (int c = 0; c < ncols; c++) {
                    var key = table[0, c];
                    var value = table[r, c];
                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) {
                        continue;
                    }

                    objProxy.SetField(key, value, offset);
                }
                offset++;
            }
        }
        string ScriptSerialize<T>(Table table) { return ScriptSerialize(table,typeof(T));}
        string ScriptSerialize(Table table,Type classtype) {
            var fieldexpr = "       public const {fieldtype} {fieldname} = {fieldvalue};\n";
            var codetemp  = @"
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
namespace {namespace} {
    static public class {classname} {
{fieldexpr}
    }
}
";
            var fields = classtype.GetFields().ToList().FindAll(x=>x.IsPublic && x.IsStatic);
            var fieldstr = "";
            var nrows = table.nrows;
            var ncols = table.ncols;
            foreach (var f in fields) {
                for (int row = 0; row < nrows; row++) {
                    var key = table[row,0];
                    var value = table[row,1];

                    if(f.Name != key) {
                        continue;
                    }

                    if (f.FieldType == typeof(float)) {
                        value = value + "f";
                    } else if (f.FieldType == typeof(string)) {
                        value = "\""+value + "\"";
                    }

                    fieldstr += fieldexpr
                        .Replace("{fieldtype}", f.FieldType.Name)
                        .Replace("{fieldname}", f.Name)
                        .Replace("{fieldvalue}", value);
                }
            }

            return codetemp
                .Replace("{namespace}", classtype.Namespace)
                .Replace("{classname}", classtype.Name)
                .Replace("{fieldexpr}", fieldstr);
        }
    }
}