using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Implementierung eines einfachen Portal Pads mit 3D button
public class Teleporter_Pad : MonoBehaviour
    , IColliderEventHoverEnterHandler
    , IColliderEventHoverExitHandler
    , IColliderEventPressEnterHandler
{
    [Tooltip("Das ViveCameraRig dieser Szene")]
    public Transform CameraRig;
    private Transform pivot;

    [Tooltip("Das Ziel Pad")]
    public Transform DestinationPad;
    private bool ready = false;

    private Renderer base_renderer;

    [Tooltip("Das zum Button zugehörige Portal Pad")]
    public Transform Pad;
    private Material PadExitMaterial;

    [Tooltip("Das Matrial bei Aktivierung des Portals")]
    public Material PadEnterMaterial;

    [Tooltip("Der Abstand zum Pad, bewegt sich die Camera näher an das Pad, wird es aktiviert")]
    public float DistanceToPad = 0.5f;

    // Wird vom jeweils verbundenen Portal gesetzt, damit beim Berühren eines Portals das jeweils andere auch [PadEnterMaterial] zugewiesen bekommt
    private bool active = true;

    private void OnEnable()
    {
        // Suchen der Kamera Komponente des CameraRig
        pivot = CameraRig.Find("Camera").transform;

        // Renderer aus Komponenten holen
        base_renderer = Pad.GetComponent<Renderer>();

        // Standard Material bestimmen
        PadExitMaterial = base_renderer.sharedMaterial;
    }

    private void Start()
    {
    }

    private void Update()
    {
        // Wenn active = False, dann ist dieses Portal inaktiv, da jemand auf dem verbundenen Protal steht. Dient dazu, dass dieses Portal nicht wieder zu seinem Standard-Material wechselt
        if (active)
        {
            if (Vector2.Distance(new Vector2(pivot.position.x, pivot.position.z), new Vector2(Pad.position.x, Pad.position.z)) < DistanceToPad)
            {
                // Wenn Portal betreten wird, dann werden die Funktionen des Zielportals deaktiviert
                DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().active = false;
                // Portal ist bereit zu teleportieren
                ready = true;
                // [PadEnterMaterial] wird für dieses Portal und Zielportal gesetzt
                base_renderer.sharedMaterial = PadEnterMaterial;
                DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().base_renderer.sharedMaterial = PadEnterMaterial;
            }
            else
            {
                // Sonst zu ausgangssitualtion zurückkehren
                DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().active = true;
                ready = false;
                base_renderer.sharedMaterial = PadExitMaterial;
                DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().base_renderer.sharedMaterial = DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().PadExitMaterial;
            }
        }
    }

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        // Da Button und teleportable Skript beide über Trigger taste Aktiviert werden, werden die ViveCurvePointers bei hover über den Button deaktiviert, da sonst Portal teleport und teleportable gleichzeitig ausgeführt werden
        CameraRig.Find("ViveCurvePointers").gameObject.SetActive(false);
        
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        // ViveCurvePointers werden wieder aktiviert, wenn die Corntoller nicht über dem Button hoveren
        CameraRig.Find("ViveCurvePointers").gameObject.SetActive(true);
    }

    public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    {
        if (ready)
        {
            // Wenn Button gedrückt wird und Person im Pad, dann teleportieren
            // Achtung: Checken von Rotation des CameraRig und ausgleichen Offset lokale Koordinaten Kamera
            Vector3 pad_pos = DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().Pad.position - (Quaternion.AngleAxis(CameraRig.eulerAngles.y, Vector3.up) * new Vector3(pivot.localPosition.x, 0, pivot.localPosition.z));
            CameraRig.position = new Vector3(pad_pos.x, pad_pos.y - DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().Pad.lossyScale.y*2, pad_pos.z);
        }
    }
}
