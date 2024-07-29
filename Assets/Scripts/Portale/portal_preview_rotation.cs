using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Vive;

// Implementierung eines vertikalen Portals mit Preview bei dem man die Ausgangsblickrichtung ändern kann
public class Teleporter_Preview_Rotation : Teleporter_Portal_Preview
{
    [Tooltip("Das Objekt, das als Indikator für die Richtungsanzeige genutzt wird")]
    public Transform Pointer;

    [Tooltip("Die Distanz zum Portal, ab der man durch den Stick die Ausgangsblcikrichtung bestimmen kann")]
    public float DistanceToPortal = 1.5f;

    // Ursprüngliche Rotation und Position merken, um später einfacher zu drehen
    private Vector3 initial_dir_of_pointer;
    private Quaternion initial_rot_of_pointer;

    private void OnEnable()
    {
        teleport_enable();

        destination = TeleportDestination;

        // Erstellen der Preview Textur
        preview = new RenderTexture(TextureWidth, TextureHeight, 16);
        PreviewCamera.targetTexture = preview;
        transform.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", preview);

        // Attribute des Richtungspointers bei der Initialisierung
        initial_dir_of_pointer = new Vector3(Pointer.position.x - TeleportDestination.position.x, 0, Pointer.position.z - TeleportDestination.position.z);
        initial_dir_of_pointer.Normalize();
        initial_rot_of_pointer = Pointer.rotation;
}

    private void Update()
    {
        // Befindet sich die HMD in der nähe des Portals?
        if (Vector2.Distance(new Vector2(pivot.position.x, pivot.position.z), new Vector2(transform.position.x, transform.position.z)) < DistanceToPortal)
        {
            // Hier wird die Position und Rotation des Pointers geändert, sobald der Stick des rechten Controllers bewegt wird
            Vector3 pointer_dir = new Vector3(ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.JoystickX), 0, ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.JoystickY));

            pointer_dir = Quaternion.Euler(0, -90, 0) * pointer_dir;

            // der Pointer ändert sich nur, wenn der Stick weit genug vom Zentrum (Ruheposition) entfernt ist
            if (pointer_dir.magnitude > 0.8f)
            {
                pointer_dir.Normalize();
                // Pointer an die richtige Stelle bewegen
                Pointer.position = TeleportDestination.position + pointer_dir * 0.7f;

                float target_rotation = Vector3.Angle(initial_dir_of_pointer, pointer_dir);
                Vector3 cross = Vector3.Cross(initial_dir_of_pointer, pointer_dir);
                if (cross.y > 0) target_rotation = -target_rotation;

                Quaternion modifiedRotation = initial_rot_of_pointer;
                modifiedRotation.eulerAngles = new Vector3(modifiedRotation.eulerAngles.x, modifiedRotation.eulerAngles.y - target_rotation, modifiedRotation.eulerAngles.z);

                // Pointer Rotieren
                Pointer.rotation = modifiedRotation;
            }
        }
    }

    new public void OnTriggerEnter(Collider other)
    {
        if (TeleportDestination != null)
        {
            // Bei Kollison HMD und Portalfläche wird das CameraRig teleportiert, hinzu kommt, dass sich die Rotation des CameraRig ändert, sodass die Imersion erhalten bleibt, "durch eine Tür zu Gehen"
            Vector3 target_dir = new Vector3(Pointer.position.x - TeleportDestination.position.x, 0, Pointer.position.z - TeleportDestination.position.z);
            Vector3 camera_dir = new Vector3(CameraRig.Find("Camera").transform.forward.x, 0, CameraRig.Find("Camera").transform.forward.z);
            float target_angle = Vector3.Angle(target_dir, camera_dir);
            Vector3 cross = Vector3.Cross(target_dir, camera_dir);
            if (cross.y > 0) target_angle = -target_angle;

            // Das CameraRig wird in Richtung der durch den Pointer bestimmten Richtung teleportiert
            CameraRig.Rotate(0, target_angle, 0);

            StartTeleport(false);
        }
    }
}
