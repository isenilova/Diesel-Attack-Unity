using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavePure
{

    public string curPref = "00";
    public Dictionary<string, int> knownLevels = new Dictionary<string, int>();
    public Dictionary<string, int> hideLevels = new Dictionary<string, int>();

    public int load = 0;
    public int deathCnt = 0;
    public int diamCnt = 0;

    public int earthDash = 0;

    public int penguin = 0;
    public int wizard = 0;
    public int spider = 0;


    public Dictionary<string, string> levelDescr = new Dictionary<string, string>();
    public Dictionary<string, List<int>> diamondGet = new Dictionary<string, List<int>>();
    public Dictionary<string, int> diamondCnt = new Dictionary<string, int>();

}
