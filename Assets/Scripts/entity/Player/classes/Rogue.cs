using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : Player
{
    //Constructors
    public Rogue(string name, string id, GameObject gameObject, int maxHealth, int attack, int defense, int speed, int fleeRating) : base(name, id, gameObject, maxHealth, attack, defense, speed, fleeRating)
    {
        description = "A fast and stealthy class that can debuff enemies and dodge attacks with ease, but has low defenses";
        Entity bow = new Entity("Bow", "bow", gameObject);
        Entity rapier = new Entity("Rapier", "rapier", gameObject);
        Entity dagger = new Entity("Dagger", "dagger", gameObject);
        weapons.add(ref rapier);
        weapons.add(ref dagger);
        weapons.add(ref bow);
        armor = new Entity("Light Armor", "lightArmor", gameObject);
    }

    //TODO: Add functions based on skills; make them override generic player skills; maybe make it a skills class?
}
