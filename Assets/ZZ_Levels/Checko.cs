using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checko : MonoBehaviour
{
    public Playero plr;
    // Start is called before the first frame update

    public GameObject[] ships;
    // Update is called once per frame
    public Image main;

    private void Start()
    {
        //ClickShip(PlayerData.player.curShip);
    }

    void Update()
    {
        plr = PlayerData.player;
    }

    public void ClickShip(int index)
    {
        
        if (index == 1 && PlayerData.player.maxShip < 2) return;
        
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].GetComponent<Image>().sprite = ships[i].GetComponent<UnoSprite>().sprs[1];
            ships[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        
        ships[index].GetComponent<Image>().sprite = ships[index].GetComponent<UnoSprite>().sprs[0];
        ships[index].transform.GetChild(0).gameObject.SetActive(true);

        plr.curShip = index;

        main.sprite = ships[index].GetComponent<UnoSprite>().sprs[0];
        
        PlayerData.Save();
    }
}
