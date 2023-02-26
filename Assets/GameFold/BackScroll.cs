using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class BackScroll : MonoBehaviour
{

    public float corBack = 1.0f;


    public bool setnext = true;
    
    public float speed = 1;
    float sz;
    public Transform[] backs;

    public Transform loCam;
	// Use this for initialization
	void Start ()
    {
       
        backs[0] = transform.GetChild(0);
        backs[1] = transform.GetChild(1);

        sz = backs[1].position.x - backs[0].position.x;
	}


    public bool waitExchange = false;
    public bool goingOff = false; 
    public GameObject next;
    public void MakeExcnahge(GameObject who)
    {
        waitExchange = true;
        next = who;
        goingOff = true;
    }

    public void SetToNextScreen(bool gf = true)
    {
        if(!setnext) return;
        
        Debug.Log("NextScreen_______________________________________________");
        
        goingOff = gf;
        float x1 = CamBound.instance.hix.position.x;
        float sum = 0;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            var be = transform.GetChild(i).GetComponent<SpriteRenderer>().bounds.extents;
            transform.GetChild(i).transform.position = new Vector3(x1 + sum +  be.x, transform.GetChild(i).transform.position.y,
                transform.GetChild(i).transform.position.z);

            sum += 2 * be.x;
        }
    }

    public void SetToNext(GameObject back, float x1)
    {
        if (!setnext)
        {
            
           // gameObject.transform.position -= new Vector3(speed*Time.deltaTime, 0, 0);
            
           for (int i = 0; i < backs.Length; i++)
            {
                backs[i].position -= new Vector3(speed*Time.deltaTime, 0, 0);
            }
            
            
            
        return;    
        }

        
        
        
        Debug.Log("NextScreen----------------------------------------------");
        back.SetActive(true);
        float sum = 0;
        //set to origin
        for (int i = 0; i < back.transform.childCount; i++)
        {
            for (int j = 0; j < back.transform.childCount; j++)
            {
                if (back.transform.GetChild(j).GetComponent<OneBack>().num == i)
                {
                    back.transform.GetChild(j).SetSiblingIndex(i);
                    break;
                }
            }
        }
        
        for (int i = 0; i < back.transform.childCount; i++)
        {
            var be = back.transform.GetChild(i).GetComponent<SpriteRenderer>().bounds.extents;
            back.transform.GetChild(i).transform.position = new Vector3(x1 + sum +  be.x, back.transform.GetChild(i).transform.position.y,
                back.transform.GetChild(i).transform.position.z);

            sum += 2 * be.x;
        }
    }
    
    public void SortChildrenByName()
    {

        var ft = backs.ToList();
        ft.Sort( (a, b) => a.position.x.CompareTo(b.position.x));
        
        ft[0].position -= new Vector3(speed*Time.deltaTime, 0, 0);
        for (int i = 1; i < ft.Count; i++)
        {
            ft[i].position = ft[i - 1].position +
                             new Vector3(ft[i - 1].GetComponent<SpriteRenderer>().bounds.extents.x, 0, 0) +
                             new Vector3(ft[i].GetComponent<SpriteRenderer>().bounds.extents.x, 0, 0);
        }
            
        
    } 
	// Update is called once per frame
	void LateUpdate ()
    {

        if(!setnext) return;
        //find min
        float min = 1e+10f;
        Vector3 be = Vector3.zero;
        int q = -1;
        for (int i = 0; i < backs.Length; i++)
        {
            if (backs[i].position.x < min)
            {
                min = backs[i].position.x;
                q = i;
                be = backs[i].GetComponent<SpriteRenderer>().bounds.extents;
                //be.x *= backs[i].localScale.x;
            }
        }

        //find max
        float max = -1e+10f;
        Vector3 be1 = Vector3.zero;
        int q1 = -1;
        for (int i = 0; i < backs.Length; i++)
        {
            if (backs[i].position.x > max)
            {
                max = backs[i].position.x;
                be1 = backs[i].GetComponent<SpriteRenderer>().bounds.extents;
                //be1.x *= backs[i].localScale.x;
                q1 = i;
            }
        }

        if (min + be.x < loCam.position.x)
        {
            if (waitExchange)
            {
                //float dd = loCam.position.x - min - be.x; 
                SetToNext(next, max + be1.x);
                waitExchange = false;
                //next is moved to our last
                //and became active
                //and set wait exchange to false
            }

            if (!goingOff)
            {
                //Debug.Log("NextScreen***************************************************");
                backs[q].transform.position = new Vector3(max + be1.x + be.x - corBack * speed*Time.deltaTime, backs[q].transform.position.y,
                    backs[q].transform.position.z);
            }
        }


        SortChildrenByName();
        /*
        for (int i = 0; i < backs.Length; i++)
        {
            backs[i].position -= new Vector3(speed*Time.deltaTime, 0, 0);
        }
        */

        bool q5 = false;
        if (goingOff)
        {
            if (max + be1.x > loCam.position.x) q5 = true;

            if (!q5)
            {
                goingOff = false;
                gameObject.SetActive(false);
            }
        }
        

        //if all < min and goinf off we disable it 
    }
}
