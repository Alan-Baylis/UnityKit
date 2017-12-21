using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditorInternal;
using UnityEditor;

namespace UnityKit.Editor {
    public class Expr {

        public static string ExprType(string type, bool isList) {
            if (isList) return string.Format("List<{0}>", type);
            else return type;
        }

        public static string ExprListNew(string type, string fieldName) {
            return string.Format("new {0}();\n", ExprType(type, true));
        }

        public static string ExprListAdd(string type, string fieldName, string path, string prefix = "") {
            return string.Format("{0}{1}.Add({2});\n", prefix, fieldName, ExprFind(type, path));
        }

        public static string ExprFind(string type, string path) {
            return EXPR_FIND.Replace("{fieldpath}", path).Replace("{fieldtype}", ExprType(type, false));
        }

        public static string ExprGetter(string type, string fieldName, string path, bool isList, List<string> pathes) {
            if (isList) {
                string findstr = "";
                pathes.ForEach(x => {
                    findstr += ExprListAdd(type, fieldName, x, "_");
                });

                return EXPR_GETTER
                .Replace("{fieldtype}", ExprType(type, isList))
                .Replace("{fieldname}", fieldName)
                .Replace("{initexpr}", ExprListNew(type, fieldName) + findstr);
            } else {
                return EXPR_GETTER
                .Replace("{fieldtype}", ExprType(type, isList))
                .Replace("{fieldname}", fieldName)
                .Replace("{initexpr}", ExprFind(type, path) + ";\n");
            }
        }

        public static string EXPR_FIND = "transform.Find(\"{fieldpath}\").GetComponent<{fieldtype}>()";
        public static string EXPR_GETTER = @"
        protected {fieldtype} _{fieldname};
        public {fieldtype} {fieldname} {
            get {
                if(_{fieldname} == null) {
                    _{fieldname} = {initexpr}                }
                return _{fieldname};
            }
        }
";
    }

    public enum UIType {
        UI,
        Component,
        View,
    }

    public struct UINode {
        public string tag;
        public string typeName;
        public UIType UIType;
    }

    struct FieldExpr {
        /// <summary>
        /// 是否是列表
        /// </summary>
        public bool isList;
        public string path;
        public List<string> pathes;
        public string fieldName;
        public string fieldType;
    }

    public class UIScriptCreator {
        Dictionary<string, UINode> _nodeDict = new Dictionary<string, UINode>();

        string _viewTemplatePath = "Assets/Assets/UI/Template/View.txt";

        /// <summary>
        /// 脚本模板路径
        /// </summary>
        public string ViewTemplatePath {
            get { return _viewTemplatePath; }
            set { _viewTemplatePath = value; }
        }

        string _UIPrefabPath = "Assets/Assets/UI";

        /// <summary>
        /// UI Prefab 路径
        /// </summary>
        public string UIPrefabPath {
            get { return _UIPrefabPath; }
            set { _UIPrefabPath = value; }
        }

        string _UIScriptPath = "Assets/Scripts/UI";

        /// <summary>
        /// 脚本导出路径
        /// </summary>
        public string UIScriptPath {
            get { return _UIScriptPath; }
            set { _UIScriptPath = value; }
        }

        public void AddNode(UINode node) {
            if (!_nodeDict.ContainsKey(node.tag)) {
                _nodeDict.Add(node.tag, node);
                if (!InternalEditorUtility.tags.Contains(node.tag)) {
                    InternalEditorUtility.AddTag(node.tag);
                }
            }
        }

        public void RemoveNode(UINode node) {
            if (_nodeDict.ContainsKey(node.tag)) {
                _nodeDict.Remove(node.tag);
                if (InternalEditorUtility.tags.Contains(node.tag)) {
                    InternalEditorUtility.RemoveTag(node.tag);
                }
            }
        }

        public void RemoveNode(string tag) {
            if (_nodeDict.ContainsKey(tag)) {
                _nodeDict.Remove(tag);
                if (InternalEditorUtility.tags.Contains(tag)) {
                    InternalEditorUtility.RemoveTag(tag);
                }
            }
        }

        public void Create(GameObject prefab) {
            if (!DependCheck()) return;
            string template = File.ReadAllText(ViewTemplatePath);
            UINode uiNode;
            if (_nodeDict.TryGetValue(prefab.tag, out uiNode) && uiNode.UIType == UIType.View) {
                CreateScript(prefab.transform, template);
            }
        }

        public void CreateAll() {
            if (!DependCheck()) return;

            string template = File.ReadAllText(ViewTemplatePath);
            Directory.GetDirectories(UIPrefabPath, "Prefab", SearchOption.AllDirectories)
                .ToList()
                .ForEach(x => {
                    Directory.GetFiles(x).ToList()
                    .FindAll(fileName => fileName.EndsWith(".prefab"))
                    .ForEach(fileName => {
                        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(fileName);
                        if (prefab != null) {
                            UINode uiNode;
                            if (_nodeDict.TryGetValue(prefab.tag, out uiNode) && uiNode.UIType == UIType.View) {
                                CreateScript(prefab.transform, template);
                            }
                        }
                    });
                });
        }

        bool DependCheck() {
            if (!File.Exists(ViewTemplatePath)) {
                EditorUtility.DisplayDialog("Template File Is Not Found", ViewTemplatePath, "OK");
                return false;
            }

            if (!Directory.Exists(UIPrefabPath)) {
                EditorUtility.DisplayDialog("UI Prefab Dir Is Not Found", UIPrefabPath, "OK");
                return false;
            }

            if (!_nodeDict.Values.Any(x => x.UIType == UIType.View)) {
                EditorUtility.DisplayDialog("No View Node", "UI(View) ?", "OK");
                return false;
            }

            if (!Directory.Exists(UIScriptPath)) {
                Directory.CreateDirectory(UIScriptPath);
            }
            return true;
        }

        void CreateScript(Transform node, string template) {
            var clsInfo = new Dictionary<string, FieldExpr>();
            Lookup(node, "", clsInfo, true);
            StringBuilder VarExpr = new StringBuilder();
            StringBuilder InitExpr = new StringBuilder();

            foreach (var info in clsInfo.Values) {
                VarExpr.Append(Expr.ExprGetter(info.fieldType, info.fieldName, info.path, info.isList, info.pathes));
            }

            var code = template
            .Replace("{ClassName}", node.name)
            .Replace("{VarExpr}", VarExpr.ToString())
            .Replace("{InitExpr}", InitExpr.ToString());

            var filename = Path.Combine(UIScriptPath, node.name + ".cs");
            if (!Directory.Exists(UIScriptPath)) {
                Directory.CreateDirectory(UIScriptPath);
            }

            File.WriteAllText(filename, code, Encoding.UTF8);
        }

        void Lookup(Transform node, string path, Dictionary<string, FieldExpr> clsInfo, bool skip) {
            if (_nodeDict.ContainsKey(node.tag) && !skip) {
                string goName = node.gameObject.name;
                UINode uiNode = _nodeDict[node.tag];
                var fieldName = goName;
                var fieldType = uiNode.typeName;

                if (goName.IndexOf("|") != -1) {
                    var arr = goName.Split('|');
                    if (arr.Length >= 2) {
                        fieldName = arr[1];
                        fieldType = arr[0];
                    }
                }

                if (clsInfo.ContainsKey(fieldName)) {
                    Debug.LogWarning(string.Format("field <{0}> repeated. at different layer", goName));
                } else if (!string.IsNullOrEmpty(fieldName) && !string.IsNullOrEmpty(fieldType)) {
                    clsInfo.Add(fieldName, new FieldExpr {
                        path = path,
                        fieldName = fieldName,
                        fieldType = fieldType
                    });
                }

                if (uiNode.UIType == UIType.Component) {
                    //Component 节点自成一体, 不再向下查询
                    return;
                }
            }

            for (int i = 0; i < node.childCount; i++) {
                var child = node.GetChild(i);
                bool isGroup = new Regex("[a-zA-Z|]+_[0-9]+$").IsMatch(child.name);
                Lookup(child, string.IsNullOrEmpty(path) ? child.name : string.Format("{0}/{1}", path, child.name), clsInfo, false);
                if (isGroup) {
                    var fieldName = new Regex("[a-zA-Z|]+_[0-9]+$").Match(child.name).Value;
                    if (fieldName.IndexOf("|") != -1) {
                        var arr = fieldName.Split('|');
                        if (arr.Length < 2) {
                            Debug.LogWarning("Error GameObject Name");
                            continue;
                        }
                        fieldName = arr[1];
                    }

                    FieldExpr childField;
                    if (clsInfo.TryGetValue(fieldName, out childField)) {

                        fieldName = new Regex("[a-zA-Z]+").Match(fieldName).Value;
                        FieldExpr lst;
                        if (clsInfo.TryGetValue(fieldName, out lst)) {
                            lst.isList = true;
                            lst.pathes.Add(childField.path);
                        } else {
                            clsInfo.Add(fieldName, new FieldExpr() {
                                isList = true,
                                pathes = new List<string>() { childField.path },
                                fieldName = fieldName,
                                fieldType = childField.fieldType
                            });
                        }
                        clsInfo.Remove(childField.fieldName);
                    }
                }
            }
        }
    }
}