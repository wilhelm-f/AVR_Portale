using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

// Implementierung eines einfachen Teleports
public class Teleport : MonoBehaviour
    , IColliderEventHoverEnterHandler
    , IColliderEventHoverExitHandler
    , IColliderEventPressEnterHandler
{
    [Tooltip("Das ViveCameraRig dieser Szene")]
    public Transform CameraRig;
    private Transform pivot;

    [Tooltip("Der Name der Zielszene")]
    public string DestinationScene = "";

    [Tooltip("Das Zielobkekt, wenn zu anderer Szene teleportiert wird, dann leer")]
    public Transform TeleportDestination;

    [Tooltip("Nach der teleportation können die ViveCurvePointers für eine bestimmte Zeit deaktiviert werden, um überschneidungen zu vermeiden")]
    public float DeactivatePointers = 1.0f;
    private bool recently_teleported = false;

    private void OnEnable()
    {
        // Suchen der Kamera Komponente des CameraRig
        pivot = CameraRig.Find("Camera").transform;
    }

    private void Start()
    {
    }

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

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        // Da Button und teleportable Skript beide über Trigger taste Aktiviert werden, werden die ViveCurvePointers bei hover über den Button deaktiviert, da sonst Portal teleport und teleportable gleichzeitig ausgeführt werden
        CameraRig.Find("ViveCurvePointers").gameObject.SetActive(false);
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        // ViveCurvePointers werden wieder aktiviert, wenn die Corntoller nicht über dem Button hoveren, außer sie sollen deaktiviert bleiben
        if (!recently_teleported)
        {
            CameraRig.Find("ViveCurvePointers").gameObject.SetActive(true);
        }
    }

    public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    {
        if (DestinationScene == "")
        {
            if (TeleportDestination != null)
            {
                // teleportiern
                // Achtung: Checken von Rotation des CameraRig und ausgleichen Offset lokale Koordinaten Kamera
                CameraRig.Find("ViveCurvePointers").gameObject.SetActive(false);
                recently_teleported = true;
                CameraRig.position = TeleportDestination.position - (Quaternion.AngleAxis(CameraRig.eulerAngles.y, Vector3.up) * new Vector3(pivot.localPosition.x, 0, pivot.localPosition.z));
                DeactivatePointers = 1.0f;
            }
        }
        else
        {
            // neue Szene laden und teleportieren
            SceneManager.LoadScene(DestinationScene, LoadSceneMode.Single);
        }
    }
}
