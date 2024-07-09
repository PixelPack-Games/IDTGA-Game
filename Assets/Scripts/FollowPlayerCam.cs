using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FollowPlayerCam : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public GameObject player;
    public Transform FollowTarget;
    // Start is called before the first frame update
    void Start()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                FollowTarget = player.transform;
                vcam.LookAt = FollowTarget;
                vcam.Follow = FollowTarget;
            }
        }
    }
}
