using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityKit {
    public class Timer : IDisposable {

        float _counter;
        bool _isDestroy = false;
        TimerTrigger _customTrigger = null;

        bool _isCompleted = false;
        public bool isUnsed { get { return _isCompleted || _isDestroy; } }

        bool _running = false;
        public bool running {
            get { return _running; }
            set {
                if (_isDestroy) throw new Exception("Destroyed");
                _running = value;
            }
        }
        
        float _curTime;
        public float curTime { get { return _curTime; } }

        float _interval;
        public float interval { get { return _interval; } }

        bool _realTime = true;
        public bool realTime {
            get { return _realTime; }
            set { _realTime = value; }
        }

        int _repeatCount = 1;
        public int repeatCount {
            get { return _repeatCount; }
        }

        int _currentCount = 0;
        public int currentCount {
            get { return _currentCount; }
        }

        public Timer(
            float interval, 
            int repeat = int.MaxValue, 
            bool isRealTime = true, 
            TimerTrigger trigger = null) {
            _interval = interval;
            _repeatCount = repeat;
            _realTime = isRealTime;
            _customTrigger = trigger;
        }

        public Action onTime;
        public Action onComplete;

        bool _isStarted = false;
        public void Start() {
            if (_isDestroy || _isStarted) return;
            _isStarted = true;
            _counter = 0f;
            _running = true;
            if (_customTrigger != null) _customTrigger.Add(this);
            else TimerTrigger.Instance.Add(this);
        }

        public void Stop() {
            if (_isDestroy) return;
            _isCompleted = true;
            _running = false;
        }

        public void Dispose() {
            _isDestroy = true;
            _isStarted = false;
            _repeatCount = int.MaxValue;
            _counter = 0;
            _curTime = 0;
            _currentCount = 0;
            _isCompleted = false;
            _customTrigger = null;
            onTime = null;
            onComplete = null;
        }

        internal void Update(float deltaTime) {
            if (_running && !_isDestroy) {
                _counter += deltaTime;
                _curTime += deltaTime;
                if(_counter > _interval) {
                    _currentCount += 1;
                    _counter = _counter - _interval;
                    if(onTime != null) onTime.Invoke();
                }

                if (_currentCount >= _repeatCount) {
                    if (onComplete != null) onComplete.Invoke();
                    _running = false;
                    _isCompleted = true;
                }
            }
        }
    }
}