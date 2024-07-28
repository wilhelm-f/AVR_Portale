using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.ColliderEvent;

// Falls wie in meinem Fall der Trigger für das Drücken eines 3D-Buttons in der Szene und das teleportable Script verwendet wird, muss die teleportable Funktion kurzzeitig deaktiviert werden
public class Deactivate_Pointers : MonoBehaviour
    , IColliderEventHoverEnterHandler
    , IColliderEventHoverExitHandler
{
    [Tooltip("Das ViveCameraRig dieser Szene")]
    public Transform CameraRig;

    private float DeactivatePointers = 0.0f;
    private bool recently_teleported = false;

    private void Update()
    {
        // ViveCurvePointers nach bestimmter Zeit wieder aktivieren
        DeactivatePointers -= Time.deltaTime;
        if (recently_teleported && DeactivatePointers <= 0.0f)
        {
            CameraRig.Find("ViveCurvePointers").gameObject.SetActive(true);
            recently_teleported = false;
        }
    }

    //Bei und nach der Teleportation können die ViveCurvePointers für eine bestimmte Zeit (in Sekunden) deaktiviert werden, um eine nachfolgende Teleportation durch das teleportable-Script zu vermeiden (Fall Doppelbelegung Trigger)
    public void SetInactive(float time)
    {
        recently_teleported = true;
        DeactivatePointers = time;
    }

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        // Da 3D Buttons und teleportable Skript beide über Trigger Taste aktiviert werden, werden die ViveCurvePointers bei hover über den Button deaktiviert, da sonst Portal Teleport und teleportable Skript beide ausgeführt werden
        CameraRig.Find("ViveCurvePointers").gameObject.SetActive(false);
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        // ViveCurvePointers werden wieder aktiviert, wenn die Corntoller nicht über dem Button hoveren, außer sie sollen deaktiviert bleiben (kurz nach teleportation)
        if (!recently_teleported)
        {
            CameraRig.Find("ViveCurvePointers").gameObject.SetActive(true);
        }
    }
}
