using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeData : MonoBehaviour
{
    public static SceneChangeData Instance;
    public GameObject player;
    public loadNextScene loadNextScene;
    // Start is called before the first frame update
    void Start()
    {
         
}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
