using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDestroy : MonoBehaviour {

    public Material matswap;

    public GameObject[] parts;
	// Use this for initialization
	public void Do(float prc)
    {
        float u = parts.Length;
        float pt = 1 / u;

        for (int i = 0; i < parts.Length; i++)
        {
            float bd = 1 - pt * (i + 1);

            if (prc <= bd)
            {
                parts[i].GetComponent<Renderer>().material = matswap;
            }
        }
    }
}
