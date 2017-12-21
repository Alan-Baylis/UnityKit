/**
 * 由于Dictionary无法序列化。
 * 采用双List暂存数据,运行时在构造真实DIctionary
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Collections.Generic {

    public interface IMap {
        void Add(object key, object value);
        void Clear();
        bool Contains(object key);
        void Remove(object key);
        object this[object key] { get;set; }
    }

    [Serializable]
    public class Map<TKey, TValue> : IDictionary<TKey, TValue>, IMap {

        [NonSerialized]
        Dictionary<TKey, TValue> _dict;

        List<TKey> _keys = new List<TKey>();
        List<TValue> _values = new List<TValue>();

        public ICollection<TKey> Keys { get { return _dict.Keys; } }
        public ICollection<TValue> Values { get { return _dict.Values; } }
        public int Count { get { return _dict.Count; } }
        public bool IsReadOnly { get { return false; } }
        public TValue this[TKey key] { get { return _dict[key]; } set { _dict[key] = value; } }
        public object this[object key] { get { return this[(TKey)key]; } set { this[(TKey)key] = (TValue)value; } }

        public Map() { _dict = new Dictionary<TKey, TValue>(); }
        public Map(int capcity) { _dict = new Dictionary<TKey, TValue>(capcity); }
        public Map(SerializationInfo info, StreamingContext context) { _dict = new Dictionary<TKey, TValue>(); }

        public void Add(KeyValuePair<TKey, TValue> item) { _dict.Add(item.Key, item.Value); }
        public void Add(TKey key, TValue value) { _dict.Add(key, value); }
        public void Add(object key, object value) { Add((TKey)key, (TValue)value); }
        public bool ContainsKey(TKey key) { return _dict.ContainsKey(key); }
        public bool Contains(KeyValuePair<TKey, TValue> item) { return _dict.ContainsKey(item.Key) && _dict[item.Key].Equals(item.Value);}
        public bool Contains(object key) { return _dict.ContainsKey((TKey)key); }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) { throw new System.NotImplementedException(); }
        public void Clear() { if(_dict !=null) _dict.Clear();}
        public bool Remove(TKey key) { return _dict.Remove(key); }
        public bool Remove(KeyValuePair<TKey, TValue> item) { return _dict.Remove(item.Key); }
        public void Remove(object key) { Remove((TKey)key); }
        public bool TryGetValue(TKey key, out TValue value) { return _dict.TryGetValue(key, out value); }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() { return _dict.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _dict.GetEnumerator(); }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext cnx) {
            _dict = new Dictionary<TKey, TValue>();
            int count = Math.Min(_keys.Count, _values.Count);
            for (int i = 0; i < count; i++) {
                _dict.Add(_keys[i], _values[i]);
            }
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext cnx) {
            _keys.Clear();
            _values.Clear();
            foreach (var key in _dict.Keys) {
                _keys.Add(key);
                _values.Add(_dict[key]);
            }
        }
    }
}