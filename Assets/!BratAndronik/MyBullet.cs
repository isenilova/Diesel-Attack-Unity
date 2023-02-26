using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBullet : MonoBehaviour
{

    public GameObject myExplos;

    private Transform myChild;
    
    // Start is called before the first frame update
    void Start()
    {
        myChild = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(myChild == null){
           if(myExplos != null) Instantiate(myExplos, transform.position, Camera.main.transform.rotation);
            Destroy(gameObject);


        }
    }
}
