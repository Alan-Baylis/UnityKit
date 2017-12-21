using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityKit {
    /// <summary>
    /// 实现字典的原因是,为了以后在UGUI.Text 扩展的时候注入 IDictionary<string, string>.而不会引入其他依赖
    /// </summary>
    sealed public class LanguageMgr: Singleton<LanguageMgr> ,IDictionary<string, string> {

# region dict impl
        Dictionary<string, string> _dict = new Dictionary<string, string>();

        public ICollection<string> Keys { get { return _dict.Keys; } }
        public ICollection<string> Values { get { return _dict.Values; } }
        public int Count { get { return _dict.Count; } }
        public bool IsReadOnly { get { return true; } }

        public string this[string key] {
            get {
                string ret;
                if (_dict.TryGetValue(key, out ret)) {
                    return ret;
                }
                return key;
            }
            set { _dict[key] = value; }
        }

        public void Add(KeyValuePair<string, string> item) {
            _dict.Add(item.Key, item.Value);
        }

        public void Add(string key, string value) {
            _dict.Add(key, value);
        }

        public bool ContainsKey(string key) {
            return _dict.ContainsKey(key);
        }

        public bool Contains(KeyValuePair<string, string> item) {
            return _dict.ContainsKey(item.Key) && _dict[item.Key].Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public void Clear() {
            _dict.Clear();
        }

        public bool Remove(string key) {
            return _dict.Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item) {
            return _dict.Remove(item.Key);
        }

        public void Remove(object key) {
            Remove((string)key);
        }

        public bool TryGetValue(string key, out string value) {
            return _dict.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() { return _dict.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _dict.GetEnumerator(); }
#endregion

        public Language currentLanguge { get; private set; }

        public void Install(LanguagePack languagePack) {
            _dict.Clear();
            currentLanguge = Language.GetLanguageByCode(languagePack.llcode);
            var pack = languagePack.pack;
            for (int i = 0; i < pack.Count; i++) {
                var words = pack[i];
                _dict.Add(words.key,words.value);
            }
        }
    }
}