using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllShoot : MonoBehaviour {

    [Header("------------Shooting params-------------")]
    public float shootDelay = 1.0f;
    public GameObject projectile;

    public BezSpawner.ShootType shootType;

    public float projRotSpeed = 10.0f;
    public float projSpeed = 100.0f;
    public float projSpeedY = 0;

    float t = 0;

    public float initDelay = 0;
    public bool useMany = false;
    public int manyCnt = 3;
    public float pauseTime = 5.0f;

    int curCnt = 0;
    bool doneDelay = false;
    bool reqPause = false;
    bool onceShoot = false;

    public Transform laserPoint;

    [Header("-----------------Conus params---------------")]
    public int numSame = 2;
    public float angle;
    float curAngle;
    float dir = 1;

    [Header("------------------Effect params--------------")]
    public DoDamage[] efs;
    public GameObject particle;
    public bool useFrw = false;
    public float sdvig = 0.5f;
    public float fadeTime = 1.0f;

    public bool usePrefire = false;
    public float prefireTime = 0.5f;
    public GameObject prefirePr;

    public bool useOrient = false;

    // Use this for initialization
    void Start ()
    {

        curAngle = -angle;

	}
	
    public void DoShoot()
    {
        if (onceShoot && shootType == BezSpawner.ShootType.hideUnhide)
        {
            return;
        }

        onceShoot = true;

        if (efs != null)
        {
            for (int i = 0; i < efs.Length; i++)
            {
                efs[i].Do(5);
            }
        }

        if (particle != null)
        {
            var fg = (GameObject)Instantiate(particle);
            fg.transform.position = transform.position;

            if (useFrw)
            {
                fg.transform.SetParent(transform);
                fg.transform.localPosition = new Vector3(0,0,sdvig);
                fg.transform.rotation = transform.rotation;
                fg.transform.Rotate(0,-90,0,Space.Self);
            }
            else
            {
                Destroy(fg, fadeTime);
            }
        }


        if (shootType == BezSpawner.ShootType.coneRandom)
        {
            var go = (GameObject)Instantiate(projectile);
            go.transform.position = transform.position;

            /*
            if (go.GetComponent<fall>() != null)
            {
                Destroy(go.GetComponent<fall>());
            }
            */
            float a1 = Random.Range(-angle, angle);
            a1 = a1 * Mathf.PI / 180;
            Vector3 pnt = transform.up;
            Debug.Log(pnt);
            float nx = pnt.x * Mathf.Cos(a1) - pnt.y * Mathf.Sin(a1);
            float ny = pnt.x * Mathf.Sin(a1) + pnt.y * Mathf.Cos(a1);

            go.AddComponent<fall>();
            go.GetComponent<fall>().sx = nx * projSpeed;
            go.GetComponent<fall>().sy = ny * projSpeed;


            return;
        }

        if (shootType == BezSpawner.ShootType.coneSynchroCircle)
        {
            curAngle = -angle;

            for (int i = 0; i < numSame; i++)
            {
                var go = (GameObject)Instantiate(projectile);
                go.transform.position = transform.position;

                /*
                if (go.GetComponent<fall>() != null)
                {
                    Destroy(go.GetComponent<fall>());
                }
                */
                float a1 = curAngle;
                a1 = a1 * Mathf.PI / 180;
                Vector3 pnt = transform.up;
                //Debug.Log(pnt);
                float nx = pnt.x * Mathf.Cos(a1) - pnt.y * Mathf.Sin(a1);
                float ny = pnt.x * Mathf.Sin(a1) + pnt.y * Mathf.Cos(a1);

                go.AddComponent<fall>();
                go.GetComponent<fall>().sx = nx * projSpeed;
                go.GetComponent<fall>().sy = ny * projSpeed;

                curAngle += 2 * angle / (numSame - 1);

            }


            return;
        }

        if (shootType == BezSpawner.ShootType.coneSynchroLine)
        {
            curAngle = -angle;

            for (int i = 0; i < numSame; i++)
            {
                var go = (GameObject)Instantiate(projectile);
                go.transform.position = transform.position;

                /*
                if (go.GetComponent<fall>() != null)
                {
                    Destroy(go.GetComponent<fall>());
                }
                */
                float a1 = curAngle;
                a1 = a1 * Mathf.PI / 180;
                Vector3 pnt = transform.up;
                //Debug.Log(pnt);
                float nx = pnt.x * Mathf.Cos(a1) - pnt.y * Mathf.Sin(a1);
                float ny = pnt.x * Mathf.Sin(a1) + pnt.y * Mathf.Cos(a1);

                go.AddComponent<fall>();
                go.GetComponent<fall>().sx = nx * projSpeed / Mathf.Cos(Mathf.Abs(a1));
                go.GetComponent<fall>().sy = ny * projSpeed / Mathf.Cos(Mathf.Abs(a1));

                curAngle += 2 * angle / (numSame - 1);

            }


            return;
        }

        if (shootType == BezSpawner.ShootType.coneZigzag)
        {
            var go = (GameObject)Instantiate(projectile);
            go.transform.position = transform.position;

            /*
            if (go.GetComponent<fall>() != null)
            {
                Destroy(go.GetComponent<fall>());
            }
            */
            float a1 = curAngle;
            a1 = a1 * Mathf.PI / 180;
            Vector3 pnt = transform.up;
            Debug.Log(pnt);
            float nx = pnt.x * Mathf.Cos(a1) - pnt.y * Mathf.Sin(a1);
            float ny = pnt.x * Mathf.Sin(a1) + pnt.y * Mathf.Cos(a1);

            go.AddComponent<fall>();
            go.GetComponent<fall>().sx = nx * projSpeed;
            go.GetComponent<fall>().sy = ny * projSpeed;

            float dx = 2 * angle / (numSame - 1);

            if (curAngle + dx*dir > angle)
            {
                dir *= -1;
            }

            if (curAngle + dx * dir < -angle)
            {
                dir *= -1;
            }

            curAngle += dx * dir;



            return;
        }

        if (shootType == BezSpawner.ShootType.coneZigzagLine)
        {
            var go = (GameObject)Instantiate(projectile);
            go.transform.position = transform.position;

            /*
            if (go.GetComponent<fall>() != null)
            {
                Destroy(go.GetComponent<fall>());
            }
            */
            float a1 = curAngle;
            a1 = a1 * Mathf.PI / 180;
            Vector3 pnt = transform.up;
            Debug.Log(pnt);
            float nx = pnt.x * Mathf.Cos(a1) - pnt.y * Mathf.Sin(a1);
            float ny = pnt.x * Mathf.Sin(a1) + pnt.y * Mathf.Cos(a1);

            go.AddComponent<fall>();
            go.GetComponent<fall>().sx = nx * projSpeed / Mathf.Cos(Mathf.Abs(a1));
            go.GetComponent<fall>().sy = ny * projSpeed / Mathf.Cos(Mathf.Abs(a1));

            float dx = 2 * angle / (numSame - 1);

            if (curAngle + dx * dir > angle)
            {
                dir *= -1;
            }

            if (curAngle + dx * dir < -angle)
            {
                dir *= -1;
            }

            curAngle += dx * dir;



            return;
        }

        if (shootType == BezSpawner.ShootType.none)
        {
            return;
        }

        if (shootType == BezSpawner.ShootType.homing)
        {
            if (projectile == null) return;
            var go = (GameObject)Instantiate(projectile);
            go.transform.position = transform.position;

            if (go.GetComponent<fall>() != null)
            {
                Destroy(go.GetComponent<fall>());
            }

            //we add trg to projectile
            go.AddComponent<SmoothFollow>();
            go.GetComponent<SmoothFollow>().rotSpeed = projRotSpeed;
            go.GetComponent<SmoothFollow>().speed = projSpeed;

            var go1 = new GameObject();
            var gp = GameObject.FindGameObjectWithTag("Player");
           if(gp == null) return; 
               var vec = (-go.transform.position + gp.transform.position).normalized;

            go1.tag = "targ";
            go1.transform.position = go.transform.position + vec;
            go1.transform.SetParent(go.transform);
            go.GetComponent<SmoothFollow>().forw = go1.transform;

            return;
        }

        if (shootType == BezSpawner.ShootType.forward)
        {
            var go = (GameObject)Instantiate(projectile);
            go.transform.position = transform.position;

            /*
            if (go.GetComponent<fall>() != null)
            {
                Destroy(go.GetComponent<fall>());
            }
            */

            go.AddComponent<fall>();
            go.GetComponent<fall>().sx = -projSpeed;
            go.GetComponent<fall>().sy = -projSpeedY;
            return;
        }

        if (shootType == BezSpawner.ShootType.hideUnhide)
        {
            if (usePrefire)
            {
                var go = (GameObject)Instantiate(prefirePr);
                go.transform.SetParent(laserPoint);
                go.transform.localPosition = Vector3.zero;
                Invoke("ActFire", prefireTime);
            }
            else
            {
                 var go = (GameObject)Instantiate(projectile);
                 go.transform.SetParent(laserPoint);
                 go.transform.localPosition = Vector3.zero;
                if (useOrient)
                {
                    go.transform.localRotation = Quaternion.identity;
                }
            }


            /*
            if (go.GetComponent<fall>() != null)
            {
                Destroy(go.GetComponent<fall>());
            }
            */

            return;
        }

        if (shootType == BezSpawner.ShootType.straightToPlayer)
        {
            var go = (GameObject)Instantiate(projectile);
            go.transform.position = transform.position;

            /*
            if (go.GetComponent<fall>() != null)
            {
                Destroy(go.GetComponent<fall>());
            }
            */

            var gp = GameObject.FindGameObjectWithTag("Player");
            if (gp == null) return;
            var vec = (-go.transform.position + gp.transform.position).normalized;

            go.AddComponent<fall>();
            go.GetComponent<fall>().sx = vec.x * projSpeed;
            go.GetComponent<fall>().sy = vec.y * projSpeed;
            return;
        }
    }

    public void ActFire()
    {
        Destroy(laserPoint.GetChild(0).gameObject);
        
        var go = (GameObject)Instantiate(projectile);
        go.transform.SetParent(laserPoint);
        go.transform.localPosition = Vector3.zero;  
    }

    private void OnDisable()
    {
        if (laserPoint != null && laserPoint.childCount > 0)
        {
            Destroy(laserPoint.GetChild(0).gameObject);
        }

        reqPause = false;
        t = 0;
        doneDelay = false;
        curCnt = 0;
        onceShoot = false;
        curAngle = -angle;
        dir = 1;
    }

    // Update is called once per frame
    void Update () {

        t += Time.deltaTime;

        if (t > initDelay && !doneDelay)
        {
            doneDelay = true;
            t = 0;
        }

        if (!doneDelay)
        {
            return;
        }

        if (reqPause && t > pauseTime)
        {
            t -= pauseTime;
            reqPause = false;
        }

        if (reqPause)
        {
            return;
        }

        if (t > shootDelay)
        {
            t -= shootDelay;
            DoShoot();
            curCnt++;

            if (useMany && curCnt >= manyCnt)
            {
                curCnt = 0;
                reqPause = true;
            }
        }
	}
}
