using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityKit.Tools {

    public class Singleton<T> where T:class, new(){
        static T _instance;
        static readonly object _lockObj;
        
        static Singleton() {
            _lockObj = new object();
        }

        public static T Instance {
            get {
                if(_instance == null) {
                    lock (_lockObj) {
                        _instance = new T();
                    }
                }
                return _instance;
            }
        }
    }
}