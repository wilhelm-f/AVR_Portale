//========= Copyright 2016-2024, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

// This component shows the status that interacting with ColliderEventCaster
public class Teleport : MonoBehaviour
    , IColliderEventHoverEnterHandler
    , IColliderEventHoverExitHandler
    , IColliderEventPressEnterHandler
{
    public Transform CameraRig;
    private Transform pivot;
    public string DestinationScene = "";
    public Transform TeleportDestination;

    public float DeactivatePointers = 1.0f;
    private bool recently_teleported = false;

    private void OnEnable()
    {
        pivot = CameraRig.Find("Camera").transform;
    }

    private void Start()
    {
    }

    private void Update()
    {
        DeactivatePointers -= Time.deltaTime;
        if (recently_teleported && DeactivatePointers <= 0.0f)
        {
            CameraRig.Find("ViveCurvePointers").gameObject.SetActive(true);
            recently_teleported = false;
        }
    }

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        CameraRig.Find("ViveCurvePointers").gameObject.SetActive(false);
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        if (!recently_teleported)
        {
            CameraRig.Find("ViveCurvePointers").gameObject.SetActive(true);
        }
    }

    public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    {
        if (DestinationScene == "")
        {
            if (TeleportDestination != null)
            {
                CameraRig.Find("ViveCurvePointers").gameObject.SetActive(false);
                recently_teleported = true;
                CameraRig.position = TeleportDestination.position - (Quaternion.AngleAxis(CameraRig.eulerAngles.y, Vector3.up) * new Vector3(pivot.localPosition.x, 0, pivot.localPosition.z));
                DeactivatePointers = 1.0f;
            }
        }
        else
        {
            SceneManager.LoadScene(DestinationScene, LoadSceneMode.Single);
        }
    }
}
