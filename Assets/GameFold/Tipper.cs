using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tipper : MonoBehaviour {

    public Text tipText;
    string standart = "<color=yellow>Tip:</color>";

    public List<string> tips = new List<string>();
	// Use this for initialization
	void Start ()
    {
        tips.Add("This forest is full of danger - always be ready !");
        tips.Add("Did you know that you can shoot down and up ? Try it - gonna be fun");
        tips.Add("Some enemies may left keys after dying - be sure you check it");
        tips.Add("Stuck ? Press ESC to restart level or chose one from level map");
        tips.Add("There are many bosses in forest, to escape it you should defeat all of them");
        tips.Add("Destroy barrels or crates to enable hidden passages or gain battle advantage");
        tips.Add("Some buttons can be triggered only by falling on them from high places ");



        tipText.text = standart + "  " +  tips[Random.Range(0, tips.Count)];


    }


}
