using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityKit {
    public class Language : ScriptableObject, ISerializationCallbackReceiver {
        [Serializable]
        public struct Word {
            public string key;
            public string value;
        }

        [ReadOnly]
        [SerializeField]
        string _code = string.Empty;
        public string code {
            get { return _code; }
            set {
                if (!ISO6391.IsCode(value)) throw new Exception("language locale code is error");
                _code = value;
                _langName = ISO6391.GetName(value);
            }
        }

        [ReadOnly]
        [SerializeField]
        string _langName = string.Empty;
        public string langName {
            get { return _langName; }
            set {
                if (!ISO6391.IsLangName(value)) throw new Exception("language locale name is error");
                _langName = value;
                _code = ISO6391.GetCode(value);
            }
        }

        [SerializeField]
        bool _isDefault;
        public bool isDefault {
            get { return _isDefault; }
            set { _isDefault = value; }
        }

        [HideInInspector]
        [SerializeField]
        List<Word> _words = new List<Word>();
        public List<Word> Words { get { return _words; } }
        
        public void Add(string key, string value) {
            if (!_words.TrueForAll(x => x.key != key)) {
                Debug.LogWarning(string.Format("language repeated -> key:{0}", key));
                return;
            }
            _words.Add(new Word() {
                key = key,
                value = value
            });
        }

        public void Add(Word words) {
            if (!_words.TrueForAll(x => x.key != words.key || x.Equals(words))) {
                Debug.LogWarning(string.Format("language repeated -> key:{0}", words.key));
                return;
            }
            _words.Add(words);
        }

        public void OnBeforeSerialize() {
            if (string.IsNullOrEmpty(_code) ||
                string.IsNullOrEmpty(_langName) ||
                !ISO6391.IsCode(_code) || 
                !ISO6391.IsLangName(_langName)) {
                throw new Exception("language code or name is error");
            }
        }

        public void OnAfterDeserialize() { }
    }
}
