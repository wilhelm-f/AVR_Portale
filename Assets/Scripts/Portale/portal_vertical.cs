using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implementierung eines vertikalen Portals
public class Portal_Vertical : Teleport
{
    [Tooltip("Der Punkt, an den ein CameraRig teleportiert wird, wenn man ZU diesem Protal teleportiert")]
    public Transform SourcePoint;

    [Tooltip("Das Zielportal")]
    public Transform TeleportDestination;
    protected Vector3 normal;

    private void OnEnable()
    {
        teleport_enable();

        //Die "Normale" ist hier nicht die Normale der Portalfläche, sondern ein Vektor zwischen Portalfläche und Teleportpunkt, im Prefab entspricht das auch der Normale, wird jedoch der Ausgangspunkt geändert, ändert sich hier auch dieser Vektor. 
        normal = new Vector3(SourcePoint.position.x - transform.position.x, 0, SourcePoint.position.z - transform.position.z);
        normal.Normalize();

        setDestination(TeleportDestination);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (TeleportDestination != null)
        {
            if (other.gameObject.name == pivot.gameObject.name)
            {
                // Bei Kollison HMD und Portalfläche wird das CameraRig teleportiert, hinzu kommt, dass sich die Rotation des CameraRig ändert, sodass die Imersion erhalten bleibt, "durch eine Tür zu Gehen"
                SetRotation();

                StartTeleport(false);
            }
        }
    }

    protected void SetRotation()
    {
        // Das CameraRig wird in Richtung der Normale des Zielportals gedreht, diese Drehung wird angepasst, je nachdem wie der Nutzer bei der Teleportation zum Eingangsprotal stand
        Vector3 target_dir = TeleportDestination.gameObject.GetComponentInChildren<Portal_Vertical>().normal;
        Vector3 camera_dir = new Vector3(pivot.forward.x, 0, pivot.forward.z);

        float target_rotation = Vector3.Angle(target_dir, camera_dir);
        Vector3 cross = Vector3.Cross(target_dir, camera_dir);
        if (cross.y > 0) target_rotation = -target_rotation;

        float camera_portal_angle = Vector3.Angle(normal * -1, camera_dir);
        cross = Vector3.Cross(normal * -1, camera_dir);
        if (cross.y < 0) camera_portal_angle = -camera_portal_angle;

        CameraRig.Rotate(0, target_rotation + camera_portal_angle, 0);
    }

    public void setDestination(Transform portal)
    {
        if (portal != null)
        {
            destination = portal.gameObject.GetComponentInChildren<Portal_Vertical>().SourcePoint;
            TeleportDestination = portal;
        } else
        {
            destination = null;
            TeleportDestination = null;
        }
    }
}
