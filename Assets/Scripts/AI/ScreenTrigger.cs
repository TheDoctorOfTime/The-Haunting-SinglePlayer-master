using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        GameObject.Find("Overlays").GetComponent<OverlayControls>().SetGSActive(false);
    }
}
