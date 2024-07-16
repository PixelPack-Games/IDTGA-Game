using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static LinkedList<DataManager> _dataManager;

    private void Awake()
    {
        if (_dataManager == null)
        {
            _dataManager = new LinkedList<DataManager>();
        }

        DontDestroyOnLoad(gameObject);
        DataManager data = this;
        _dataManager.add(ref data);

    }
}
