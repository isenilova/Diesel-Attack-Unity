using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{
     GameObject myScore;
    private GUIScore scoreScr;

    public int scoreForMe = 10;

    private bool addscore = false;

    private OneHealth lifeScr;
    
    // Start is called before the first frame update
    void Start()
    {

        myScore = GameObject.FindGameObjectWithTag("Score");
        
        scoreScr = myScore.GetComponent<GUIScore>();

        lifeScr = gameObject.GetComponent<OneHealth>();


    }

    // Update is called once per frame
    void Update()
    {
        if(addscore) return;

        if (lifeScr.curHealth < 0)
        {
            addscore = true;
            
            scoreScr.addScore(scoreForMe);


        }


    }
}
