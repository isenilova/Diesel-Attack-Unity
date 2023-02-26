using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disa : MonoBehaviour
{
    // Start is called before the first frame update
    void OnDisable()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
    }
}
