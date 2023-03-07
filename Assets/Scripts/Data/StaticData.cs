using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static StaticData instance;

    [Header("CONFIG")]
    public int fixedFrameRate = 60;
    public string versionNumber;

    [Header("ANIMATION")] 
    public float animationDuration = 0.25F;
    public float buttonShakeAmount = 0.2F;
    public float buttonShakeStrength = 0.25F;
    
    
    [Header("COLOURS")] 
    public Color32 UI_Default;
    public Color32 UI_Selected;
    public Color32 UI_SelectedTarget;
    public Color32 UI_SelectedPredicted;

//******************************
    void Awake(){
        if (instance != null) {
            Destroy(gameObject);
        }else{
            instance = this;
        }
    }
}
