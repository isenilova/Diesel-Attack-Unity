using BezierSolution;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezSpawner : MonoBehaviour
{
    private float epsilonSpaum = 0.5f;
    

    public BezierWalkerWithSpeed.TravelMode travelMode;

    public enum ShootType
    {
        none,
        forward,
        straightToPlayer,
        homing,
        hideUnhide,
        coneSynchroCircle,
        coneRandom,
        coneZigzag,
        coneZigzagLine,
        coneSynchroLine,


    }

    [Header("-------------Spawning params-------------")]
    public string tg = "spline1";

    public float speed = 10;
    public int amount = 5;
    public float delay = 1;

    bool triggered = false;

    public GameObject enemy;
    public GameObject spline;


    public bool useTrigger = false;
    public bool useTime = true;
    public float timeActivate = 7.0f;

    public bool useLook = false;

    [Header("------------Shooting params-------------")]
    public int shootNumber = 7;
    public float shootDelay = 1.0f;
    public GameObject projectile;

    public ShootType shootType = ShootType.none;

    public float projRotSpeed = 10.0f;
    public float projSpeed = 100.0f;
    public float projSpeedY = 0;


    public float initDelay = 0;
    public bool useMany = false;
    public int manyCnt = 3;
    public float pauseTime = 5.0f;

    int[] shtPerc = new int[1];


    private bool CRACH = false;

    private void Start()
    {
        shtPerc = new int[amount];

        for (int i = 0; i < shootNumber; i++)
        {
            if (i >= amount) break;
            shtPerc[i] = 1;
        }
        //do random swap
        for (int i = 0; i < amount; i++)
        {
            int t = shtPerc[i];
            int u = Random.Range(0, amount);
            shtPerc[i] = shtPerc[u];
            shtPerc[u] = t;
        }


        if (spline == null)
        {

            var gos = GameObject.FindGameObjectsWithTag(tg);

            if (gos.Length == 0)
            {
                CRACH = true;
                Debug.Log("SPLINE TAG"+tg+"NOT FOUNND");
                
                return;

            }

            spline = gos[0];

            float min = 1e+10f;

            for (int i = 0; i < gos.Length; i++)
            {
                var dst = (gos[i].transform.position - transform.position).magnitude;
                if (dst < min)
                {
                    min = dst;
                    spline = gos[i];
                }
            }

        }

    }


    public void Triggered()
    {
        if (!useTrigger) return;
        if (triggered) return;

        triggered = true;

        for (float i = 0; i < amount; i+=1)
        {
            StartCoroutine(DoSpawn(i));
        }
    }

    public void Activated()
    {
        if (!useTime) return;
        if (triggered) return;

        triggered = true;

        for (float i = 0; i < amount; i += 1)
        {
            StartCoroutine(DoSpawn(i));
        }
    }


    public IEnumerator DoSpawn(float i)
    {
        yield return new WaitForSeconds(i * delay);

        Debug.Log("Spawned");
        var go = (GameObject)Instantiate(enemy);
        go.transform.position = transform.position;
        Debug.Log(go.name);

        if (go.GetComponent<BezierWalkerWithSpeed>() == null) go.AddComponent<BezierWalkerWithSpeed>();

        go.name += name;
        go.GetComponent<BezierWalkerWithSpeed>().spline = spline.GetComponent<BezierSpline>();
        go.GetComponent<BezierWalkerWithSpeed>().speed = speed;
        go.GetComponent<BezierWalkerWithSpeed>().travelMode = travelMode;

        if (go.GetComponent<OneOrientation>() != null)
        {
            go.GetComponent<BezierWalkerWithSpeed>().orientation = go.GetComponent<OneOrientation>().orientation;
        }
        //we change the direction if needed
        
        if (travelMode == BezierWalkerWithSpeed.TravelMode.Once)
        {
            go.GetComponent<BezierWalkerWithSpeed>().onPathCompleted.AddListener(() =>
            {
                var t = go.GetComponentInChildren<OneDeath>();
                if (t != null) t.done = true;
                Destroy(go);
            });
        }


        if (!useLook)
        {
            go.GetComponent<BezierWalkerWithSpeed>().lookForward = false;
        }

        if (shtPerc[(int)i] == 1 && shootType != ShootType.none)
        {


            if (go.GetComponent<AllShoot>() == null)
            {
                go.AddComponent<AllShoot>();
            }

            go.GetComponent<AllShoot>().shootDelay = shootDelay;
            go.GetComponent<AllShoot>().projectile = projectile;
            go.GetComponent<AllShoot>().shootType = shootType;

            go.GetComponent<AllShoot>().projRotSpeed = projRotSpeed;
            go.GetComponent<AllShoot>().projSpeed = projSpeed;
            go.GetComponent<AllShoot>().projSpeedY = projSpeedY;


            go.GetComponent<AllShoot>().initDelay = initDelay;
            go.GetComponent<AllShoot>().useMany = useMany;
            go.GetComponent<AllShoot>().manyCnt = manyCnt;
            go.GetComponent<AllShoot>().pauseTime = pauseTime;

        }

    }


    private void Update()
    
    {
        if(CRACH) return;
        
        if (TimeController.instance.tm > timeActivate && !triggered && useTime && (TimeController.instance.tm < timeActivate + epsilonSpaum) )
        {
            Debug.Log("triggered~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Activated();
        }
    }
}
