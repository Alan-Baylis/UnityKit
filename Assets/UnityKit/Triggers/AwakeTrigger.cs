using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityKit {
    public class AwakeTrigger : BaseTrigger {
        protected override TriggerMoment callmonent {
            get {
                return TriggerMoment.Awake;
            }
        }
    }
}
