using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RotatePong : MonoBehaviour 
{
    public Vector3 m_RotationSpeed;

    public Space space = Space.World;

    public float minZ = -10;
    public float maxZ = 50;

    public float iniDir = 1;
    private float angle = 0;

    private void Update()
    {

        if (angle + iniDir * Time.deltaTime * m_RotationSpeed.z > maxZ || angle + iniDir * Time.deltaTime * m_RotationSpeed.z < minZ)
        {
            iniDir *= -1;
        }

        angle += iniDir * Time.deltaTime * m_RotationSpeed.z;
        
        transform.Rotate(iniDir * m_RotationSpeed * Time.deltaTime, space);


    }
}
