using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TrainingDummy : Enemy
{
    //Constructors
    public TrainingDummy(string name, string id, GameObject gameObject, int maxHealth, int attack, int defense, int speed, int fleeRating) : base(name, id, gameObject, maxHealth, attack, defense, speed, fleeRating)
    {
        description = "Not much to look at, but a formidible foe to those just starting out... if they were to hit back";
        armor = new Entity("Light Armor", "lightArmor", gameObject);
        Debug.Log("Armor: " + armor.getName());
    }

}
