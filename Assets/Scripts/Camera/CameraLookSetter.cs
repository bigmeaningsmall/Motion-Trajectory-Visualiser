using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class CameraLookSetter : MonoBehaviour{
    
    public Transform lookTarget;

    public int vertMoves = 3;
    public int vertIndex = 0;
    public int hozMoves = 3;
    public int hozIndex = 0;

    public Vector3 initialPosition;
    private Vector3 tmpPos;

    private void Awake(){
        initialPosition = lookTarget.position;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CameraUp(){
        // +4
        if (vertIndex <= vertMoves){
            vertIndex++;
            lookTarget.DOMoveY(lookTarget.position.y+StaticData.instance.cameraMoveOffset, StaticData.instance.animationDuration * 10);
        }
    }
    // -3
    public void CameraDown(){
        if (vertIndex > -vertMoves){
            vertIndex--;
            lookTarget.DOMoveY(lookTarget.position.y-StaticData.instance.cameraMoveOffset, StaticData.instance.animationDuration * 10);
        }
    }
    public void CameraLeft(){
        // +6
        if (hozIndex < hozMoves*2){
            hozIndex++;
            lookTarget.DOMoveX(lookTarget.position.x+(StaticData.instance.cameraMoveOffset*2), StaticData.instance.animationDuration * 10);
        }
    }
    // -6
    public void CameraRight(){
        if (hozIndex > -hozMoves*2){
            hozIndex--;
            lookTarget.DOMoveX(lookTarget.position.x-(StaticData.instance.cameraMoveOffset*2), StaticData.instance.animationDuration * 10);
        }
    }
}
