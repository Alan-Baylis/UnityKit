using System;
using System.Collections.Generic;

namespace UnityKit {

    /// <summary>
    /// 普通对象池。不建议缓存Unity Object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjPool<T> : IDisposable {
        public const int DEFAULT_CAPACITY = 20;
        protected List<T> _idleItems;
        protected List<T> _usedItems;
        protected Func<T> _factory;
        protected Action<T> _onCreate;
        protected Action<T> _onReturn;
        protected Action<T> _onDestroy;

        int _capacity = 10;
        public int Capacity {
            get { return _capacity; }
            protected set { _capacity = value; }
        }

        public ObjPool() : this(DEFAULT_CAPACITY) { }
        public ObjPool(int capacity) : this(capacity, () => (T)Activator.CreateInstance(typeof(T), true)) { }
        public ObjPool(int capacity, Func<T> factory) : this(capacity, factory, null, null, null) { }
        public ObjPool(int capacity, Func<T> factory, Action<T> onCreate = null, Action<T> onReturn = null, Action<T> onDestroy = null) {
            if (capacity < 0) capacity = DEFAULT_CAPACITY;
            _idleItems = new List<T>(capacity);
            _usedItems = new List<T>(capacity);
            _capacity = capacity;
            _factory = factory;
            if (_factory == null) {
                _factory = () => (T)Activator.CreateInstance(typeof(T), true);
            }
            _onCreate = onCreate;
            _onReturn = onReturn;
            _onDestroy = onDestroy;
        }

        public void Preload(int count) {
            for (int i = _idleItems.Count; i < Math.Min(Capacity, count); i++) {
                Return(_factory());
            }
        }

        virtual public T Rent() {
            T obj;
            int count = _idleItems.Count;
            if (count > 0) {
                obj = _idleItems[count - 1];
                _idleItems.RemoveAt(count - 1);
                if (obj.Equals(default(T))) {
                    obj = _factory();
                }
            } else {
                obj = _factory();
            }
            _usedItems.Add(obj);
            if (_onCreate != null) _onCreate.Invoke(obj);
            return obj;
        }

        virtual public void Return(T obj) {
            if (_usedItems.Contains(obj)) {
                _usedItems.Remove(obj);
                if (obj != null) {
                    if (Capacity > _idleItems.Count) {
                        _idleItems.Add(obj);
                        if (_onReturn != null) _onReturn.Invoke(obj);
                    } else {
                        if (_onDestroy != null) _onDestroy.Invoke(obj);
                    }
                }
            }
        }

        public void Return(Predicate<T> pred) {
            new List<T>(_usedItems).FindAll(pred).ForEach(Return);
        }

        public void ReturnAll() {
            new List<T>(_usedItems).ForEach(Return);
        }

        public void Clean() {
            if(_onDestroy != null) {
                _usedItems.ForEach(_onDestroy);
                _idleItems.ForEach(_onDestroy);
            }
            _usedItems.Clear();
            _idleItems.Clear();
        }

        virtual public void Dispose() {
            Clean();
            _factory = null;
            _onCreate = null;
            _onReturn = null;
            _onDestroy = null;
        }
    }
}