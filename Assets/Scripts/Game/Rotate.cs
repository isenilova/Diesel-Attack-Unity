using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour 
{
    public Vector3 m_RotationSpeed;

    public Space space = Space.World;

    private void Update()
    {
        Transform targ = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "targ") targ = transform.GetChild(i);
        }

        Vector3 savedPos = Vector3.zero;
        if (targ != null) savedPos = targ.position;

        transform.Rotate(m_RotationSpeed * Time.deltaTime, space);

        if (targ != null)
        {
            targ.position = savedPos;
        }
    }
}
