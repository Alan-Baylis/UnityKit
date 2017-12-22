using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityKit {
    public interface IObserver {
        bool isActive { get; }
        IList<string> subjects { get; }
        void HandNotification(Notification notification);
        void SendNotification(string message, object data);
    }
}
