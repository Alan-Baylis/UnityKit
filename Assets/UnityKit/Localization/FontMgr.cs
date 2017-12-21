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
    sealed public class FontMgr : Singleton<FontMgr>, IDictionary<string, Font> {
        Dictionary<string, Font> _dict = new Dictionary<string, Font>();

        ///// <summary>
        ///// 注册字体
        ///// <para>如果别名已经被注册,字体注册失败返回false. 否则字体注册成功返回true</para> 
        ///// </summary>
        ///// <param name="fontAlias"></param>
        ///// <param name="font"></param>
        ///// <returns></returns>
        //public bool Register(string fontAlias, Font font) {
        //    if (_dict.ContainsKey(fontAlias)) {
        //        return false;
        //    } else {
        //        _dict.Add(fontAlias,font);
        //        return true;
        //    }
        //}

        ///// <summary>
        ///// 注销字体
        ///// </summary>
        ///// <param name="fontAlias"></param>
        //public void UnRegister(string fontAlias) {
        //    _dict.Remove(fontAlias);
        //}

        ///// <summary>
        ///// 通过字体别名获取字体
        ///// </summary>
        ///// <param name="fontAlias"></param>
        ///// <param name="font"></param>
        ///// <returns></returns>
        //public bool GetFont(string fontAlias, out Font font) {
        //    return _dict.TryGetValue(fontAlias,out font);
        //}

        //public bool HasFont(string fontAlias) {
        //    return _dict.ContainsKey(fontAlias);
        //}

        public ICollection<string> Keys { get { return _dict.Keys; } }
        public ICollection<Font> Values { get { return _dict.Values; } }
        public int Count { get { return _dict.Count; } }
        public bool IsReadOnly { get { return true; } }

        public Font this[string key] {
            get {
                Font ret;
                if (_dict.TryGetValue(key, out ret)) {
                    return ret;
                }
                return ret;
            }
            set { _dict[key] = value; }
        }

        public void Add(KeyValuePair<string, Font> item) {
            _dict.Add(item.Key, item.Value);
        }

        public void Add(string key, Font value) {
            _dict.Add(key, value);
        }

        public bool ContainsKey(string key) {
            return _dict.ContainsKey(key);
        }

        public bool Contains(KeyValuePair<string, Font> item) {
            return _dict.ContainsKey(item.Key) && _dict[item.Key].Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<string, Font>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public void Clear() {
            _dict.Clear();
        }

        public bool Remove(string key) {
            return _dict.Remove(key);
        }

        public bool Remove(KeyValuePair<string, Font> item) {
            return _dict.Remove(item.Key);
        }

        public bool TryGetValue(string key, out Font value) {
            return _dict.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, Font>> GetEnumerator() { return _dict.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _dict.GetEnumerator(); }
    }
}
