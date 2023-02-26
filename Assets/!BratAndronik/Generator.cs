using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public int maxProgNum = 5;
    private int curNum = 0;

    public float timeStart = 1f;
    public float timeDelta = 2f;
    private float tm = 0;

    public float speedArrow = 40f;
    public float lifeArrow = 20f;

    public GameObject myArrow;
    private GameObject curArrow;
    private MoveForward arScr;


    public bool SaveRotProj = false;
    public bool useZZ = false;
    
    // Start is called before the first frame update
    void Start()
    {
        tm = timeDelta;

    }

    // Update is called once per frame
    void Update()
    {
        timeStart -= Time.deltaTime;
        
        if(timeStart > 0f) return;
        
        if(curNum >= maxProgNum) return;

        tm += Time.deltaTime;

        if (tm >= timeDelta)
        {
            tm = 0f;

            curNum++;
            
            mySpawn();


        }

    }



    void mySpawn()
    {
        if (myArrow != null)
        {

            if (useZZ)
            {
                curArrow = Instantiate(myArrow);
                curArrow.transform.position = transform.position;
            }
            if (!SaveRotProj)
            {
                curArrow = Instantiate(myArrow, transform.position, transform.rotation);
            }
            else
            {
                curArrow = Instantiate(myArrow);
                curArrow.transform.position = transform.position;
                curArrow.transform.forward = transform.forward;
            }

            
            arScr = curArrow.GetComponent<MoveForward>();

            if (arScr != null)
            {
                arScr.lifeTime = lifeArrow;
                arScr.mySpeed = speedArrow;
            }

        }



    }
}
