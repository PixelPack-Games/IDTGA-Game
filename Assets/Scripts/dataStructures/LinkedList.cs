using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/*
 * @brief a custom Linked List class and Node inner class
 */
public class LinkedList<T> : INetworkSerializable
{

    //Properties
    private Node<T> head;

    //Constructor
    public LinkedList()
    {
        head = null;
    }

    //Implementations
    public void NetworkSerialize<T1>(BufferSerializer<T1> serializer) where T1 : IReaderWriter
    {
        string head = this.head.ToString();
        serializer.SerializeValue(ref head);
    }

    //Functions
    public void add(ref T data)
    {
        if (head == null)
        {
            head = new Node<T>(ref data);
            head.next = null;
            return;
        }

        Node<T> node = head;

        while (true)
        {
           if (node.next == null)
            {
                node.next = new Node<T>(ref data);
                node.next.next = null;
                break;
            }

            node = node.next;
        }
    }

    public T remove(string name)
    {
        if (head == null)
        {
            return default;
        }

        if (head.data.ToString().Equals(name))
        {
            Node<T> rem = head;
            head = head.next;
            return (rem.data);
        }

        Node<T> node = head;

        while (true)
        {
            if (node.next == null)
            {
                break;
            }

            if (node.next.data.ToString().Equals(name))
            {
                Node<T> rem = node.next;
                node.next = node.next.next;
                return (rem.data);
            }

            node = node.next;
        }

        return default;
    }

    public T find(string name)
    {
        if (head == null)
        {
            return default;
        }

        Node<T> node = head;

        while (true)
        {
            if (node == null)
            {
                return default;
            }

            if (node.data.ToString().Equals(name))
            {
                return (node.data);
            }

            node = node.next;
        }
    }

    //Debug
    public void iterate()
    {
        Node<T> node = head;

        while (true)
        {
            if (node == null)
            {
                break;
            }

            Debug.Log(node.data);
            node = node.next;
        }
    }

    //Node
    private class Node<E> : INetworkSerializable
    {
        public E data;
        public Node<E> next;

        //Constructor
        public Node(ref E data)
        {
            this.data = data;
        }

        //Implementations
        public void NetworkSerialize<T1>(BufferSerializer<T1> serializer) where T1 : IReaderWriter
        {
            string data = this.data.ToString();
            serializer.SerializeValue(ref data);
        }
    }
}
