using System;

namespace UnityKit {

    public class Singleton<T> where T : class {
        public static T Instance {
            get { return InstanceWrapper<T>.instance; }
        }

        internal class InstanceWrapper<Cls> where Cls : class {
            static InstanceWrapper() { }
            internal static readonly Cls instance = (Cls)Activator.CreateInstance(typeof(Cls),true);
        }
    }
}