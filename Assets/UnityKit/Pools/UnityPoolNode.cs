using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityKit {

    [Serializable]
    public class UnityPoolNode  {

        [SerializeField]
        private string _poolNodeName = "?";
        public string poolNodeName {
            get { return _poolNodeName; }
            set { _poolNodeName = value; }
        }

        [SerializeField]
        private GameObject _prefab;
        public GameObject prefab {
            get { return _prefab; }
            set { _prefab = value; }
        }

        [SerializeField]
        private int _capacity = 1;
        public int capacity {
            get { return _capacity; }
            private set { _capacity = value; }
        }

        [SerializeField]
        private Transform _parent = null;
        public Transform parent {
            get { return _parent; }
            set { parent = value; }
        }


        [SerializeField]
        private int _preloadCount = 0;
        public int preloadCount {
            get { return _preloadCount; }
            set {
                if (value < 0) {
                    Debug.LogWarning("count must be > 0");
                    value = 0;
                }
                _preloadCount = value;
            }
        }

        [SerializeField]
        private bool _preLoadAsync = false;
        public bool preLoadAsync {
            get { return _preLoadAsync; }
            set { _preLoadAsync = value; }
        }

        protected List<GameObject> _idleItems = new List<GameObject>();
        protected List<GameObject> _usedItems = new List<GameObject>();

        public Action<GameObject> onRent;
        public Action<GameObject> onReturn;
        public Action<GameObject> CustomDestroy;

        public bool Contains(GameObject obj) {
            return _idleItems.Contains(obj) || _usedItems.Contains(obj);
        }

        protected GameObject CreateGameObject() {
            var obj = GameObject.Instantiate(_prefab,_parent);
            obj.SetActive(false);
            return obj;
        }

        protected void DestroyGameObject(GameObject go) {
            if (go == null) return;
            if (CustomDestroy == null) CustomDestroy(go);
            else GameObject.Destroy(go);
        }

        public void Preload() {
            for (int i = _idleItems.Count; i < Math.Min(capacity, _preloadCount); i++) {
                Return(CreateGameObject());
            }
        }

        public IEnumerator PreloadItetor() {
            while (_idleItems.Count < _preloadCount) {
                Return(CreateGameObject());
                yield return null;
            }
        }

        virtual public GameObject Rent() {
            GameObject obj;
            int count = _idleItems.Count;
            if (count > 0) {
                obj = _idleItems[count - 1];
                _idleItems.RemoveAt(count - 1);
                if (obj == null) {
                    obj = CreateGameObject();
                }
            } else {
                obj = CreateGameObject();
            }
            _usedItems.Add(obj);
            if (onRent != null) onRent.Invoke(obj);
            return obj;
        }

        virtual public bool Return(GameObject obj) {
            if (_usedItems.Contains(obj)) {
                _usedItems.Remove(obj);
                if (obj != null) {
                    if (capacity > _idleItems.Count) {
                        _idleItems.Add(obj);
                        if (onReturn != null) onReturn.Invoke(obj);
                    } else {
                        DestroyGameObject(obj);
                    }
                }
                return true;
            }
            return false;
        }

        public void ReturnAll() {
            new List<GameObject>(_usedItems).ForEach((x)=>Return(x));
        }

        public void Return(Predicate<GameObject> pred) {
            _usedItems.FindAll(pred).ForEach(x=>Return(x));
        }

        void Clean() {
            _usedItems.ForEach(DestroyGameObject);
            _idleItems.ForEach(DestroyGameObject);
            _usedItems.Clear();
            _idleItems.Clear();
        }

        virtual public void Dispose() {
            Clean();
            onRent = null;
            onReturn = null;
            CustomDestroy = null;
        }
    }
}