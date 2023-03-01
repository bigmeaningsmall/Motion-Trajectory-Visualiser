using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Enums;

public class ButtonEvent : MonoBehaviour
{
    public UnityEvent onClick;

    public void OnClick()
    {
        print("click");
        if (onClick != null)
        {
            onClick.Invoke();
        }
    }
}
