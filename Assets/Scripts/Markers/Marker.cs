using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public MarkerDefinition markerDefinition;
/*    public CanvasGroup fadeGroup;
    public CanvasGroup aimFadeGroup;
    public AnimationCurve fadeRemap;*/

    public Vector3 targetPos { get; private set; }
    public GameObject worldObject { get; private set; }

    MarkerHandler markerHandler;
    Transform objToFollow;

    float timeAlive;

    private void Update()
    {
        if (objToFollow)
        {
            targetPos = objToFollow.transform.position;
        }


        if (markerDefinition.unlimitedLife) return;

        timeAlive += Time.deltaTime;
        if (timeAlive > markerDefinition.lifeDuration)
        {
            DestroyMarker();
        }
    }

    public virtual void InitializeMarker(MarkerHandler markerHandler, GameObject worldObject, Vector3 targetPos, Transform objToFollow)
    {
        this.markerHandler = markerHandler;
        this.worldObject = worldObject;
        this.targetPos = targetPos;
        this.objToFollow = objToFollow;
/*        aimFadeGroup.alpha = 0;
        fadeGroup.alpha = 0;*/
    }

    public virtual void DestroyMarker()
    {
        markerHandler.RemoveMarker(this);
    }
}
