using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Done", 4);
    }

    public void Done()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame
}
