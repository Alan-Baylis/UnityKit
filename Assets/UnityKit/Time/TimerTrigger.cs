using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityKit {
    public class TimerTrigger : MonoSingleton<TimerTrigger> {
        List<Timer> _realTimers = new List<Timer>();
        List<Timer> _scaleTimers = new List<Timer>();
        List<Timer> _unsedTimers = new List<Timer>();

        public bool IsPuase { get; set; }

        public void Add(Timer timer) {
            if (timer.realTime) {
                _realTimers.Add(timer);
            } else {
                _scaleTimers.Add(timer);
            }
        }

        private void FixedUpdate() {
            if (IsPuase) return;
            int rCount = _realTimers.Count;
            for (int i = 0; i < rCount; i++) {
                _realTimers[i].Update(Time.fixedUnscaledDeltaTime);
            }

            for (int i = rCount - 1; i >= 0; i--) {
                var timer = _realTimers[i];
                if (timer.isUnsed) {
                    _unsedTimers.Add(timer);
                    _realTimers.RemoveAt(i);
                }
            }

            int sCount = _scaleTimers.Count;
            for (int i = 0; i < sCount; i++) {
                _scaleTimers[i].Update(Time.fixedDeltaTime);
            }

            for (int i = sCount - 1; i >= 0; i--) {
                var timer = _scaleTimers[i];
                if (timer.isUnsed) {
                    _unsedTimers.Add(timer);
                    _scaleTimers.RemoveAt(i);
                }
            }
        }

        private void LateUpdate() {
            for (int i = 0; i < _unsedTimers.Count; i++) {
                _unsedTimers[i].Dispose();
            }
            _unsedTimers.Clear();
        }
    }
}