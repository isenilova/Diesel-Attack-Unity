using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float mySpeed = 20f;
    public float lifeTime = 20f;

    public bool destrForwZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

            transform.position += transform.forward * mySpeed * Time.deltaTime;


            if (destrForwZ && Mathf.Abs(transform.forward.z) > 0.1f)
            {
                Destroy(gameObject);
                return;
            }
            
        lifeTime -= Time.deltaTime;
        
        if(lifeTime < 0f) Destroy(gameObject);

    }
}
