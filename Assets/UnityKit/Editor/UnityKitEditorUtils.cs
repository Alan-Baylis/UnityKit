using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityKit.Editor {
    static public class UnityKitEditorUtils {
        public static readonly string PATH_CONFIG = "Assets/_Config";
        static public T GetConfig<T>() where T: ScriptableObject {
            var filename = typeof(T).Name + ".asset";
            if (!string.IsNullOrEmpty(PATH_CONFIG)) {
                if (!Directory.Exists(PATH_CONFIG)) {
                    Directory.CreateDirectory(PATH_CONFIG);
                }
                filename = Path.Combine(PATH_CONFIG, filename);
            }
            return AssetDatabase.LoadAssetAtPath<T>(filename);
        }

        static public T CreateConfig<T>() where T : ScriptableObject {
            var filename = GetFilename<T>();
            var config = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(config, filename);
            return config;
        }

        static public string GetFilename<T>(string ext = "") where T:UnityEngine.Object {
            Type type = typeof(T);
            string filename = type.Name;
            if (!string.IsNullOrEmpty(PATH_CONFIG)) {
                if (!Directory.Exists(PATH_CONFIG)) {
                    Directory.CreateDirectory(PATH_CONFIG);
                }
                filename = Path.Combine(PATH_CONFIG, filename);
            }
            if (string.IsNullOrEmpty(ext)) {
                if (type.IsSubclassOf(typeof(ScriptableObject)) ||
                    type.IsSubclassOf(typeof(TextAsset))) {
                    filename += ".asset";
                } else if (type.IsSubclassOf(typeof(GameObject))) {
                    filename += ".prefab";
                } else if (type.IsSubclassOf(typeof(Texture))) {
                    filename += ".png";
                } else if (type.IsSubclassOf(typeof(AudioClip))) {
                    filename += ".mp3";
                } else {
                    Debug.LogWarning("NotImplementedException. You should set file extension.");
                }
            } else {
                if (ext.StartsWith(".")) filename += ext;
                else filename += "." + ext;
            }

            return filename;
        }

        static public bool Exist<T>() where T:UnityEngine.Object {
            var filename = GetFilename<T>();
            return File.Exists(filename);
        }
    }
}