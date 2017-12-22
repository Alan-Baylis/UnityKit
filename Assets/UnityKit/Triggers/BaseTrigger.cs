using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityKit {

    public enum TriggerMoment {
        Awake,
        Start,
        Destroy
    }

    abstract public class BaseTrigger : MonoBehaviour {

        public Action Callback;

        protected void Awake() {
            if (callmonent == TriggerMoment.Awake) Callback.Invoke();
        }

        protected void Start() {
            if (callmonent == TriggerMoment.Start) Callback.Invoke();
        }

        protected void OnDestroy() {
            if (callmonent == TriggerMoment.Destroy) Callback.Invoke();
        }
        abstract protected TriggerMoment callmonent { get; }
    }
}