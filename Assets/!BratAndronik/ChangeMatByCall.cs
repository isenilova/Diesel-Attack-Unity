using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatByCall : MonoBehaviour
{
    public Material[] ChangeMat;

    Renderer myRend;
    private Material[] newmats;
     
    // Start is called before the first frame update
    void Start()
    {
        myRend = gameObject.GetComponent<Renderer>();
        newmats = new Material[myRend.materials.Length];

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMatFunc(int i)
    {
        if (i >= ChangeMat.Length) return;



        newmats[0] = ChangeMat[i];

        for (int k = 1; k < myRend.materials.Length; k++) newmats[k] = myRend.materials[k];

        myRend.materials = newmats;

    }


}
