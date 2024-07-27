using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Implementierung eines vertikalen Portals
public class Teleporter_Portal : MonoBehaviour
{
    [Tooltip("Das ViveCameraRig dieser Szene")]
    public Transform CameraRig;
    private Transform pivot;

    [Tooltip("Der Punkt, an den ein CameraRig teleportiert wird, wenn man ZU diesem Protal teleportiert")]
    public Transform SourcePoint;

    [Tooltip("Das Zielportal")]
    public Transform DestinationPortal;
    private Vector3 normal;

    private void OnEnable()
    {
        // Suchen der Kamera Komponente des CameraRig
        pivot = CameraRig.Find("Camera").transform;

        //Die "Normale" ist hier nicht die Normale der Portalfläche, sondern ein Vektor zwischen Portalfläche und Teleportpunkt, im Prefab entspricht das auch der Normale, wird jedoch der Ausgangspunkt geändert, ändert sich hier auch dieser Vektor. 
        normal = new Vector3(SourcePoint.position.x - transform.position.x, 0, SourcePoint.position.z - transform.position.z);
        normal.Normalize();
    }

    private void Start()
    {
    }

    private void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (DestinationPortal != null)
        {
            if (other.gameObject.name == pivot.gameObject.name)
            {
                // Bei Kollison HMD und Portalfläche wird das CameraRig teleportiert, hinzu kommt, dass sich die Rotation des CameraRig ändert, sodass die Imersion erhalten bleibt, "durch eine Tür zu Gehen"
                Transform destination_point = DestinationPortal.gameObject.GetComponentInChildren<Teleporter_Portal>().SourcePoint;
                Vector3 target_dir = DestinationPortal.gameObject.GetComponentInChildren<Teleporter_Portal>().normal;
                Vector3 camera_dir = new Vector3(CameraRig.Find("Camera").transform.forward.x, 0, CameraRig.Find("Camera").transform.forward.z);
                float target_rotation = Vector3.Angle(target_dir, camera_dir);
                Vector3 cross = Vector3.Cross(target_dir, camera_dir);
                if (cross.y > 0) target_rotation = -target_rotation;

                float camera_portal_angle = Vector3.Angle(normal * -1, camera_dir);
                cross = Vector3.Cross(normal * -1, camera_dir);
                if (cross.y < 0) camera_portal_angle = -camera_portal_angle;

                // Das CameraRig wird in Richtung der Normale des Zielportals gedreht, diese Drehung wird angepasst, je nachdem wie der Nutzer bei der Teleportation zum Eingangsprotal stand
                CameraRig.Rotate(0, target_rotation + camera_portal_angle, 0);
                CameraRig.position = destination_point.position - (Quaternion.AngleAxis(CameraRig.eulerAngles.y, Vector3.up) * new Vector3(pivot.localPosition.x, 0, pivot.localPosition.z));
            }
        }
    }
}
