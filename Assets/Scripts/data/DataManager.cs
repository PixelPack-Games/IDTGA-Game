using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager _dataManager;

    private void Awake()
    {
        if (_dataManager != null)
        {
            Destroy(gameObject);
            return;
        }

        _dataManager = this;
        DontDestroyOnLoad(gameObject);
    }
}
