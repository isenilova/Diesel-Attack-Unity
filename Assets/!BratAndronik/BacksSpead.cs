using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacksSpead : MonoBehaviour
{

    public GameObject[] objBacks;

    public float timeToAdd = 10f;

    public float addSpdCoef = 1f;
    public float multypleSpdCoef = 1f;

    private float tm = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tm += Time.deltaTime;

        if (tm >= timeToAdd)
        {

            for (int i = 0; i < objBacks.Length; i++)
            {
                objBacks[i].GetComponent<BackScroll>().speed *= multypleSpdCoef;
                objBacks[i].GetComponent<BackScroll>().speed += addSpdCoef;

            }

            tm = 0;
        }
    }
}
