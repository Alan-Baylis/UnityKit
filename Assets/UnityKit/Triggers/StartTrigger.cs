using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityKit {
    public class StartTrigger :BaseTrigger{
        protected override TriggerMoment callmonent {
            get {
                return TriggerMoment.Start;
            }
        }
    }
}
