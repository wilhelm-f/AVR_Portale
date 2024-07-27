//========= Copyright 2016-2024, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// This component shows the status that interacting with ColliderEventCaster
public class Teleporter_Pad : MonoBehaviour
    , IColliderEventHoverEnterHandler
    , IColliderEventHoverExitHandler
    , IColliderEventPressEnterHandler
{
    public Transform CameraRig;
    private Transform pivot;
    public Transform DestinationPad;
    private bool ready = false;

    private Renderer base_renderer;

    public Transform Pad;
    private Material PadExitMaterial;
    public Material PadEnterMaterial;
    public float DistanceToPad = 0.5f;

    private bool active = true;

    private void OnEnable()
    {
        pivot = CameraRig.Find("Camera").transform;
        base_renderer = Pad.GetComponent<Renderer>();
        PadExitMaterial = base_renderer.sharedMaterial;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (active)
        {
            if (Vector2.Distance(new Vector2(pivot.position.x, pivot.position.z), new Vector2(Pad.position.x, Pad.position.z)) < DistanceToPad)
            {
                DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().active = false;
                ready = true;
                base_renderer.sharedMaterial = PadEnterMaterial;
                DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().base_renderer.sharedMaterial = PadEnterMaterial;
            }
            else
            {
                DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().active = true;
                ready = false;
                base_renderer.sharedMaterial = PadExitMaterial;
                DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().base_renderer.sharedMaterial = DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().PadExitMaterial;
            }
        }
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
        if (ready)
        {
            Vector3 pad_pos = DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().Pad.position - (Quaternion.AngleAxis(CameraRig.eulerAngles.y, Vector3.up) * new Vector3(pivot.localPosition.x, 0, pivot.localPosition.z));
            CameraRig.position = new Vector3(pad_pos.x, pad_pos.y - DestinationPad.gameObject.GetComponentInChildren<Teleporter_Pad>().Pad.lossyScale.y*2, pad_pos.z);
        }
    }
}
