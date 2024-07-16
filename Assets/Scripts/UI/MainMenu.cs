using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : NetworkBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Relay relayManager;
    private GameType gameType;

    public void onClick()
    {
        if (gameType == GameType.GAME_TYPE_HOST)
        {
            //SceneManager.LoadScene(sceneIndex);
            NetworkManager.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        else if (gameType == GameType.GAME_TYPE_JOIN)
        {
            relayManager.onJoinGame();
        }
    }

    public void setGameType(int gameType)
    {
        this.gameType = (GameType)gameType;
    }
}

public enum GameType
{
    GAME_TYPE_HOST,
    GAME_TYPE_JOIN

}
