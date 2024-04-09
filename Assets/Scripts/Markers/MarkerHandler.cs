using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerHandler : MonoBehaviour
{
    public static MarkerHandler instance;

    [SerializeField] private MarkerDefinition[] allMarkerDefinitions;
    [SerializeField] private Transform markerContainer;

    List<Marker> markerInstances = new List<Marker>();
    Camera cam;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        UpdateMarkers();
    }
    void UpdateMarkers()
    {

        for (int i = 0; i < markerInstances.Count; i++)
        {
            var currentMarker = markerInstances[i];
            currentMarker.transform.position = cam.WorldToScreenPoint(currentMarker.targetPos);
            // aimFadeAmount = 1 - player.weaponManager.aim.aimAmount;

            bool isBehindCam = currentMarker.transform.position.z < 0;
            float dot = Vector3.Dot(cam.transform.forward, (cam.transform.position - currentMarker.targetPos).normalized);
            //Debug.Log(dot);

            /*
                        if (currentMarker.markerDefinition.fadeByAim)
                            currentMarker.aimFadeGroup.alpha = isBehindCam ? 0 : Mathf.Max(aimFadeAmount, currentMarker.markerDefinition.minFadeAmount);

                        if (currentMarker.markerDefinition.fadeByDotProduct)
                            currentMarker.fadeGroup.alpha = isBehindCam ? 0 : Mathf.Clamp01(Mathf.Clamp(currentMarker.fadeRemap.Evaluate(Mathf.Clamp01(-dot)), currentMarker.markerDefinition.minFadeAmount, 1));

                        currentMarker.gameObject.SetActive(!player.IsUIOpen());*/
        }

    }

    public Marker SpawnMarker(byte markerID, Vector3 targetPos, Transform objToFollow)
    {
        MarkerDefinition markerDef = allMarkerDefinitions[markerID];

        GameObject localObj = Instantiate(markerDef.markerLocalObj, markerContainer);
        GameObject worldObj = null;
        Marker marker = localObj.GetComponent<Marker>();

        if (markerDef.markerWorldObj)
        {
            worldObj = Instantiate(markerDef.markerWorldObj, targetPos, Quaternion.identity);
        }

        marker.InitializeMarker(this, worldObj, targetPos, objToFollow);

        AddMarker(marker);

        return marker;
    }

    public void AddMarker(Marker marker)
    {
        markerInstances.Add(marker);
    }

    public void RemoveMarker(Marker marker)
    {
        markerInstances.Remove(marker);

        if (marker.worldObject)
            Destroy(marker.worldObject);

        Destroy(marker.gameObject);
    }

}
