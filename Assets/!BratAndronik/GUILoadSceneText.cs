using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class GUILoadSceneText : MonoBehaviour
{

    public string LoadScene = "Level_1_1";

    public string mytext = "This is the test text shown below the 1th level starts.";

    private Text txt;

    public float nextLetter = 0.1f;

    private float tm = 0;
    private int curnum = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        txt = gameObject.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (curnum >= mytext.Length)
        {
            FindObjectOfType<AudioSource>().Stop();
            return;
        }

        tm += Time.deltaTime;


        if (tm >= nextLetter)
        {
            tm = 0;
            txt.text = txt.text + mytext[curnum];

            curnum++;


        }


    }
}
