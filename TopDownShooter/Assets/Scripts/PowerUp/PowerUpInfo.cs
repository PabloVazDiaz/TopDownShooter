using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName ="NewPowerUpInfo",menuName ="PowerUp")]
public class PowerUpInfo : ScriptableObject
{

    public float duration;
    public float newValue;
    
    
    public string AttributeToChange;
    
    public PowerableAttribute attributeToChange;



}
