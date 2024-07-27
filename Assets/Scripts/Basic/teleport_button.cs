//========= Copyright 2016-2024, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.Utility;
using UnityEngine;
using HTC.UnityPlugin.Vive;

// This component shows the status that interacting with ColliderEventCaster
public class Teleport_Button : MonoBehaviour
{
    public Transform CameraRig;
    private Transform pivot;
    public Transform DestinationPad;
    private bool ready = false;

    private Renderer base_renderer;

    private Material PadExitMaterial;
    public Material PadEnterMaterial;
    public float DistanceToPad = 0.5f;

    private bool active = true;

    public ControllerButton button = ControllerButton.Trigger;

    private void OnEnable()
    {
        ViveInput.AddListenerEx(HandRole.RightHand,
                                button,
                                ButtonEventType.Down,
                                Teleport);

        pivot = CameraRig.Find("Camera").transform;
        base_renderer = transform.GetComponent<Renderer>();
        PadExitMaterial = base_renderer.sharedMaterial;
    }

    private void Teleport()
    {
        if (ready)
        {
            Vector3 pad_pos = DestinationPad.gameObject.GetComponentInChildren<Teleport_Button>().transform.position - (Quaternion.AngleAxis(CameraRig.eulerAngles.y, Vector3.up) * new Vector3(pivot.localPosition.x, 0, pivot.localPosition.z));
            CameraRig.position = new Vector3(pad_pos.x, pad_pos.y - DestinationPad.gameObject.GetComponentInChildren<Teleport_Button>().transform.lossyScale.y * 2, pad_pos.z);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (active)
        {
            if (Vector2.Distance(new Vector2(pivot.position.x, pivot.position.z), new Vector2(transform.position.x, transform.position.z)) < DistanceToPad)
            {
                DestinationPad.gameObject.GetComponent<Teleport_Button>().active = false;
                ready = true;
                base_renderer.sharedMaterial = PadEnterMaterial;
                DestinationPad.gameObject.GetComponentInChildren<Teleport_Button>().base_renderer.sharedMaterial = PadEnterMaterial;
            }
            else
            {
                DestinationPad.gameObject.GetComponent<Teleport_Button>().active = true;
                ready = false;
                base_renderer.sharedMaterial = PadExitMaterial;
                DestinationPad.gameObject.GetComponentInChildren<Teleport_Button>().base_renderer.sharedMaterial = DestinationPad.gameObject.GetComponentInChildren<Teleport_Button>().PadExitMaterial;
            }
        }
    }
}
