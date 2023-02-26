using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill : BaseObj
{

    public string nm = "skill";
    public string skillname = "skill";
    public string description;

    public int atkId;

    public string tp;

    public string slot = "";
    public string usedSlot = "";
    public int numSlot = 0;
    public int quick = 0;
    
    public int amount = 1;

    public int numInv;

    public string assignTag = "pick";

    public float rotX = 0;
    public float rotY = 0;
    public float rotZ = 0;


    public int monLvlReq = 0;

    public int reqPlvl = 0;
    public int reqInt = 0;
    public int reqDes = 0;
    public int reqStr = 0;

    public Damage dmg;
    public float dmgMultiplier = 1.0f;
    public float dmgReceived = 1.0f;
    public float castTime = 1.0f;

    //shitty can be deleted
    public float lifeTime = 100;
    public string monsterRef = "";
    public string element = "";

    public int useMonsterRange = 1;

    public int passive = 0;
    public string useItem = ""; 


    public KeyValuePair<string, int> depSkill;
    public int maxLvl = 10;
    public int skillUpcost = 1;
    public float skillCd = 1;
    public int sklHcost = 0;
    public int sklMcost = 0;
    public int sklScost = 0;




    public string prefabName;
    public string preffix = "Items";

    public string preffix2D = "Skills2D";
    public string prefabName2D;

    //ok here is where fancy stuff began
    //we create a projectile which moves at speed and when hits the target triggers the effect
    public string skillStyle = "melee";
    //range, magic


    public float area = 50;
    public float range = 100;
    public float minRange = 0;
    public string target = "point";  //can be 'target' or 'point'
    public string skillType = "all"; //enemy, ally, all
    public int visibleProj = 1; //projectile is visible
    public string projEffect = ""; //effect of moving projectile
    public string buffApply = "";  //buffApplying
    public int projAmount = 1;   //amount of projectiles
    public int projPar = 1;
    public float projDelay = 0.1f;
    public float projSpd = 10.0f;
    public string triggerEffect = "";
    public string areaEffect = "";
    public int targetNum = 100;
    //possible decal effect ?
    //spawning monster
    public string monsterSpawn = "";
    public int spawnLvl = 1;

    public string upgradeMonster = "";
    public int useMain = 0;

    //do we watch direction we use skill
    public int dirEnd = 1;

    //animation of skill
    public string sklAn = "attack";

    public int sklAmount = 1;
    public int useAmnt = 1;

    public string baseSkill = "";
    public string nextSkill = "";
    public int skillLvl = 1;


    public int addf = 0;

    public List<KeyValuePair<string, int>> buffsSet = new List<KeyValuePair<string, int>>();
    //list of buffs with levels i suppose


    public Dictionary<string, List<KeyValuePair<string, string>>> behaviour;

    public static string GetSkill(string mon, int level)
    {
        if (level == 1) return mon;

        return mon + "_xX" + level.ToString();
    }

    public static Skill GetChainSkill(string skl, int lvl)
    {
        if (lvl == 0)
        {
            return null;
        }

        var skl1 = (Skill)DatabaseAll.instance.data["Skill"][skl];
        if (skl1.maxLvl < lvl)
        {
            return null;
        }

        while (skl1.skillLvl < lvl)
        {
            skl1 = (Skill)DatabaseAll.instance.data["Skill"][skl1.nextSkill];
        }
        return skl1;

    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
};
