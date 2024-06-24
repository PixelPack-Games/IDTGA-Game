using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/*
 * @brief an Actor class that extends the Entity class, used as a base class for any entity that acts
 */
public class Actor : Entity
{
    //Properties
    protected int currHealth, maxHealth, attack, defense, speed, fleeRating; //actor stats
    private bool alive; //is this actor alive?

    //Contructors

    public Actor() : base()
    {

    }
    public Actor(string name, string id, GameObject gameObject, int maxHealth, int attack, int defense, int speed, int fleeRating) : base(name, id, gameObject)
    {
        this.maxHealth = maxHealth;
        currHealth = maxHealth;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        this.fleeRating = fleeRating;
        alive = true;
    }

    //Implementations
    public new void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        base.NetworkSerialize<T>(serializer);
        serializer.SerializeValue(ref currHealth);
        serializer.SerializeValue(ref maxHealth);
        serializer.SerializeValue(ref attack);
        serializer.SerializeValue(ref defense);
        serializer.SerializeValue(ref speed);
        serializer.SerializeValue(ref alive);
    }

    //Functions
    public void takeDamage(int damage)
    {
        currHealth -= damage;

        if (currHealth <= 0)
        {
            alive = false;
            //DO NOT die() immediately, as the actor may have special actions (like animations or dialogue) it needs to take before being destroyed
        }
    }

    private void restoreHealth(int heal)
    {
        currHealth += heal;

        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }

        alive = true;
    }

    //Getters
    public int getCurrHealth()
    {
        return (currHealth);
    }

    private int getMaxHealth()
    {
        return (maxHealth);
    }

    private int getAttack()
    {
        return (attack);
    }

    public int getDefense()
    {
        return (defense);
    }

    private int getSpeed()
    {
        return (speed);
    }

    private bool getAlive()
    {
        return (alive);
    }

    public int getFleeRating()
    {
        return (fleeRating);
    }

    //Setters
    private void setCurrhealth(int currHealth)
    {
        this.currHealth = currHealth;
    }

    private void setMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    private void setAttack(int attack)
    {
        this.attack = attack;
    }

    private void setDefense(int defense)
    {
        this.defense = defense;
    }

    private void setSpeed(int speed)
    {
        this.speed = speed;
    }

    private void setAlive(bool alive)
    {
        this.alive = alive;
    }

    private void setFleeRating(int fleeRating)
    {
        this.fleeRating = fleeRating;
    }
}
