using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityKit {

    [Serializable]
    public class UnityPoolNode  {
        [SerializeField]
        private string _name = "<Please Set Name>";
        public string poolNodeName {
            get { return _name; }
            set { _name = value; }
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

        protected List<GameObject> _idleObjs = new List<GameObject>();
        protected List<GameObject> _usedObjs = new List<GameObject>();

        protected Action<GameObject> _onBeforeRent;
        public Action<GameObject> OnBreforeRent {
            get { return _onBeforeRent; }
            set { _onBeforeRent = value; }
        }

        protected Action<GameObject> _onBeforeReturn;
        public Action<GameObject> OnBreforeReturn {
            get { return _onBeforeReturn; }
            set { _onBeforeReturn = value; }
        }
        virtual protected void BeforeRent(GameObject go) {
            if (go == null) return;
            if (_onBeforeRent != null) _onBeforeRent.Invoke(go);
            else go.SetActive(true);
        }

        virtual protected void BeforeReturn(GameObject go) {
            if (go == null) return;
            if (_onBeforeReturn != null) _onBeforeReturn.Invoke(go);
            else go.SetActive(false);
        }

        protected Action<GameObject> _OnDestroy;
        public Action<GameObject> OnDestroy {
            get { return _OnDestroy; }
            set { _OnDestroy = value; }
        }

        public bool Contains(GameObject go) {
            return _idleObjs.Contains(go) || _usedObjs.Contains(go);
        }

        virtual protected GameObject CreateInstance() {
            var obj = GameObject.Instantiate(_prefab,_parent);
            obj.SetActive(false);
            return obj;
        }

        protected void Destroy(GameObject go) {
            if (go == null) return;
            if (_OnDestroy == null) _OnDestroy(go);
            else GameObject.Destroy(go);
        }

        public void Preload() {
            for (int i = _idleObjs.Count; i < Math.Min(capacity, _preloadCount); i++) {
                Return(CreateInstance());
            }
        }

        public IEnumerator PreloadItetor() {
            while (_idleObjs.Count < _preloadCount) {
                Return(CreateInstance());
                yield return null;
            }
        }

        virtual public GameObject Rent() {
            GameObject go;
            int count = _idleObjs.Count;
            if (count > 0) {
                go = _idleObjs[count - 1];
                _idleObjs.RemoveAt(count - 1);
                if (go == null) {
                    go = CreateInstance();
                }
            } else {
                go = CreateInstance();
            }
            _usedObjs.Add(go);
            BeforeRent(go);
            return go;
        }

        virtual public bool Return(GameObject go) {
            if (_usedObjs.Contains(go)) {
                _usedObjs.Remove(go);
                if (go != null) {
                    if (capacity > _idleObjs.Count) {
                        _idleObjs.Add(go);
                        BeforeReturn(go);
                    } else {
                        Destroy(go);
                    }
                }
                return true;
            }
            return false;
        }

        public void ReturnAll() {
            new List<GameObject>(_usedObjs).ForEach((x)=>Return(x));
        }

        public void Return(Predicate<GameObject> pred) {
            _usedObjs.FindAll(pred).ForEach(x=>Return(x));
        }

        public void Clear() {
            _usedObjs.ForEach(Destroy);
            _idleObjs.ForEach(Destroy);
            _usedObjs.Clear();
            _idleObjs.Clear();
        }

        virtual public void Dispose() {
            Clear();
            _onBeforeRent = null;
            _onBeforeReturn = null;
            _OnDestroy = null;
        }
    }
}