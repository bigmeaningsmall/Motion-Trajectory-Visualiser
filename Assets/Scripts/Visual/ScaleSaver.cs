using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSaver : MonoBehaviour{
    
    [HideInInspector]
    public Vector3 initialScale;
    void Start(){
        initialScale = transform.localScale;
    }
    
}
