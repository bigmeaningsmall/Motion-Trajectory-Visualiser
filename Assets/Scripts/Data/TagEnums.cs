using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class TagEnums : MonoBehaviour{
    public Tags[] tags;

    public bool CheckForTag(Tags t){
        bool b = false;
        for (int i = 0; i < tags.Length; i++){
            if (tags[i] == t){
                b= true;
            }
            else{
                b= false;
            }
        }
        return b;
    }
}
