using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interaction_Controller : MonoBehaviour
{
    public Collider2D UpCollider;
    public Collider2D DownCollider;
    public Collider2D LeftCollider;
    public Collider2D RightCollider;

    void Start()
    {
        // Initialize all colliders if necessary
        // This assumes the colliders are already assigned in the inspector
        if (!UpCollider || !DownCollider || !LeftCollider || !RightCollider)
        {
            Debug.LogError("All colliders must be assigned in the inspector.");
        }
        else
        {
            UpCollider.enabled = false;
            DownCollider.enabled = false;
            LeftCollider.enabled = false;
            RightCollider.enabled = true;
        }
    }
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpCollider.enabled = true;
            DownCollider.enabled = false;
            LeftCollider.enabled = false;
            RightCollider.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            UpCollider.enabled = false;
            DownCollider.enabled = true;
            LeftCollider.enabled = false;
            RightCollider.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            UpCollider.enabled = false;
            DownCollider.enabled = false;
            LeftCollider.enabled = true;
            RightCollider.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            UpCollider.enabled = false;
            DownCollider.enabled = false;
            LeftCollider.enabled = false;
            RightCollider.enabled = true;
        }
    }

    public void EnableColliders()
    {
        UpCollider.enabled = true;
        DownCollider.enabled = true;
        LeftCollider.enabled = true;
        RightCollider.enabled = true;
    }

    public void DisableColliders()
    {
        UpCollider.enabled = false;
        DownCollider.enabled = false;
        LeftCollider.enabled = false;
        RightCollider.enabled = false;
    }

    public void ToggleColliders(bool enable)
    {
        UpCollider.enabled = enable;
        DownCollider.enabled = enable;
        LeftCollider.enabled = enable;
        RightCollider.enabled = enable;
    }
}
