using System;
using UnityEngine;

public enum ButtonType
{
    green,
    red,
    gray
}
public class PhoneButton : MonoBehaviour
{
    public ButtonType buttonType;

    private void Awake()
    {
        buttonType = ButtonType.gray;
    }
}
