//========= Copyright 2016-2024, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using HTC.UnityPlugin.Vive;

// This component shows the status that interacting with ColliderEventCaster
public class Teleporter_Preview_Rotation : MonoBehaviour
{
    public Transform CameraRig;
    public Camera PreviewCamera;
    public Transform DestinationPad;
    public Transform Pointer;
    private Transform pivot;
    private RenderTexture preview;
    private float DistanceToPortal = 1.5f;
    private Vector3 initial_dir_of_pointer;
    private Quaternion initial_rot_of_pointer;

    private void OnEnable()
    {
        pivot = CameraRig.Find("Camera").transform;
        preview = new RenderTexture(413, 256, 16);
        PreviewCamera.targetTexture = preview;
        transform.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", preview);
        initial_dir_of_pointer = new Vector3(Pointer.position.x - DestinationPad.position.x, 0, Pointer.position.z - DestinationPad.position.z);
        initial_dir_of_pointer.Normalize();
        initial_rot_of_pointer = Pointer.rotation;
}

    private void Start()
    {

    }

    private void Update()
    {
        if (Vector2.Distance(new Vector2(pivot.position.x, pivot.position.z), new Vector2(transform.position.x, transform.position.z)) < DistanceToPortal)
        {
            Vector3 pointer_dir = new Vector3(ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.JoystickX), 0, ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.JoystickY));

            pointer_dir = Quaternion.Euler(0, -90, 0) * pointer_dir;
            if (pointer_dir.magnitude > 0.8f)
            {
                pointer_dir.Normalize();
                Pointer.position = DestinationPad.position + pointer_dir * 0.7f;

                float target_rotation = Vector3.Angle(initial_dir_of_pointer, pointer_dir);
                Vector3 cross = Vector3.Cross(initial_dir_of_pointer, pointer_dir);
                if (cross.y > 0) target_rotation = -target_rotation;

                Quaternion modifiedRotation = initial_rot_of_pointer;
                modifiedRotation.eulerAngles = new Vector3(modifiedRotation.eulerAngles.x, modifiedRotation.eulerAngles.y - target_rotation, modifiedRotation.eulerAngles.z);
                Pointer.rotation = modifiedRotation;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (DestinationPad != null)
        {
            Vector3 target_dir = new Vector3(Pointer.position.x - DestinationPad.position.x, 0, Pointer.position.z - DestinationPad.position.z);
            Vector3 camera_dir = new Vector3(CameraRig.Find("Camera").transform.forward.x, 0, CameraRig.Find("Camera").transform.forward.z);
            float target_angle = Vector3.Angle(target_dir, camera_dir);
            Vector3 cross = Vector3.Cross(target_dir, camera_dir);
            if (cross.y > 0) target_angle = -target_angle;
            CameraRig.Rotate(0, target_angle, 0);

            Vector3 pad_pos = DestinationPad.position - (Quaternion.AngleAxis(CameraRig.eulerAngles.y, Vector3.up) * new Vector3(pivot.localPosition.x, 0, pivot.localPosition.z));
            CameraRig.position = new Vector3(pad_pos.x, pad_pos.y - DestinationPad.lossyScale.y * 2, pad_pos.z);
        }
    }
}
