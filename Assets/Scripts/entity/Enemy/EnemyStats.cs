using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private string Name = "enemyname";
    [SerializeField] private string Id = "enemy1";
    [SerializeField] private int MaxHealth = 10;
    [SerializeField] private int attack = 1;
    [SerializeField] private int defense = 1;
    [SerializeField] private int speed = 1;
    [SerializeField] private int fleeRating = 1;
    private Entity entity;
    public Enemy enemy;



    void Awake()
    {
        entity = new Enemy(Name, Id, this.gameObject, MaxHealth, attack, defense, speed, fleeRating);
        enemy = (Enemy)entity;
        Debug.Log(enemy.getName() + " created with " + enemy.getCurrHealth() + " health");
       
    }
}
