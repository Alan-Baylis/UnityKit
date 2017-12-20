using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityKit.Tools;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //WaitCo.Action(Do).Wait(2, WaitType.Seconds, 10).Append(Update).Run();
        ((Action)Do).Wait(2, WaitType.Seconds, 10).Wait(()=>Time.time > 3).Run();
    }

    void Do() {
        Debug.LogWarning("Call");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
