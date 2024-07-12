using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedListTest : MonoBehaviour
{
    private LinkedList<int> nums;
    public int count = 0;
    void Start()
    {
        nums = new LinkedList<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            nums.add(ref count);
            Debug.Log("Added: " + nums[count++]);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Removing: " + nums.remove(--count));
        }
    }
}
