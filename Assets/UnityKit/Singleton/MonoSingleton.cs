using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityKit {
    abstract public class MonoSingleton<T>: MonoBehaviour where T: MonoSingleton<T> {
        static T _instance;
        public static T Instance {
            get {
                if (_instance == null) {
                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                }
                return _instance;
            }
        }

        virtual protected void Awake() {
            if (_instance == null) {
                _instance = this as T;
            } else {
                Debug.LogWarning(this.GetType().Name +"-> Singleton instance exits.");
                Destroy(this);
            }
        }
    }
}