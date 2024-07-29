using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Implementierung eines einfachen Teleports
public abstract class Teleport : MonoBehaviour
{
    [Tooltip("Das ViveCameraRig dieser Szene")]
    public Transform CameraRig;
    protected Transform pivot;

    protected string scene = "";

    protected Transform destination;

    protected void teleport_enable()
    {
        // Suchen der Kamera Komponente des CameraRig
        pivot = CameraRig.Find("Camera").transform;
    }

    protected void StartTeleport(bool deactivatePointers = true)
    {
        if (scene == "")
        {
            if (destination != null)
            {
                // teleportiern
                // Achtung: Checken von Rotation des CameraRig und ausgleichen Offset lokale Koordinaten Kamera
                CameraRig.Find("ViveCurvePointers").gameObject.SetActive(!deactivatePointers);
                CameraRig.position = destination.position - (Quaternion.AngleAxis(CameraRig.eulerAngles.y, Vector3.up) * new Vector3(pivot.localPosition.x, 0, pivot.localPosition.z));
            }
        }
        else
        {
            // neue Szene laden und teleportieren
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
