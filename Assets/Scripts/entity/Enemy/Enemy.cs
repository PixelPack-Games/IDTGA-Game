using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/*
 * @brief enemy class that extends from Actor class which will contain base features all enemies will share
 */
public class Enemy : Actor
{
    //Properties
    private int level; //level values; no need for exp values for enemies
    protected LinkedList<Entity> weapons; //list of weapons this player has; TODO: change Entity to a Weapon class when implemented
    protected Entity armor; //amor this enemy has on (may increase defence)
    protected string description; //class description

    //Constructors
    public Enemy(string name, string id, GameObject gameObject, int maxHealth, int attack, int defense, int speed, int fleeRating) : base(name, id, gameObject, maxHealth, attack, defense, speed, fleeRating)
    {
        level = 1;
        weapons = new LinkedList<Entity>();
    }

    public Enemy(string name, string id, GameObject gameObject, int maxHealth, int attack, int defense, int speed, int fleeRating, int level) : base(name, id, gameObject, maxHealth, attack, defense, speed, fleeRating)
    {
        this.level = level;
    }

    //Implementtations
    public new void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        base.NetworkSerialize<T>(serializer);
        serializer.SerializeValue(ref level);
        serializer.SerializeValue(ref description);
        serializer.SerializeValue(ref weapons);
        serializer.SerializeValue(ref armor);
    }

    //Functions
    public void attackAction(Actor other)
    {
        int damage = attack - other.getDefense();

        if (damage < 0)
        {
            damage = 0;
        }

        other.takeDamage(damage);
    }

    private void useItem(Entity item, NetworkObject net) //TODO: change Entity to Item when Item class is implemented
    {
        //TODO: Implement using item after Item class has been implemented

        item.die(net);
    }

   
    //Getters
    private int getLevel()
    {
        return (level);
    }


    public LinkedList<Entity> getWeapons()
    {
        return (weapons);
    }

    private Entity getarmor()
    {
        return (armor);
    }

    private string getDescription()
    {
        return (description);
    }




    //Setters
    private void setLevel(int level)
    {
        this.level = level;
    }

    private void setWeapons(LinkedList<Entity> weapons)
    {
        this.weapons = weapons;
    }

    private void setArmor(Entity armor)
    {
        this.armor = armor;
    }

    private void setDescription(string description)
    {
        this.description = description;
    }
}