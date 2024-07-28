using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.ColliderEvent;

// Implementierung eines einfachen Portal Pads mit 3D button
public class Portal_Pad_3D_Button : Portal_Pad
    , IColliderEventPressEnterHandler
{
    private void OnEnable()
    {
        portal_enable();
    }

    public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    {
        if (ready)
        {
            StartTeleport();

            Deactivate_Pointers deactivate = transform.gameObject.GetComponentInChildren<Deactivate_Pointers>();
            if (deactivate != null)
            {
                deactivate.SetInactive(1.0f);
            }
        }
    }
}
