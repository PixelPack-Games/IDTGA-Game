using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : Enemy
{
    public Slime(string name, string id, GameObject gameObject, int maxHealth, int attack, int defense, int speed, int fleeRating) : base(name, id, gameObject, maxHealth, attack, defense, speed, fleeRating)
    {
        description = "Not much to look at, but a formidible foe to those just starting out... if they were to hit back";
    }

}
