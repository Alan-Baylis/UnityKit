using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityKit {
    public class LanguagePack : ScriptableObject, ISerializationCallbackReceiver {

        string _llcode = string.Empty;
        public string llcode {
            get { return _llcode; }
            set {
                if (!Language.IsLanguageCode(value)) throw new Exception("language locale code is error");
                _llcode = value;
            }
        }

        [SerializeField]
        List<LanguageWords> _pack = new List<LanguageWords>();
        public List<LanguageWords> pack { get { return _pack; } }
        
        public void Add(string key, string value, string des) {
            if (!_pack.TrueForAll(x => x.key != key)) throw new Exception(string.Format("language repeated -> key:{0}", key));
            _pack.Add(new LanguageWords() {
                key = key,
                value = value
            });
        }

        public void Add(LanguageWords words) {
            if (!_pack.TrueForAll(x => x.key != words.key || x == words)) throw new Exception(string.Format("language repeated -> key:{0}", words.key));
            _pack.Add(words);
        }

        public void OnBeforeSerialize() {
            if (!Language.IsLanguageCode(_llcode)) throw new Exception("language locale code is error");
        }

        public void OnAfterDeserialize() {

        }
    }
}
