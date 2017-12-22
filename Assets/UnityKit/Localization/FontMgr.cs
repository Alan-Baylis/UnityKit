/**
 * 字体切换实现思路
 * 运行时用Text序列化的字体名称从FontMgr内查找对应替换字体替换当前字体。
 * 由于中文字体过于庞大，最优方案时使用 only ascii characters 的语言作为默认UI
 **/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityKit {
    sealed public class FontMgr : Singleton<FontMgr> {
        Dictionary<string, Font> _dict = new Dictionary<string, Font>();

        /// <summary>
        /// 注册字体
        /// <para>如果别名已经被注册,字体注册失败返回false. 否则字体注册成功返回true</para> 
        /// </summary>
        /// <param name="fontAlias"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public bool Add(string fontAlias, Font font) {
            if (_dict.ContainsKey(fontAlias)) {
                return false;
            } else {
                _dict.Add(fontAlias, font);
                return true;
            }
        }

        /// <summary>
        /// 注销字体
        /// </summary>
        /// <param name="fontAlias"></param>
        public void Remove(string fontAlias) {
            _dict.Remove(fontAlias);
        }

        /// <summary>
        /// 通过字体别名获取字体
        /// </summary>
        /// <param name="fontAlias"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public bool TryGetValue(string fontAlias, out Font font) {
            return _dict.TryGetValue(fontAlias, out font);
        }

        public bool Contains(string fontAlias) {
            return _dict.ContainsKey(fontAlias);
        }
    }
}
