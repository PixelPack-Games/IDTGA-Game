using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class jyj_timer : MonoBehaviour
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

public abstract class Timer
{
    protected float initTime;
    protected float currTime { get; set; }
    public bool isRunning { get; protected set; }
    private float progress => currTime / initTime;
    public Action onTimerStart = delegate { };
    public Action onTimerStop = delegate { };

    protected Timer(float time)
    {
        initTime = time;
        isRunning = false;
    }

    public void start()
    {
        if (isRunning)
        {
            Debug.Log("Timer already running");
            return;
        }

        currTime = initTime;
        isRunning = true;
        onTimerStart.Invoke();
    }

    public void stop()
    {
        if (!isRunning)
        {
            Debug.Log("No timer currently running");
            return;
        }

        isRunning = false;
        onTimerStop.Invoke();
    }

    public void resume()
    {
        isRunning = true;
    }

    public void pause()
    {
        isRunning = false;
    }

    public abstract void tick(float deltaTime);
}

public class Countdown : Timer
{
    public Countdown(float time) : base(time) { }

    public override void tick(float deltaTime)
    {
        if (!isRunning)
        {
            return;
        }

        if (currTime <= 0)
        {
            stop();
            return;
        }

        currTime -= deltaTime;
    }

    public bool isDone()
    {
        return (currTime <= 0);
    }

    public void reset()
    {
        currTime = initTime;
    }

    public void reset(float time)
    {
        initTime = time;
        reset();
    }
}
