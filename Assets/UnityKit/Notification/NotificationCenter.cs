using System;
using System.Collections.Generic;
using UnityEngine;
namespace UnityKit {
    sealed public class NotificationCenter : MonoSingleton<NotificationCenter> {

        List<IObserver> _observers = new List<IObserver>();
        public void AddObserver(IObserver observer) {
            if (!_observers.Contains(observer)) {
                _observers.Add(observer);
            }
        }

        public void RemoveObserver(IObserver observer) {
            _observers.Remove(observer);
        }


        Queue<Notification> _expireNotifications = new Queue<Notification>();
        Queue<Notification> _sendNotifications = new Queue<Notification>();
        public void SentNotification(object sender, string message, object data) {
            var notification = ObjPool<Notification>.Instance.Rent();
            _sendNotifications.Enqueue(notification);
        }

        private void FixedUpdate() {
            int last = _observers.Count - 1;
            if(last >= 0) {
                while (_sendNotifications.Count > 0) {
                    var notification = _sendNotifications.Dequeue();
                    for (int i = 0; i <= last; i++) {
                        var observer = _observers[i];
                        if (observer.subjects.Contains(notification.subject)) {
                            if (i == last) observer.HandNotification(notification);
                            else _observers[last].HandNotification(notification.Clone());
                        }
                    }
                }
            }
        }

        private void LateUpdate() {
            
        }
    }
}