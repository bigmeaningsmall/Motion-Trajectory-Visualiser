using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollower : MonoBehaviour{

    public bool followActive = true;

    [Range(0,1)]
    public float smoothSpeed = 0f;
    
    [Header("Target Transforms")]
    public Transform headTarget;
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    
    [Header("Avatar Transforms")]
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    
    [Header("Rotation Offsets")]
    public Vector3 headOffset;
    public Vector3 leftHandOffset;
    public Vector3 rightHandOffset;
    
    void Start()
    {
        
    }
    
    void Update(){
        if (followActive){
            head.transform.position = new Vector3(headTarget.position.x, headTarget.position.y, headTarget.position.z);
            leftHand.transform.position = new Vector3(leftHandTarget.position.x, leftHandTarget.position.y, leftHandTarget.position.z);
            rightHand.transform.position = new Vector3(rightHandTarget.position.x, rightHandTarget.position.y, rightHandTarget.position.z);

            head.eulerAngles = new Vector3(headTarget.eulerAngles.x + headOffset.x, headTarget.eulerAngles.y + headOffset.y, headTarget.eulerAngles.z + headOffset.z);
            leftHand.eulerAngles = new Vector3(leftHandTarget.eulerAngles.x + leftHandOffset.x, leftHandTarget.eulerAngles.y + leftHandOffset.y, leftHandTarget.eulerAngles.z + leftHandOffset.z);
            rightHand.eulerAngles = new Vector3(rightHandTarget.eulerAngles.x + rightHandOffset.x, rightHandTarget.eulerAngles.y + rightHandOffset.y, rightHandTarget.eulerAngles.z + rightHandOffset.z);
        }

    }
}
