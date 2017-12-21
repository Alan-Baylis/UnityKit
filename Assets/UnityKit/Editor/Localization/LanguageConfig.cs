using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityKit.Editor {
    public class LanguageConfig : ScriptableObject {

        public string excelPath = "Assets/_Config/Languages";

        /// <summary>
        /// 导出目录
        /// </summary>
        public string exportPath = "Assets/Assets/Languages";
    }
}
