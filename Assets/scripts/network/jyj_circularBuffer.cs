using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jyj_circularBuffer : MonoBehaviour
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

public class CircularBuffer<T>
{
    T[] buffer;
    int bufferSize;

    public CircularBuffer(int bufferSize)
    {
        this.bufferSize = bufferSize;
        buffer = new T[bufferSize];
    }

    public void add(T item, int index)
    {
        buffer[index % bufferSize] = item;
    }

    public T get(int index)
    {
        return buffer[index % bufferSize];
    }

    public void reset()
    {
        buffer = new T[bufferSize];
    }

}
