using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityKit {
    public class DestroyTrigger : BaseTrigger {
        protected override TriggerMoment callmonent {
            get { return TriggerMoment.Destroy; }
        }
    }
}