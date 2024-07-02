using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;

public class Relay : MonoBehaviour
{
    [SerializeField] private GameObject canavs;
    [SerializeField] private int MAX_PLAYERS;
    [SerializeField] private TextMeshProUGUI joinCode;
    [SerializeField] private TMP_InputField enterCode;
    private UnityTransport transport;

   private async void Awake()
    {
        transport = FindObjectOfType<UnityTransport>();
        canavs.SetActive(false);
        await Authenticate();
        canavs.SetActive(true);
    }

    private static async Task Authenticate()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void onCreateGame()
    {
        canavs.SetActive(false);
        Allocation alloc = await RelayService.Instance.CreateAllocationAsync(MAX_PLAYERS);
        joinCode.text = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
        transport.SetHostRelayData(alloc.RelayServer.IpV4, (ushort)alloc.RelayServer.Port, alloc.AllocationIdBytes, alloc.Key, alloc.ConnectionData);
        NetworkManager.Singleton.StartHost();
    }

    public async void onJoinGame()
    {
        canavs.SetActive(false);
        JoinAllocation alloc = await RelayService.Instance.JoinAllocationAsync(enterCode.text);
        transport.SetClientRelayData(alloc.RelayServer.IpV4, (ushort)alloc.RelayServer.Port, alloc.AllocationIdBytes, alloc.Key, alloc.ConnectionData, alloc.HostConnectionData);
        NetworkManager.Singleton.StartClient();

    }
}
