//========= Copyright 2016-2024, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

// This component shows the status that interacting with ColliderEventCaster
public class Change_Portal_Dest : MonoBehaviour
{
    public Transform PortalSource;

    public List<Transform> PortalDestinations = new List<Transform>();

    private int index = 0;

    private void OnEnable()
    {
        changePortal();
    }

    public void changeDestination(bool forward)
    {
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
        foreach (Transform portal in PortalDestinations)
        {
            portal.GetComponentInChildren<Teleporter_Portal>().DestinationPortal = null;
        }
        Material mat = PortalDestinations[index].GetComponentInChildren<Teleporter_Portal>().transform.GetComponent<Renderer>().sharedMaterial;
        PortalSource.GetComponentInChildren<Teleporter_Portal>().transform.GetComponent<Renderer>().sharedMaterial = mat;
        PortalSource.GetComponentInChildren<Teleporter_Portal>().DestinationPortal = PortalDestinations[index];
        PortalDestinations[index].GetComponentInChildren<Teleporter_Portal>().DestinationPortal = PortalSource;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}
