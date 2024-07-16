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
    [SerializeField] private GameObject buttons;
    [SerializeField] private int MAX_PLAYERS;
    [SerializeField] private TextMeshProUGUI joinCode;
    [SerializeField] private TMP_InputField enterCode;
    [SerializeField] private GameObject startButton;
    private UnityTransport transport;

   private async void Awake()
    {
        transport = FindObjectOfType<UnityTransport>();
        buttons.SetActive(false);
        await Authenticate();
        buttons.SetActive(true);
    }

    private static async Task Authenticate()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void onCreateGame()
    {
        buttons.SetActive(false);
        Allocation alloc = await RelayService.Instance.CreateAllocationAsync(MAX_PLAYERS);
        joinCode.text = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
        startButton.SetActive(true);
        transport.SetHostRelayData(alloc.RelayServer.IpV4, (ushort)alloc.RelayServer.Port, alloc.AllocationIdBytes, alloc.Key, alloc.ConnectionData);
        NetworkManager.Singleton.StartHost();
    }

    public async void onJoinGame()
    {
        buttons.SetActive(false);
        JoinAllocation alloc = await RelayService.Instance.JoinAllocationAsync(enterCode.text);
        transport.SetClientRelayData(alloc.RelayServer.IpV4, (ushort)alloc.RelayServer.Port, alloc.AllocationIdBytes, alloc.Key, alloc.ConnectionData, alloc.HostConnectionData);
        NetworkManager.Singleton.StartClient();

    }
}
