using System;
using System.Collections.Generic;

namespace UnityKit {
    /// <summary>
    /// 非线程安全对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjPool<T> : IDisposable {
        public const int DEFAULT_CAPACITY = 20;
        protected List<T> _idleObjs;
        protected List<T> _usedObjs;

        protected Func<T> _factory = null;
        public Func<T> Factory {
            get { return _factory; }
            set { _factory = value; }
        }

        T CreateObj() {
            return _factory != null ? (T)Activator.CreateInstance(typeof(T), true) : _factory.Invoke();
        }

        protected Action<T> _onBeforeRent;
        public Action<T> OnBeforeRent {
            get { return _onBeforeRent; }
            set { _onBeforeRent = value; }
        }

        protected Action<T> _onBeforeReturn;
        public Action<T> OnBeforeReturn {
            get { return _onBeforeReturn; }
            set { _onBeforeReturn = value; }
        }

        protected Action<T> _onBeforeDestroy;
        public Action<T> OnBeforeDestroy {
            get { return _onBeforeDestroy; }
            set { _onBeforeDestroy = value; }
        }

        int _capacity = 10;
        public int Capacity {
            get { return _capacity; }
            protected set { _capacity = value; }
        }

        public ObjPool() : this(DEFAULT_CAPACITY) { }
        public ObjPool(int capacity) : this(capacity, () => (T)Activator.CreateInstance(typeof(T), true)) { }
        public ObjPool(int capacity, Func<T> factory) : this(capacity, factory, null, null, null) { }
        public ObjPool(int capacity, Func<T> factory, Action<T> onBeforeRent = null, Action<T> onBeforeReturn = null, Action<T> onDestroy = null) {
            if (capacity < 0) capacity = DEFAULT_CAPACITY;
            _idleObjs = new List<T>(capacity);
            _usedObjs = new List<T>(capacity);
            _capacity = capacity;
            _factory = factory;
            _onBeforeRent = onBeforeRent;
            _onBeforeReturn = onBeforeReturn;
            _onBeforeDestroy = onDestroy;
        }

        public void Preload(int count) {
            for (int i = _idleObjs.Count; i < Math.Min(Capacity, count); i++) {
                Return(CreateObj());
            }
        }

        virtual public T Rent() {
            T obj;
            int count = _idleObjs.Count;
            if (count > 0) {
                obj = _idleObjs[count - 1];
                _idleObjs.RemoveAt(count - 1);
                if (obj.Equals(default(T))) {
                    obj = _factory();
                }
            } else {
                obj = _factory();
            }
            _usedObjs.Add(obj);
            if (_onBeforeRent != null) _onBeforeRent.Invoke(obj);
            return obj;
        }

        virtual public void Return(T obj) {
            if (_usedObjs.Contains(obj)) {
                _usedObjs.Remove(obj);
                if (obj != null) {
                    if (Capacity > _idleObjs.Count) {
                        _idleObjs.Add(obj);
                        if (_onBeforeReturn != null) _onBeforeReturn.Invoke(obj);
                    } else {
                        if (_onBeforeDestroy != null) _onBeforeDestroy.Invoke(obj);
                    }
                }
            }
        }

        public void Return(Predicate<T> pred) {
            new List<T>(_usedObjs).FindAll(pred).ForEach(Return);
        }

        public void ReturnAll() {
            new List<T>(_usedObjs).ForEach(Return);
        }

        public void Clear() {
            if(_onBeforeDestroy != null) {
                _usedObjs.ForEach(_onBeforeDestroy);
                _idleObjs.ForEach(_onBeforeDestroy);
            }
            _usedObjs.Clear();
            _idleObjs.Clear();
        }

        virtual public void Dispose() {
            Clear();
            _factory = null;
            _onBeforeRent = null;
            _onBeforeReturn = null;
            _onBeforeDestroy = null;
        }
    }
}