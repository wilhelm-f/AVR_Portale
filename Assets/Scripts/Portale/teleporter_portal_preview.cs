using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Implementierung eines vertikalen Portals mit Preview
public class Teleporter_Portal_Preview : MonoBehaviour
{
    [Tooltip("Das ViveCameraRig dieser Szene")]
    public Transform CameraRig;
    private Transform pivot;

    [Tooltip("Die Kamera, die für die Privew für die teleportation zu diesem Portal genutzt wird. ACHTUNG, nicht die Kamera, die für die Preview DIESES Portals genutzt wird")]
    public Camera PreviewCamera;

    [Tooltip("Der Punkt, an den ein CameraRig teleportiert wird, wenn man ZU diesem Protal teleportiert")]
    public Transform SourcePoint;

    [Tooltip("Das Zielportal")]
    public Transform DestinationPortal;
    private RenderTexture preview;
    private Vector3 normal;

    private void OnEnable()
    {
        // Suchen der Kamera Komponente des CameraRig
        pivot = CameraRig.Find("Camera").transform;

        // Erstellen der Preview Textur
        preview = new RenderTexture(256, 413, 16);
        DestinationPortal.GetComponentInChildren<Teleporter_Portal_Preview>().PreviewCamera.targetTexture = preview;
        transform.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", preview);

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
            // Bei Kollison HMD und Portalfläche wird das CameraRig teleportiert, hinzu kommt, dass sich die Rotation des CameraRig ändert, sodass die Imersion erhalten bleibt, "durch eine Tür zu Gehen"
            Transform destination_point = DestinationPortal.gameObject.GetComponentInChildren<Teleporter_Portal_Preview>().SourcePoint;
            if (other.gameObject.name == pivot.gameObject.name)
            {
                Vector3 target_dir = DestinationPortal.gameObject.GetComponentInChildren<Teleporter_Portal_Preview>().normal;
                Vector3 camera_dir = new Vector3(CameraRig.Find("Camera").transform.forward.x, 0, CameraRig.Find("Camera").transform.forward.z);
                float target_angle = Vector3.Angle(target_dir, camera_dir);
                Vector3 cross = Vector3.Cross(target_dir, camera_dir);
                if (cross.y > 0) target_angle = -target_angle;

                float camera_portal_angle = Vector3.Angle(normal * -1, camera_dir);
                cross = Vector3.Cross(normal * -1, camera_dir);
                if (cross.y < 0) camera_portal_angle = -camera_portal_angle;

                // Das CameraRig wird in Richtung der Normale des Zielportals gedreht, diese Drehung wird angepasst, je nachdem wie der Nutzer bei der Teleportation zum Eingangsprotal stand
                CameraRig.Rotate(0, target_angle+camera_portal_angle, 0);
                CameraRig.position = destination_point.position - (Quaternion.AngleAxis(CameraRig.eulerAngles.y, Vector3.up) * new Vector3(pivot.localPosition.x, 0, pivot.localPosition.z));
            }
            // Teleportation Tennisball
            if (other.gameObject.name == "Tennis_Ball")
            {
                other.gameObject.transform.position = new Vector3(destination_point.position.x, destination_point.position.y + 1.0f, destination_point.position.z);
                Vector3 throw_target_normal = DestinationPortal.gameObject.GetComponentInChildren<Teleporter_Portal_Preview>().normal;
                float throw_current_velocity = other.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
                other.gameObject.GetComponent<Rigidbody>().velocity = throw_target_normal * throw_current_velocity;
            }
        }
    }
}
