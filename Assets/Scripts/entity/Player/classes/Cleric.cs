using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Cleric : Player
{
    //Constructors
    public Cleric(string name, string id, GameObject gameObject, int maxHealth, int attack, int defense, int speed, int fleeRating) : base(name, id, gameObject, maxHealth, attack, defense, speed, fleeRating)
    {
        description = "A support class that can take hits and heal allies, but lacks in offensive power";
        Entity staff = new Entity("Staff", "staff", gameObject);
        Entity magic = new Entity("Divine Magic", "divine", gameObject);
        weapons.add(ref staff);
        weapons.add(ref magic);
        armor = new Entity("Heavy Armor", "heavyArmor", gameObject);
    }

    //TODO: Add functions based on skills; make them override generic player skills; maybe make it a skills class?

    //Functions
    public override bool singleAidSkill(Player target)
    {
        if (!base.singleAidSkill(target))
        {
            return false;
        }

        //TODO: make sure not healing an enemy
        target.restoreHealth(defense * 2);
        return true;
    }

    public override bool singleHarmSkill(Enemy target)
    {
        if (!base.singleHarmSkill(target))
        {
            return false;
        }

        target.takeDamage(attack + target.getDefense());
        return true;
    }
}
