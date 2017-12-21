using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public bool TryGetValue(string key, out string value) {
            return _dict.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() { return _dict.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _dict.GetEnumerator(); }
#endregion

        public Language currentLanguge { get; private set; }

        List<LanguagePack> _langePacks = new List<LanguagePack>();
        LanguagePack _defaultLangPack;
        LanguagePack _currentLangPack;

        public void Install(LanguagePack languagePack) {

            for (int i = _langePacks.Count - 1; i >= 0; i--) {
                var oldPack = _langePacks[i];
                if (oldPack.llcode == languagePack.llcode) {
                    if (_defaultLangPack == oldPack) _defaultLangPack = null;
                    _langePacks.RemoveAt(i);
                    if (oldPack == languagePack) continue;
#if UNITY_EDITOR
                    if (oldPack != null) {
                        if (Application.isPlaying) {
                            GameObject.Destroy(oldPack);
                        } else {
                            GameObject.DestroyImmediate(oldPack);
                        }
                    }
#else
                    if (oldPack != null) {
                        GameObject.Destroy(oldPack);
                    }
#endif
                }
            }
            if (languagePack.isDefault) _defaultLangPack = languagePack;
            _langePacks.Add(languagePack);
        }

        public void SetCurrentLanguage(SystemLanguage language) { SetCurrentLanguage(Language.GetLanguageBySL(language)); }
        public void SetCurrentLanguage(Language language) { SetCurrentLanguage(language.code); }
        public void SetCurrentLanguage(string llcode) {

            _dict.Clear();
            _currentLangPack = _langePacks.Find(x=>x.llcode == llcode);
            if (_currentLangPack == null) _currentLangPack = _defaultLangPack;
            if (_currentLangPack == null) Debug.LogWarning("Language is not found");

            var wordsPack = _currentLangPack.pack;
            for (int i = 0; i < wordsPack.Count; i++) {
                var words = wordsPack[i];
                _dict.Add(words.key, words.value);
            }
        }
    }
}