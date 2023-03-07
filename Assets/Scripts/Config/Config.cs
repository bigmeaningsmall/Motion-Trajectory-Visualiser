using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour{

    private void Awake(){
        
    }

    void Start(){
        Time.fixedDeltaTime = (1f/StaticData.instance.fixedFrameRate);
        
    }

}
