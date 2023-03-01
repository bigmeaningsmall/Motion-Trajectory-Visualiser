using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TransformUtilities : MonoBehaviour{
    
    public static TransformUtilities transformUtilities;
    
    //distance between 2 transforms
    public static float GetDistance(Transform a, Transform b){
        return (b.position - a.position).magnitude;
    }

    //position between 2 transforms
    public static Vector3 GetPosition(Transform a, Transform b){
        return b.position - a.position;
    }
    
}