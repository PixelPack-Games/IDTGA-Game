using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : NetworkBehaviour
{
    [SerializeField] private string sceneName;
    private GameType gameType;

    public void onClick()
    {
        if (gameType == GameType.GAME_TYPE_HOST)
        {
            //SceneManager.LoadScene(sceneIndex);
            NetworkManager.Singleton.Shutdown();
            //NetworkManager.gameObject.SetActive(false);
            Destroy(NetworkManager.gameObject);
            SceneManager.LoadScene(sceneName);
        }
    }

    public void setGameType(int gameType)
    {
        this.gameType = (GameType)gameType;
    }
}


