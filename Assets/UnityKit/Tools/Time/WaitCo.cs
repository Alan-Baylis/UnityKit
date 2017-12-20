using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityKit.Tools {

    public enum WaitType {
        Seconds,
        SecondsRealtime,
        Frames,
        FixUpdate
    }

    static public class WaitCo {

        static public Coroutine Run(this IEnumerator wait) {
            return TimerTrigger.Instance.StartCoroutine(wait);
        }

        static public IEnumerator Append(this IEnumerator wait, IEnumerator nextWait) {
            yield return wait;
            yield return nextWait;
        }

        static public IEnumerator Append(this IEnumerator wait, Action act) {
            yield return wait;
            act.Invoke();
        }

        static public IEnumerator Wait(this Action act, Func<bool> until) {
            yield return new WaitUntil(until);
            act.Invoke();
        }

        static public IEnumerator Wait(this IEnumerator wait, Func<bool> until) {
            yield return new WaitUntil(until);
            yield return wait;
        }

        static public IEnumerator Wait(this Action act, float num, WaitType type = WaitType.Seconds, int loop = 1) {
            for (int c = 0; c < loop; c++) {
                switch (type) {
                    case WaitType.Seconds:
                        yield return new WaitForSeconds(num);
                        break;
                    case WaitType.SecondsRealtime:
                        yield return new WaitForSecondsRealtime(num);
                        break;
                    case WaitType.Frames:
                        int frameCount = Mathf.CeilToInt(num);
                        for (int i = 0; i < frameCount; i++) {
                            yield return new WaitForEndOfFrame();
                        }
                        break;
                    case WaitType.FixUpdate:
                        int fixUpdateCount = Mathf.CeilToInt(num);
                        for (int i = 0; i < fixUpdateCount; i++) {
                            yield return new WaitForFixedUpdate();
                        }
                        break;
                }

                act.Invoke();
            }
        }

        static public IEnumerator Wait(this IEnumerator wait, float num, WaitType type = WaitType.Seconds, int loop = 1) {
            switch (type) {
                case WaitType.Seconds:
                    yield return new WaitForSeconds(num);
                    break;
                case WaitType.SecondsRealtime:
                    yield return new WaitForSecondsRealtime(num);
                    break;
                case WaitType.Frames:
                    int frameCount = Mathf.CeilToInt(num);
                    for (int i = 0; i < frameCount; i++) {
                        yield return new WaitForEndOfFrame();
                    }
                    break;
                case WaitType.FixUpdate:
                    int fixUpdateCount = Mathf.CeilToInt(num);
                    for (int i = 0; i < fixUpdateCount; i++) {
                        yield return new WaitForFixedUpdate();
                    }
                    break;
            }
            yield return wait;
        }
    }
}
