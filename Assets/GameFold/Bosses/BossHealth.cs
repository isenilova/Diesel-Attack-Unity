using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour {

    public static BossHealth instance;
    public GameObject view;

    public GameObject who;
    public string nm;

    public Text whoName;
    public Image whoHealth;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (who == null) return;


        if (!view.activeInHierarchy)
        {
            view.SetActive(true);
            view.GetComponentInChildren<Text>().text = nm;
        }

        float d = who.GetComponent<Health>().CurrentHealth;
        float d1 = who.GetComponent<Health>().MaximumHealth;

        whoHealth.fillAmount = d / d1;


    }



}
