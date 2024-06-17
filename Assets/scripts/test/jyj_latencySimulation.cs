using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class jyj_latencySimulation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        GetComponent<UnityTransport>().SetDebugSimulatorParameters(120, 5, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
