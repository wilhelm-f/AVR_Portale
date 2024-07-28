using HTC.UnityPlugin.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

// Implementierung eines einfachen Portal Pads mit Controller Button
public class Portal_Pad_Input_Button : Portal_Pad
{
    [Tooltip("Der Controller Button, mit dem das Portal aktiviert wird")]
    public ControllerButton button = ControllerButton.Trigger;

    private void OnEnable()
    {
        portal_enable();

        //Listener für den Button, wird dieser gedrückt wird das CameraRig teleportiert
        ViveInput.AddListenerEx(HandRole.RightHand,
                                button,
                                ButtonEventType.Down,
                                CheckTeleport);
    }

    private void CheckTeleport()
    {
        if (ready)
        {
            StartTeleport(false);
        }
    }
}
