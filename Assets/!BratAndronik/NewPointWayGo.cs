using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPointWayGo : MonoBehaviour
{

    public GameObject[] myPoints;

    public float mySpeed = 10f;
    public float delayAtPoint = 3f;
    private int lastPointVisit = 0;

    public float destrTime = 50f;

    private float eps = 0.1f;

    private bool stop = false;

    private float tm = 0f;


    public float StartMyWay = 0f;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartMyWay -= Time.deltaTime;
        
        if(StartMyWay > 0) return;
        

        destrTime -= Time.deltaTime;

        if (destrTime <= 0f)
        {
            Destroy(gameObject);
            return;
            
        }

        if (lastPointVisit >= myPoints.Length) return;

        if (stop) return;


        transform.position += (myPoints[lastPointVisit].transform.position - transform.position) /
                              (myPoints[lastPointVisit].transform.position - transform.position).magnitude *
                              Time.deltaTime * mySpeed;



        if ((myPoints[lastPointVisit].transform.position - transform.position).magnitude < eps)
        {
            stop = true;
            
            StartCoroutine(GoStop());
        }
    }



    IEnumerator GoStop()
    {
        while (tm < delayAtPoint)
        {

            tm += Time.deltaTime;

            yield return null;

        }

        tm = 0f;
        stop = false;
        lastPointVisit++;

        yield return null;
    }
}
