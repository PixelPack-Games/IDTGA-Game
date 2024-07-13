using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Xml.Linq;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] public string Name = "playerName";
    [SerializeField] private string Id = "player1";
    [SerializeField] public int MaxHealth = 10;
    [SerializeField] public int attack = 1;
    [SerializeField] public int defense = 1;
    [SerializeField] private int speed = 1;
    [SerializeField] private int fleeRating = 5;
    public Entity entity;
    public Mage player;


    void Awake()
    {
      entity = new Mage(Name, Id, this.gameObject, MaxHealth, attack, defense, speed, fleeRating);
      player = (Mage)entity;
      Debug.Log(player.getName() + " created with " + player.getCurrHealth() + " health");
      
    }



// Update is called once per frame
void Update()
    {
        
    }
}