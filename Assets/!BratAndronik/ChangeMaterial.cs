using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    private OneHealth lifeScr;

    public GameObject matObj;
    
    public Material mainMat;

    public Material[] newMats;

    public float[] lifesToChange;

    private int myMatStatus = -1;

    private int canMat = -1;

    private Renderer objRend;
    
    // Start is called before the first frame update
    void Start()
    {
        lifeScr = gameObject.GetComponent<OneHealth>();

        objRend = matObj.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        canMat = MatNum();
        
        if(canMat== -1) return;
        
        if(canMat <= myMatStatus) return;

        myMatStatus = canMat;


        objRend.material = newMats[myMatStatus];


    }


    int MatNum()
    {
        int k = -1;

        for (int i =0; i < lifesToChange.Length; i++)
        {
            if (lifeScr.curHealth < lifesToChange[i]) k = i;


        }

        return k;
    }


}
