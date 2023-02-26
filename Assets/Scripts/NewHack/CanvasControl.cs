using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CanvasControl : MonoBehaviour
{
    private int rollCost = 500;
    public static string walletNick;
    public static bool wasLogged;
    
    public GameObject login;

    public GameObject infoCant;

    public TextMeshProUGUI nm;

    public Sprite[] shipSpr;
    public Sprite[] weapSpr;
    public List<string> shipNames;
    public List<string> gunNames;
    
    // Start is called before the first frame update
    [Header("Shop")] 
    public GameObject loading;
    public GameObject result;
    public GameObject serverResp;
    public Image resImage;
    public TextMeshProUGUI resName;
    public Text gold;
    public GameObject[] weapons;
    public GameObject[] ships;

    public void Start()
    {
        if (wasLogged)
        {
            if (login != null) login.SetActive(false);
        }
    }

    public void Skip()
    {
        login.SetActive(false);
        walletNick = "no";
        wasLogged = true;
    }
    
    public void Accept()
    {
        Debug.Log(nm.text);
        if (nm.text.Length <= 1)
        {
            infoCant.SetActive(true);
            return;
        }

        login.SetActive(false);
        walletNick = nm.text;
        walletNick = walletNick.Replace(" ", String.Empty);
        wasLogged = true;
    }

    public void OkInfo()
    {
        infoCant.SetActive(false);
    }
    
    public void RollForNFT()
    {
        Debug.Log("Lets GO !");
        //time to roll
        //requesto !
        int money = PlayerPrefs.GetInt("Score");
        if (money < rollCost)
        {
            infoCant.SetActive(true);
        }
        else
        {
            money -= rollCost;
            PlayerPrefs.SetInt("Score", money);
            gold.text = money.ToString() + " points";
            StartCoroutine(TestSmart(walletNick));
        }
    }
    
    public IEnumerator TestSmart(string nearID)
    {
        loading.SetActive(true);
        serverResp.SetActive(true);
        result.SetActive(false);
        
        var rr = "https://dieselattack.com/api/mint-nft?nearid=" + nearID + ".testnet";
        rr = rr.Replace("\u200B", "");
        Debug.Log(rr);
        var w = new WWW(rr);

        yield return w;
        
        Debug.Log(w.text);
        
        serverResp.SetActive(false);
        result.SetActive(true);
        
        ParseResult(w.text);
        //ParseResult("gun-3");
        
    }

    public void ParseResult(string str)
    {
        if (str.IndexOf("ship") >= 0)
        {
            //it s a ship
            var ss = str.Substring(5);
            int t = int.Parse(ss);
            resImage.sprite = shipSpr[t - 1];
            resName.text = shipNames[t - 1];
            
            ships[t-1].SetActive(true);
        }
        else
        {
            //its a gun
            var ss = str.Substring(4);
            int t = int.Parse(ss);
            resImage.sprite = weapSpr[t - 1];
            resName.text = gunNames[t - 1];
            
            weapons[t-1].GetComponent<GUIShopItm>().GetBuyed();
            int xx = 1 << (t - 1);
            int ii = PlayerPrefs.GetInt("Weap");
            int rr = xx & ii;
            if (rr == 0)
            {
                ii = ii | xx;
                PlayerPrefs.SetInt("Weap", ii);
            }
        }
    }

    public void OkResult()
    {
        result.SetActive(false);
        loading.SetActive(false);
    }
    
}
