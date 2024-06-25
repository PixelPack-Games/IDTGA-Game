using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Player
{
    //Constructors
    public Mage(string name, string id, GameObject gameObject, int maxHealth, int attack, int defense, int speed, int fleeRating) : base(name, id, gameObject, maxHealth, attack, defense, speed, fleeRating)
    {
        description = "A magical class that can cast powerful spells, but is very vulnerable to enemy attacks";
        Entity dagger = new Entity("Dagger", "dagger", gameObject);
        Entity magic = new Entity("Arcane Magic", "arcane", gameObject);
        weapons.add(ref magic);
        weapons.add(ref dagger);
        armor = new Entity("Light Armor", "lightArmor", gameObject);
    }

    //TODO: Add functions based on skills; make them override generic player skills; maybe make it a skills class?
}
