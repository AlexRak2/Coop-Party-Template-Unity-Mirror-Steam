using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Markers/Marker")]
public class MarkerDefinition : ScriptableObject
{
    public byte id;

    [Space(10)]
    public GameObject markerLocalObj, markerWorldObj;
    public float lifeDuration;
    public bool unlimitedLife;
    public float minFadeAmount;
    public bool fadeByDotProduct;
    public bool fadeByAim;
}

