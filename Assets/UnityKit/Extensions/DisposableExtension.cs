using System;
using UnityEngine;

namespace UnityKit {
    public static class DisposableExtension {
        /// <summary>
        /// 使disposable 依附于gameobject 上，gameobject destroy时候调用dispose 方法
        /// </summary>
        /// <param name="disposable"></param>
        /// <param name="go"></param>
        public static void Attach(this IDisposable disposable, GameObject go) {
            (go.GetComponent<DestroyTrigger>()??go.AddComponent<DestroyTrigger>()).Callback = disposable.Dispose;
        }
    }
}
