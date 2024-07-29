using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implementierung eines Skripts, mit dem man das Ziel von Portalen mit der Komponente Teleporter_Portal ändern kann
public class Change_Portal_Dest : MonoBehaviour
{
    [Tooltip("Das Eingangsportal")]
    public Transform PortalSource;

    [Tooltip("Eine Liste von Ausgangsprotalen, diese müssen eine Teleporter_Portal Komponente besitzen")]
    public List<Transform> PortalDestinations = new List<Transform>();

    private int index = 0;

    private void OnEnable()
    {
        changePortal();
    }

    public void changeDestination(bool forward)
    {
        // Index des Zielportals ändert sich
        if (forward)
        {
            index++;
            if (index == PortalDestinations.Count)
            {
                index = 0;
            }
        } else
        {
            index--;
            if (index == -1)
            {
                index = PortalDestinations.Count - 1;
            }
        }
        changePortal();
    }

    private void changePortal()
    {
        // Die Attribute der verbundenen Portale werden angepasst, das Material der Portalfläche des Eingangsportals ändert sich zu dem Material der Portalfläche des neuen Ausgangsportals
        // Bei allen Portalen Ziel Null (deaktiviert)
        foreach (Transform portal in PortalDestinations)
        {
            portal.GetComponentInChildren<Portal_Vertical>().setDestination(null);
        }

        // Material von Eingang und Ausgang
        Material mat = PortalDestinations[index].GetComponentInChildren<Portal_Vertical>().transform.GetComponent<Renderer>().sharedMaterial;
        PortalSource.GetComponentInChildren<Portal_Vertical>().transform.GetComponent<Renderer>().sharedMaterial = mat;

        // Ziel von Eingang und Ausgang setzen
        PortalSource.GetComponentInChildren<Portal_Vertical>().setDestination(PortalDestinations[index]);
        PortalDestinations[index].GetComponentInChildren<Portal_Vertical>().setDestination(PortalSource);
    }
}
