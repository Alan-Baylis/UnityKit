using System;
using System.Collections.Generic;

namespace UnityKit.Tools {
    public class SingletonContainer {
        Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        public void Add<T>(T instance) { _instances.Add(typeof(T), instance); }
        public void Add(object instance) { _instances.Add(instance.GetType(), instance); }

        public void Remove<T>() { _instances.Remove(typeof(T)); }
        public void Remove(Type type) { _instances.Remove(type); }

        public T Get<T>() { return (T) _instances[typeof(T)]; }
        public object Get(Type type) { return _instances[type]; }

        public bool TryGetValue<T>(out T ret) {
            object obj = null;
            bool has = _instances.TryGetValue(typeof(T), out obj);
            ret = has ? (T)obj:default(T);
            return has;
        }

        public bool TryGetValue(Type type, out object ret) {
            return _instances.TryGetValue(type, out ret);
        }

        public void Clear() {
            _instances.Clear();
        }
    }
}