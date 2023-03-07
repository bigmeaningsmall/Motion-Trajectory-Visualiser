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
    
    
    public static Transform[] RemoveTransformsByTag(Transform[] transforms, string tag)
    {
        // create a new array to hold the filtered transforms
        Transform[] filteredTransforms = new Transform[transforms.Length];
        int index = 0;

        // loop through each transform in the original array
        for (int i = 0; i < transforms.Length; i++)
        {
            // if the transform's tag does not match the tag to remove, add it to the filtered array
            if (transforms[i].gameObject.tag != tag)
            {
                filteredTransforms[index] = transforms[i];
                index++;
            }
        }

        // resize the filtered array to remove any null elements
        System.Array.Resize(ref filteredTransforms, index);

        return filteredTransforms;
    }
}