using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonClickShaker : MonoBehaviour
{
    [HideInInspector]
    private float shakeAmmount = 0.2f;
    [HideInInspector]
    private float shakeStrength = 0.2f;
    
    void Start(){
        shakeAmmount = StaticData.instance.buttonShakeAmount;
        shakeStrength = StaticData.instance.buttonShakeStrength;
    }
    public void ButtonClick(){
        this.transform.DOShakeScale(shakeAmmount, shakeStrength);
    }
}
