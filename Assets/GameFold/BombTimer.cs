using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombTimer : MonoBehaviour {

    public static BombTimer instance;
    // Use this for initialization
    public GameObject text;

    public Dictionary<GameObject, GameObject> tVals = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void SetTimer(GameObject go, int tm)
    {
        if (!tVals.ContainsKey(go))
        {
            var go1 = (GameObject)Instantiate(text);
            go1.transform.SetParent(transform);
            tVals.Add(go, go1);
        }

        if (tm < 0)
        {
            tVals[go].GetComponent<Text>().text = "";
        }
        else
        {
            tVals[go].GetComponent<Text>().text = tm.ToString();
        }
    }

    private void Update()
    {
        //track positions
        foreach (var df in tVals.Keys)
        {
            if (df == null) continue;
            tVals[df].transform.position =  Camera.main.WorldToScreenPoint(df.transform.position) + new Vector3(0, 50, 0);
        }
    }


}
