using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

// Dieses Skript dient dazu, die VivePointer beim hovern über Buttons zu deaktivieren und sagt dem Skript "change_portal_dest.cs", ob der Index in der Portal-Liste inkrementiert oder dekrementiert wird
// Es gibt immer einen Vorwärts- und einen Rückwärts-Button
public class Change_Portal_Dest_Button : MonoBehaviour
    , IColliderEventHoverEnterHandler
    , IColliderEventHoverExitHandler
    , IColliderEventPressEnterHandler
{
    [Tooltip("Das ViveCameraRig dieser Szene")]
    public Transform CameraRig;
    [Tooltip("Richtung: nächstes Protal oder vorhergehendes Protal")]
    public bool Forward = true;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        // Da Button und teleportable Skript beide über Trigger taste Aktiviert werden, werden die ViveCurvePointers bei hover über den Button deaktiviert, da sonst Portal teleport und teleportable gleichzeitig ausgeführt werden
        CameraRig.Find("ViveCurvePointers").gameObject.SetActive(false);
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        // ViveCurvePointers werden wieder aktiviert, wenn die Corntoller nicht über dem Button hoveren, außer sie sollen deaktiviert bleiben
        CameraRig.Find("ViveCurvePointers").gameObject.SetActive(true);
    }

    public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    {
        // Index in der Portal-Liste von "change_portal_dest.cs" inkrementieren oder dekrementieren
        transform.GetComponentInParent<Change_Portal_Dest>().changeDestination(Forward);
    }
}
