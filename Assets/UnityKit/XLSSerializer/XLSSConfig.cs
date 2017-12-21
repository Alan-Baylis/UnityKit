using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityKit {
    public class XLSSConfig : ScriptableObject {
        /// <summary>
        /// Excel 目录
        /// </summary>
        public string ExcelPath = "Assets/Editor Default Resources/Configs";

        /// <summary>
        /// 序列化导出路径
        /// </summary>
        public string BineryPath = "Assets/Assets/Configs";

        /// <summary>
        /// 脚本导出路径
        /// </summary>
        public string ScriptPath = "Assets/Scripts/Data";
    }
}