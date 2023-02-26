using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FithLVlTimer : MonoBehaviour
{

    public float timeToAct = 5f;
    public float actSpeed= 100f;
    public float actEps = 1f;
    private Color dopCol;

    private bool corr = true;

    private float alfa;

    private bool dir = true;

    private Coroutine myCorrFunc;


    public string maxValue = "2500.00";
    
    // Start is called before the first frame update
    void Start()
    {
       myCorrFunc = StartCoroutine(ActiveTimer());
    }

    // Update is called once per frame
    void Update()
    {
        Color curColor;
        gameObject.GetComponent<Text>().text = ((float)(int)(TimeController.instance.tm*100)/100).ToString() +"/" + maxValue;

        timeToAct -= Time.deltaTime;


        if ((timeToAct < TimeController.instance.tm) && corr)
        {
            corr = false;
            StopCoroutine(myCorrFunc);
            curColor = gameObject.GetComponent<Text>().color;
            gameObject.GetComponent<Text>().color = new Color(curColor.r, curColor.g, curColor.b, 1f);
            
        }
    }


    IEnumerator ActiveTimer()
    {
      
        Color curColor;

        while (true)
        {

            if (dir)
            {
                curColor = gameObject.GetComponent<Text>().color;
                alfa = gameObject.GetComponent<Text>().color.a - actSpeed * Time.deltaTime;

                if (alfa > 0)
                    gameObject.GetComponent<Text>().color = new Color(curColor.r, curColor.g, curColor.b, alfa);

                else
                {
                    alfa = 0;
                    gameObject.GetComponent<Text>().color = new Color(curColor.r, curColor.g, curColor.b, alfa);
                    dir = false;

                }

                yield return null;

            }

            else
            {

                curColor = gameObject.GetComponent<Text>().color;
                alfa = gameObject.GetComponent<Text>().color.a + actSpeed * Time.deltaTime;

                if (alfa < 1f)
                    gameObject.GetComponent<Text>().color = new Color(curColor.r, curColor.g, curColor.b, alfa);

                else
                {
                    alfa = 1;
                    gameObject.GetComponent<Text>().color = new Color(curColor.r, curColor.g, curColor.b, alfa);
                    dir = true;

                }






            }




            yield return null;

        }
    }
}
