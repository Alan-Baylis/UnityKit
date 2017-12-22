using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityKit {
    public class Notification {
        public object sender;
        public string subject;
        public object data;
        public Notification(object sender, string subject, object data) {
            this.sender = sender;
            this.subject = subject;
            this.data = data;
        }

        public Notification Clone() {
            return new Notification(sender, subject, data);
        }
    }
}