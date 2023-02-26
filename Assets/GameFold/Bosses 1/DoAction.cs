using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoAction : MonoBehaviour {

    public Material matChnageShooters;

    public GameObject[] disabledShooters;

    public Material matChange;

    public GameObject decalSpawn;

    public GameObject myExplosion;
    public GameObject theirExplosion;
    public float maxRand = 1.0f;
    public float fadeTime = 0.4f;

    public IEnumerator DoExp(GameObject where, GameObject what)
    {
        yield return new WaitForSeconds(Random.Range(0, maxRand));
        
        var go = (GameObject) Instantiate(what);
        go.transform.position = where.transform.position;
        Destroy(go, fadeTime);
    }
    
    
    public void Do()
    {
        if (myExplosion != null)
        {
            myExplosion.SetActive(true);
        }

        for (int i = 0; i < disabledShooters.Length; i++)
        {
            if (disabledShooters[i] != null)
            {
                StartCoroutine(DoExp(disabledShooters[i], theirExplosion));
            }
        }


        for (int i = 0; i < disabledShooters.Length; i++)
        {
            if (disabledShooters[i].GetComponent<AllShoot>() != null)
            {
                disabledShooters[i].GetComponent<AllShoot>().enabled = false;
                disabledShooters[i].GetComponent<AllShoot>().shootType = BezSpawner.ShootType.none;
            }

            if (matChnageShooters != null)
                {
                disabledShooters[i].GetComponentInParent<MeshRenderer>().material = matChnageShooters;
                }

        }

        if (decalSpawn != null)
        {
            decalSpawn.SetActive(true);
        }

        if (matChange != null)
        {
            GetComponent<MeshRenderer>().material = matChange;
        }
        //change material


    }
}
