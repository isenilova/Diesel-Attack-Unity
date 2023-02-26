using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class GUIScore : MonoBehaviour
{
    public int myScore = 0;

    public int realScore;

    public int stepScore = 1;
    
    
    public int zeroNum = 7;

    private Text myTxt;
    
    // Start is called before the first frame update
    void Start()
    {
        myTxt = gameObject.GetComponent<Text>();
        
        if(!PlayerPrefs.HasKey("Score")) PlayerPrefs.SetInt("Score", 0);

        myScore = PlayerPrefs.GetInt("Score");

        realScore = myScore;
        
        transforScores();
    }

    // Update is called once per frame
    void Update()
    {

        if (realScore > myScore)
        {
            myScore += stepScore;

            if (myScore > realScore) myScore = realScore;


            transforScores();
            




        }

    }

    void transforScores()
    {
        string st = "";
        int sc = myScore;
        int nm = 0;


        while (sc > 0)
        {
            nm++;

            sc = (int) (sc / 10);


        }



        for (int i = 0; i < (zeroNum-nm); i++)
        {

            st = st + "0";

        }

        st = st + myScore.ToString();


        myTxt.text = st;
    }

    public void addScore(int i)
    {

        realScore += i;
        
        PlayerPrefs.SetInt("Score", realScore);
        
        
        
        //transforScores();


    }

}
