//========= Copyright 2016-2024, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

// This component shows the status that interacting with ColliderEventCaster
public class Change_Portal_Dest_Button : MonoBehaviour
    , IColliderEventHoverEnterHandler
    , IColliderEventHoverExitHandler
    , IColliderEventPressEnterHandler
{
    public Transform CameraRig;
    public bool Forward = true;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        CameraRig.Find("ViveCurvePointers").gameObject.SetActive(false);
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        CameraRig.Find("ViveCurvePointers").gameObject.SetActive(true);
    }

    public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    {
        transform.GetComponentInParent<Change_Portal_Dest>().changeDestination(Forward);
    }
}
