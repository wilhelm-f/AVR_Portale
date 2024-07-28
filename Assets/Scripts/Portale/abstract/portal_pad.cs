using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstrakte Umsetung eines Portal-Pads, das sich mit einem Ziel Pad verbinden kann
public abstract class Portal_Pad : Teleport
{
    [Tooltip("Das Ziel Pad")]
    public Transform DestinationPad;

    protected bool ready = false;
    private Renderer base_renderer;

    [Tooltip("Die zum Pad zugehörige Eingangsposition")]
    public Transform SourcePoint;

    private Material PadExitMaterial;

    [Tooltip("Das Matrial bei Aktivierung des Portals")]
    public Material PadEnterMaterial;

    [Tooltip("Der Abstand zum Pad, bewegt sich die Camera näher an das Pad, wird es aktiviert")]
    public float DistanceToPad = 0.5f;

    // Wird vom jeweils verbundenen Portal gesetzt, damit beim Berühren eines Portals das jeweils andere auch [PadEnterMaterial] zugewiesen bekommt
    private bool active = true;

    protected void portal_enable()
    {
        teleport_enable();

        // Renderer aus Komponenten holen
        base_renderer = SourcePoint.GetComponentInChildren<Renderer>();

        // Standard Material bestimmen
        PadExitMaterial = base_renderer.sharedMaterial;

        destination = DestinationPad.gameObject.GetComponentInChildren<Portal_Pad>().SourcePoint;
    }

    private void Update()
    {
        // Wenn active = False, dann ist dieses Portal inaktiv, da jemand auf dem verbundenen Protal steht. Dient dazu, dass dieses Portal nicht wieder zu seinem Standard-Material wechselt
        if (active)
        {
            if (Vector2.Distance(new Vector2(pivot.position.x, pivot.position.z), new Vector2(SourcePoint.position.x, SourcePoint.position.z)) < DistanceToPad)
            {
                // Wenn Portal betreten wird, dann werden die Funktionen des Zielportals deaktiviert
                DestinationPad.gameObject.GetComponentInChildren<Portal_Pad>().active = false;
                // Portal ist bereit zu teleportieren
                ready = true;
                // [PadEnterMaterial] wird für dieses Portal und Zielportal gesetzt
                base_renderer.sharedMaterial = PadEnterMaterial;
                DestinationPad.gameObject.GetComponentInChildren<Portal_Pad>().base_renderer.sharedMaterial = PadEnterMaterial;
            }
            else
            {
                // Sonst zu ausgangssitualtion zurückkehren
                DestinationPad.gameObject.GetComponentInChildren<Portal_Pad>().active = true;
                ready = false;
                base_renderer.sharedMaterial = PadExitMaterial;
                DestinationPad.gameObject.GetComponentInChildren<Portal_Pad>().base_renderer.sharedMaterial = DestinationPad.gameObject.GetComponentInChildren<Portal_Pad>().PadExitMaterial;
            }
        }
    }
}
