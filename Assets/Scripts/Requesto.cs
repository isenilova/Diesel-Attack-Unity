using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Requesto : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TestSmart("uberbax666.testnet"));
    }

    public IEnumerator TestSmart(string nearID)
    {
        var rr = "https://dieselattack.com/api/mint-nft?nearid=" + nearID;
        rr = rr.Replace("\u200B", "");
        Debug.Log(rr);
        var w = new WWW(rr);

        yield return w;
        
        Debug.Log(w.text);
    }

}
