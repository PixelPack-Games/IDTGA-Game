using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jyj_networkTimer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class NetworkTimer
{
    private float timer;
    public float minTimeBetweenTicks { get; }
    public int currTick { get; private set; }

    public NetworkTimer(float serverTickRate)
    {
        minTimeBetweenTicks = 1 / serverTickRate;

    }

    public void update(float deltaTime)
    {
        timer += deltaTime;
    }

    public bool shouldTick()
    {
        if (timer >= minTimeBetweenTicks)
        {
            timer -= minTimeBetweenTicks;
            currTick++;
            return true;
        }

        return false;
    }
}
