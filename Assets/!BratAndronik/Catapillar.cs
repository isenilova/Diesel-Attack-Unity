using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapillar : MonoBehaviour
{
    private Renderer myRend;
    private Material myMat;
    
    Vector2 myOffset = new Vector2(0, 0);
    public float mySpeed = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        myRend = gameObject.GetComponent<Renderer>();
        myMat = myRend.materials[0];

    }

    // Update is called once per frame
    void Update()
    {
        myOffset = myOffset + new Vector2(mySpeed  * Time.deltaTime, 0f);
        
        myRend.material.SetTextureOffset("_MainTex", myOffset);
        
        
    }
}
