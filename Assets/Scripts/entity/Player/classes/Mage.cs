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
        string skillName = "Magic Missile";
        SkillType type = SkillType.SINGLE_HARM;
        skillNames.add(ref skillName);
        skillTypeList.add(ref type);
        skillName = "I Cast... FIREBALL!!!";
        type = SkillType.MULTI_HARM;
        skillNames.add(ref skillName);
        skillTypeList.add(ref type);
    }

    //TODO: Add functions based on skills; make them override generic player skills; maybe make it a skills class?

    public override bool singleHarmSkill(Enemy target)
    {
        if (!base.singleHarmSkill(target))
        {
            return false;
        }

        target.takeDamage((attack + target.getDefense()) * 4);
        return true;
    }

    public override int multiHarmSkill(ref LinkedList<Enemy> enemies)
    {
        int count = base.multiHarmSkill(ref enemies);
        int bogus = 0;

        while (enemies[bogus] != null)
        {
            enemies[bogus].takeDamage(((attack + enemies[bogus].getDefense()) * 4)/count);
            bogus++;
        }

        return 0; //the return value does not need to be used
    }
}
