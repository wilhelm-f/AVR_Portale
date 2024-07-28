using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HTC.UnityPlugin.ColliderEvent;

// Implementierung eines einfachen Teleports
public class Teleport_3D_Butoon : Teleport
    , IColliderEventPressEnterHandler
{
    [Tooltip("Der Name der Zielszene")]
    public string DestinationScene = "";

    [Tooltip("Das Zielobkekt. Wenn zu anderer Szene teleportiert wird, dann leer")]
    public Transform TeleportDestination;

    private void OnEnable()
    {
        teleport_enable();

        scene = DestinationScene;
        destination = TeleportDestination;
    }

    public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    {
        StartTeleport();

        Deactivate_Pointers deactivate = transform.gameObject.GetComponentInChildren<Deactivate_Pointers>();
        if (deactivate != null)
        {
            deactivate.SetInactive(1.0f);
        }
    }
}
