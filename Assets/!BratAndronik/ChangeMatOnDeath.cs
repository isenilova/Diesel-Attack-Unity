using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatOnDeath : MonoBehaviour
{
    public Material newMats;
    public GameObject objMat;

    public GameObject[] disObject;

    private OneHealth lifeScr;
    private bool isDeath = false;


    private Renderer myRenderer;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        lifeScr = gameObject.GetComponent<OneHealth>();

        myRenderer = objMat.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if ((lifeScr.curHealth <= 0f) && (!isDeath))
        {

            isDeath = true;


            myRenderer.material = newMats;


            for (int i = 0; i < disObject.Length; i++)
            {
                
                disObject[i].SetActive(false);
                
            }

            
            
            //Destroy(gameObject, 0.1f);

        }

    }
}
