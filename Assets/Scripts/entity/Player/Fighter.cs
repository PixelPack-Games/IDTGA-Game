using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Player
{
    //Properties
    private string description; //class description

    public Fighter(string name, string id, GameObject gameObject, int maxHealth, int attack, int defense, int speed, int fleeRating) : base (name, id, gameObject, maxHealth, attack, defense, speed, fleeRating)
    {
        description = "A physical class that excels in multiple weapon fighting styles";
        Entity sword = new Entity("Sword", "sword", gameObject);
        Entity axe = new Entity("Axe", "axe", gameObject);
        weapons.add(ref sword);
        weapons.add(ref axe);
        armor = new Entity("Medium Armor", "mediumArmor", gameObject);
        Debug.Log("Weapons: " + weapons.find("sword").getName() + " and " + weapons.find("axe").getName() + "\nArmor: " + armor.getName());
    }
}
