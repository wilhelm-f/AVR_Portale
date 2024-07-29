using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.ColliderEvent;

// Implementierung eines vertikalen Portals mit Preview
public class Teleporter_Portal_Preview : Portal_Vertical
{
    [Tooltip("Die Kamera, die für die Privew für die teleportation zu diesem Portal genutzt wird. ACHTUNG, nicht die Kamera, die für die Preview DIESES Portals genutzt wird")]
    public Camera PreviewCamera;

    public int TextureWidth = 256;
    public int TextureHeight = 413;

    protected RenderTexture preview;

    private void OnEnable()
    {
        teleport_enable();

        //Die "Normale" ist hier nicht die Normale der Portalfläche, sondern ein Vektor zwischen Portalfläche und Teleportpunkt, im Prefab entspricht das auch der Normale, wird jedoch der Ausgangspunkt geändert, ändert sich hier auch dieser Vektor. 
        normal = new Vector3(SourcePoint.position.x - transform.position.x, 0, SourcePoint.position.z - transform.position.z);
        normal.Normalize();

        setDestination(TeleportDestination);

        // Erstellen der Preview Textur
        preview = new RenderTexture(TextureWidth, TextureHeight, 16);
        TeleportDestination.GetComponentInChildren<Teleporter_Portal_Preview>().PreviewCamera.targetTexture = preview;
        transform.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", preview);
    }

    new public void OnTriggerEnter(Collider other)
    {
        if (TeleportDestination != null)
        {
            // Bei Kollison HMD und Portalfläche wird das CameraRig teleportiert, hinzu kommt, dass sich die Rotation des CameraRig ändert, sodass die Imersion erhalten bleibt, "durch eine Tür zu Gehen"
            if (other.gameObject.name == pivot.gameObject.name)
            {
                // Bei Kollison HMD und Portalfläche wird das CameraRig teleportiert, hinzu kommt, dass sich die Rotation des CameraRig ändert, sodass die Imersion erhalten bleibt, "durch eine Tür zu Gehen"
                SetRotation();

                StartTeleport(false);
            }
            // Teleportation Tennisball
            if (other.gameObject.name == "Tennis_Ball")
            {
                // Teleportieren
                Transform destination_point = TeleportDestination.gameObject.GetComponentInChildren<Teleporter_Portal_Preview>().SourcePoint;
                other.gameObject.transform.position = new Vector3(destination_point.position.x, destination_point.position.y + 1.0f, destination_point.position.z);

                // Geschwindigkeitsvektor anpassen
                Vector3 throw_target_normal = TeleportDestination.gameObject.GetComponentInChildren<Teleporter_Portal_Preview>().normal;
                float throw_current_velocity = other.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
                other.gameObject.GetComponent<Rigidbody>().velocity = throw_target_normal * throw_current_velocity;
            }
        }
    }
}
