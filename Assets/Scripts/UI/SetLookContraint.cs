using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class SetLookContraint : MonoBehaviour{

    private LookAtConstraint lookAtConstraint;
    
    void Start()
    {
        if (gameObject.GetComponent<LookAtConstraint>()){
            gameObject.GetComponent<LookAtConstraint>().enabled = true;
        }
    }
}
