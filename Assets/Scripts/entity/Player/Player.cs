using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/*
 * @brief player class that extends from Actor class whic hwill contain base features all players will share
 */
public class Player : Actor
{
    //Properties
    private int level, exp; //level and exp values; when exp reaches 100 level increases by 1
    protected LinkedList<Entity> weapons; //list of weapons this player has; TODO: change Entity to a Weapon class when implemented
    protected Entity armor; //amor this player has on
    protected Inventory inventory;
    protected string description; //class description
    protected LinkedList<string> skillNames; //list of all skill names for this player
    protected LinkedList<SkillType> skillTypeList; //list of all skill types (mirrors skill names)

    //Constructors
    public Player(string name, string id, GameObject gameObject, int maxHealth, int attack, int defense, int speed, int fleeRating) : base(name, id, gameObject, maxHealth, attack, defense, speed, fleeRating)
    {
        level = 1;
        exp = 0;
        weapons = new LinkedList<Entity>();
        inventory = gameObject.GetComponent<PlayerInventory>().inventory.Container;
        skillNames = new LinkedList<string>();
        skillTypeList = new LinkedList<SkillType>();
    }

    public Player(string name, string id, GameObject gameObject, int maxHealth, int attack, int defense, int speed, int fleeRating, int level, int exp) : base(name, id, gameObject, maxHealth, attack, defense, speed, fleeRating)
    {
        this.level = level;
        this.exp = exp;
    }

    //Implementations
    public new void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        base.NetworkSerialize<T>(serializer);
        serializer.SerializeValue(ref level);
        serializer.SerializeValue(ref exp);
        serializer.SerializeValue(ref description);
        serializer.SerializeValue(ref weapons);
        serializer.SerializeValue(ref armor);
        serializer.SerializeValue(ref skillNames);
        serializer.SerializeValue(ref skillTypeList);

        /*
        int bogus = 0;

        while (skillNames[bogus] != null)
        {
            string temp = skillNames[bogus];
            serializer.SerializeValue(ref temp);
            SkillType tempType = skillTypeList[bogus];
            serializer.SerializeValue(ref tempType);
        }
        */
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

    private bool canRun(Actor[] others, int partySize)
    {
        System.Random rand = new System.Random();

        for (int bogus = 0; bogus < others.Length; bogus++)
        {
            if (fleeRating + rand.Next(partySize) < others[bogus].getFleeRating() + rand.Next(others.Length))
            {
                return false;
            }
        }

        return true;
    }

    private void run(Actor[] others, Player[] party)
    {
        if (!canRun(others, party.Length))
        {
            Debug.Log("Can't run!");
            return;
        }

        for (int bogus = 0; bogus < party.Length; bogus++)
        {
            party[bogus].runSuccess();
        }
    }

    private void runSuccess()
    {
        //TODO: implement running from battles
    }

    public void increaseExp(int increase)
    {
        exp += increase;

        if (exp >= 100)
        {
            levelUp();
        }
    }

    private void levelUp()
    {
        attack++;
        defense++;
        speed++;
        level++;
        exp -= 100;
        Debug.Log(getName() + " is now level " + level);
    }

    public virtual bool singleAidSkill(Player target)
    {
        return (target != null);
    }

    public virtual bool singleHarmSkill(Enemy target)
    {
        return (target != null);
    }

    public virtual void selfAidSkill()
    {
        
    }

    public virtual int multiHarmSkill(ref LinkedList<Enemy> enemies)
    {
        int bogus = 0;

        while (enemies[bogus] != null)
        {
            bogus++;
        }

        return (bogus + 1);
    }

    //Getters
    public int getLevel()
    {
        return (level);
    }

    private int getExp()
    {
        return (exp);
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

    private void setExp(int exp)
    {
        this.exp = exp;
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

public enum SkillType
{
    NONE,
    SINGLE_AID,
    SINGLE_HARM,
    MULIT_AID,
    MULTI_HARM,
    SELF_AID
}
