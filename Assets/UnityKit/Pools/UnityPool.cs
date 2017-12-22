using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityKit {
    public class UnityPool : MonoBehaviour, IDisposable {

        [SerializeField]
        private bool _dontDestroyOnload = false;
        public bool dontDestryOnLoad {
            get { return _dontDestroyOnload; }
            set { _dontDestroyOnload = value; }
        }

        [SerializeField]
        private bool _autoRecyle = false;
        public bool autoRecyle {
            get { return _autoRecyle; }
            set { _autoRecyle = value; }
        }

        [SerializeField]
        private float _autoRecyleInterval = 1f;
        public float autoRecyleInterval {
            get { return _autoRecyleInterval; }
            set { _autoRecyleInterval = value; }
        }

        [SerializeField]
        List<UnityPoolNode> _poolNodes = new List<UnityPoolNode>();

        public Action onInit;

        virtual protected void Awake() {
            CheckNodeName();
            if (_dontDestroyOnload) DontDestroyOnLoad(this);
            StartCoroutine(Preload());
            if (autoRecyle) StartCoroutine(AutoRecyle());
        }

        void CheckNodeName() {
            List<string> nodeNames = new List<string>();
            foreach (var node in _poolNodes) {
                if (nodeNames.Contains(node.poolNodeName)) {
                    throw new Exception("please set uniq poolNodeName");
                }
                nodeNames.Add(node.poolNodeName);
            }
        }

        IEnumerator Preload() {
            foreach (var node in _poolNodes) {
                if (node.preLoadAsync) {
                    yield return node.PreloadItetor();
                } else {
                    node.Preload();
                }
            }

            if (onInit != null) onInit.Invoke();
        }

        IEnumerator AutoRecyle() {
            yield return new WaitForSecondsRealtime(_autoRecyleInterval);
            foreach (var node in _poolNodes) {
                node.Return(x => !x.activeSelf);
            }
        }

        public bool AddPoolNode(UnityPoolNode node) {
            if (_poolNodes.Contains(node) && null != _poolNodes.Find(x => x.poolNodeName == node.poolNodeName)) {
                throw new Exception("please set uniq poolNodeName");
            }
            _poolNodes.Add(node);
            return true;
        }

        public void RemovePoolNode(string poolNodeName) {
            for (int i = 0; i < _poolNodes.Count; i++) {
                var node = _poolNodes[i];
                if (node.poolNodeName == poolNodeName) {
                    node.Dispose();
                    _poolNodes.RemoveAt(i);
                    break;
                }
            }
        }

        public UnityPoolNode GetPoolNode(Predicate<UnityPoolNode> pred) {
            return _poolNodes.Find(pred);
        }

        public GameObject Rent(string poolNodeName) {
            foreach (var node in _poolNodes) {
                if (node.poolNodeName == poolNodeName) {
                    return node.Rent();
                }
            }
            return null;
        }

        public GameObject Rent(GameObject prefab) {
            foreach (var node in _poolNodes) {
                if (node.prefab == prefab) {
                    return node.Rent();
                }
            }
            return null;
        }

        public bool Return(GameObject obj) {
            foreach (var node in _poolNodes) {
                if (node.Return(obj)) {
                    return true;
                }
            }
            return false;
        }

        public void Clear() {
            _poolNodes.ForEach(x=>x.Clear());
        }

        virtual protected void OnDestroy() {
            _poolNodes.ForEach(x => x.Dispose());
        }

        virtual public void Dispose() {
            if(this.isActiveAndEnabled) Destroy(this);
            _poolNodes.ForEach(x => x.Dispose());
        } 
    }
}
