using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityKit {
    sealed public class LanguageMgr: Singleton<LanguageMgr> {
        Dictionary<string, string> _dict = new Dictionary<string, string>();
        List<Language> _langePacks = new List<Language>();
        public ISO6391.Lang currentLang { get; private set; }
        public ISO6391.Lang defaultLang { get; private set; }

        public string GetString(string key) {
            string ret;
            if (_dict.TryGetValue(key,out ret)) {
                return ret;
            }
            return key;
        }

        public bool TryGetString(string key, out string ret) {
            return _dict.TryGetValue(key, out ret);
        }

        public void Add(Language languagePack) {
            for (int i = _langePacks.Count - 1; i >= 0; i--) {
                var pack = _langePacks[i];
                if (pack.code == languagePack.code) {
                    _langePacks.RemoveAt(i);
                    if (pack == languagePack) continue;
#if UNITY_EDITOR
                    if (pack != null) {
                        if (Application.isPlaying) {
                            GameObject.Destroy(pack);
                        } else {
                            GameObject.DestroyImmediate(pack);
                        }
                    }
#else
                    if (pack != null) {
                        GameObject.Destroy(pack);
                    }
#endif
                }
            }

            if (languagePack.isDefault) defaultLang = ISO6391.GetLang(languagePack.code);
            _langePacks.Add(languagePack);
        }

        public void SetLanguage(SystemLanguage systemLanguage) { SetLanguage(ISO6391.GetCode(systemLanguage)); }

        public void SetLanguage(string codeOrName, bool UseDefaultIfNone = true) {
            _dict.Clear();
            Language language;
            if (ISO6391.IsCode(codeOrName)) {
                language = _langePacks.Find(x => x.code == codeOrName);
            } else if(ISO6391.IsLangName(codeOrName)){
                language = _langePacks.Find(x => x.langName == codeOrName);
            } else {
                throw new Exception(string.Format("Not Code ,Not LangName -> {0}", codeOrName));
            }

            currentLang = ISO6391.GetLang(codeOrName);

            if(language == null && UseDefaultIfNone) {
                language = _langePacks.Find(x=>x.code == codeOrName || x.langName == codeOrName);
            } else {
                throw new Exception(string.Format("Language Not Found. And Not Use Default -> {0}", codeOrName));
            }

            if(language == null) {
                throw new Exception(string.Format("Language Not Found. And No Default-> {0}", codeOrName));
            }

            var wordsPack = language.Words;
            for (int i = 0; i < wordsPack.Count; i++) {
                var words = wordsPack[i];
                _dict.Add(words.key, words.value);
            }
        }
    }
}