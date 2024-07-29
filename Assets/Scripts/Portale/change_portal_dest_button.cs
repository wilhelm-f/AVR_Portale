using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.ColliderEvent;

// Dieses Skript dient dazu, die VivePointer beim hovern über Buttons zu deaktivieren und sagt dem Skript "change_portal_dest.cs", ob der Index in der Portal-Liste inkrementiert oder dekrementiert wird
// Es gibt immer einen Vorwärts- und einen Rückwärts-Button
public class Change_Portal_Dest_Button : MonoBehaviour
    , IColliderEventPressEnterHandler
{
    [Tooltip("Richtung: nächstes Protal oder vorhergehendes Protal")]
    public bool Forward = true;

    public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    {
        // Index in der Portal-Liste von "change_portal_dest.cs" inkrementieren oder dekrementieren
        transform.GetComponentInParent<Change_Portal_Dest>().changeDestination(Forward);
    }
}
