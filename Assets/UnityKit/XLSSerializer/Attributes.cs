using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityKit {
    /// <summary>
    /// 序列化类型
    /// </summary>
    public enum XLSSType {
        Obj,
        Map,
        List,
        Script,
    }

    public class XLSSAttribute : Attribute {
        public string file;
        public XLSSType type;
    }

    /// <summary>
    /// 用于标记字典key
    /// </summary>
    public class PrimaryKeyAttribute : Attribute {
        public string Value { get; private set; }
        public PrimaryKeyAttribute(string value) {
            this.Value = value;
        }
    }
}