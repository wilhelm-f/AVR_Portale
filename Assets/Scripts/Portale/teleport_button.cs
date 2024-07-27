using HTC.UnityPlugin.Utility;
using UnityEngine;
using HTC.UnityPlugin.Vive;

// Implementierung eines einfachen Portal Pads mit Controller Button
public class Teleport_Button : MonoBehaviour
{
    [Tooltip("Das ViveCameraRig dieser Szene")]
    public Transform CameraRig;
    private Transform pivot;

    [Tooltip("Das Ziel Pad")]
    public Transform DestinationPad;
    private bool ready = false;

    private Renderer base_renderer;

    private Material PadExitMaterial;

    [Tooltip("Das Matrial bei Aktivierung des Portals")]
    public Material PadEnterMaterial;

    [Tooltip("Der Abstand zum Pad, bewegt sich die Camera näher an das Pad, wird es aktiviert")]
    public float DistanceToPad = 0.5f;

    // Wird vom jeweils verbundenen Portal gesetzt, damit beim Berühren eines Portals das jeweils andere auch [PadEnterMaterial] zugewiesen bekommt
    private bool active = true;

    [Tooltip("Der Controller Button, mit dem das Portal aktiviert wird")]
    public ControllerButton button = ControllerButton.Trigger;

    private void OnEnable()
    {
        //Listener für den Button, wird dieser gedrückt wird das CameraRig teleportiert
        ViveInput.AddListenerEx(HandRole.RightHand,
                                button,
                                ButtonEventType.Down,
                                Teleport);

        // Suchen der Kamera Komponente des CameraRig
        pivot = CameraRig.Find("Camera").transform;

        // Renderer aus Komponenten holen
        base_renderer = transform.GetComponent<Renderer>();

        // Standard Material bestimmen
        PadExitMaterial = base_renderer.sharedMaterial;
    }

    private void Start()
    {
    }

    private void Update()
    {
        // Wenn active = False, dann ist dieses Portal inaktiv, da jemand auf dem verbundenen Protal steht. Dient dazu, dass dieses Portal nicht wieder zu seinem Standard-Material wechselt
        if (active)
        {
            if (Vector2.Distance(new Vector2(pivot.position.x, pivot.position.z), new Vector2(transform.position.x, transform.position.z)) < DistanceToPad)
            {
                // Wenn Portal betreten wird, dann werden die Funktionen des Zielportals deaktiviert
                DestinationPad.gameObject.GetComponent<Teleport_Button>().active = false;
                // Portal ist bereit zu teleportieren
                ready = true;

                // [PadEnterMaterial] wird für dieses Portal und Zielportal gesetzt
                base_renderer.sharedMaterial = PadEnterMaterial;
                DestinationPad.gameObject.GetComponentInChildren<Teleport_Button>().base_renderer.sharedMaterial = PadEnterMaterial;
            }
            else
            {
                // Sonst zu ausgangssitualtion zurückkehren
                DestinationPad.gameObject.GetComponent<Teleport_Button>().active = true;
                ready = false;
                base_renderer.sharedMaterial = PadExitMaterial;
                DestinationPad.gameObject.GetComponentInChildren<Teleport_Button>().base_renderer.sharedMaterial = DestinationPad.gameObject.GetComponentInChildren<Teleport_Button>().PadExitMaterial;
            }
        }
    }

    private void Teleport()
    {
        if (ready)
        {
            // Wenn Button gedrückt wird und Person im Pad, dann teleportieren
            // Achtung: Checken von Rotation des CameraRig und ausgleichen Offset lokale Koordinaten Kamera
            Vector3 pad_pos = DestinationPad.gameObject.GetComponentInChildren<Teleport_Button>().transform.position - (Quaternion.AngleAxis(CameraRig.eulerAngles.y, Vector3.up) * new Vector3(pivot.localPosition.x, 0, pivot.localPosition.z));

            //Checken von Rotation des CameraRig und ausgleichen Offset lokale Koordinaten Kamera
            CameraRig.position = new Vector3(pad_pos.x, pad_pos.y - DestinationPad.gameObject.GetComponentInChildren<Teleport_Button>().transform.lossyScale.y * 2, pad_pos.z);
        }
    }
}
