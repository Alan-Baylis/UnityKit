using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace UnityKit.Editor {
    /// <summary>
    /// Menu 自定义添加
    /// </summary>
    public class LangugeXLSSerializer {
        [MenuItem("Tools/Lang")]
        static public void Excute() {
            var config = UnityKitEditorUtils.GetConfig<LanguageConfig>();
            if (config == null) {
                UnityKitEditorUtils.CreateConfig<LanguageConfig>();
                EditorUtility.DisplayDialog(
                    "Warnning",
                    string.Format("Set Configs -> {0} at first time, Please.", UnityKitEditorUtils.GetFilename<LanguageConfig>()),
                    "OK");
                return;
            }

            var excelPath = config.excelPath;
            var exportPath = config.exportPath;

            if (!Directory.Exists(exportPath)) {
                Directory.CreateDirectory(exportPath);
            }

            if (!Directory.Exists(excelPath)) {
                EditorUtility.DisplayDialog(
                    "Warnning",
                    string.Format("Excel path is not found -> {0}.", excelPath),
                    "OK");
                return;
            }

            ITableReader reader = new NPOIReader();
            Directory.GetFiles(excelPath, "*.xls")
                .ToList()
                .ForEach(x=> {
                    var llcode = Path.GetFileNameWithoutExtension(x);
                    if (Language.IsLanguageCode(llcode)) {
                        var table = reader.Read(x);
                        var langpack = LanguagePack.CreateInstance<LanguagePack>();
                        langpack.llcode = llcode;
                        for (int r = 1; r < table.nrows; r++) {
                            var key = table[r, 0];
                            var value = table[r, 0];
                            langpack.Add(key,value);
                        }
                        AssetDatabase.CreateAsset(langpack,Path.Combine(exportPath,llcode)+".asset");
                    }
                });
        }
    }
}