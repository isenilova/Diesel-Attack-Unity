using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

public class MoveControl : MonoBehaviour
{


    public float speedX = 10;
    public float speedY = 10;

    [Header("ETO SKOROST")]
    public float addSpeed = 10;
    public float addSpeedy = 0;

    public bool useInp = false;
    public bool useBlock = false;

    public Transform lox;
    public Transform loy;
    public Transform hix;
    public Transform hiy;

    public float bx = 10;
    public float by = 10;
    public float otlip = 0.1f;
    public float raycLenHor = 2;
    public float raycLenVert = 1;

    public float deathDist = 2;
    
    public Vector3 heighto = new Vector3(0, 0.5f, 0);
    public Vector3 widtho = new Vector3(1f, 0, 0);

    public bool visualize = false;
    List<GameObject> prims = new List<GameObject>();
    private int cnt = 15;

    public string horAxis = "Horizontal";
    public string vertAxis = "Vertical";

    public float downRayScale = 0.5f;
    
    void Start()
    {
        if (visualize)
        {
            for (int i = 0; i < cnt; i++)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                prims.Add(go);
            }
        }
    }


    public Transform body;
    private float angle;
    private float rotSpd = 0.5f;
    private float maxAngle = 12;
    
    private float angleEngine;
    private float rotSpdEngine = 0.5f;
    private float maxAngleEngine = 30;
    public Transform turb1;
    public Transform turb2;


    public bool useAcc = false;
    public float maxAdd = 10;
    public float spdLoss = 1.0f;
    public float accSpeed = 0;

    public Vector3 hitSpeed;
    public float decKoef = 1.05f;

    public void DecayHit()
    {
        if (Mathf.Abs(hitSpeed.x) > 0.1f)
        {
            hitSpeed.x /= decKoef;
        }

        if (Mathf.Abs(hitSpeed.y) > 0.1f)
        {
            hitSpeed.y /= decKoef;
        }
        
        
    }
    
    public void Stabilize()
    {
        if (Mathf.Abs(angle) > rotSpd)
        {
            if (angle > 0)
            {
                angle -= rotSpd;
                body.Rotate(0, rotSpd, 0);
            }
            else
            {
                angle += rotSpd;
                body.Rotate(0, -rotSpd, 0);
            }
        }
    }
    
    public void DoRotate(float dy)
    {
        //Debug.Log(dy);

        if (Mathf.Abs(dy) < 0.01f)
        {
            Stabilize();
            return;
        }

        float sgn = Mathf.Sign(dy);
        
        if (angle + sgn * rotSpd < maxAngle && angle + sgn * rotSpd > -maxAngle)
        {
            body.Rotate(0, -sgn * rotSpd, 0);
            angle += sgn * rotSpd;
        }
    }
    
    //engines stabilizing
    
    public void StabilizeEngines()
    {
        if (Mathf.Abs(angleEngine) > rotSpdEngine)
        {
            if (angleEngine > 0)
            {
                angleEngine -= rotSpdEngine;
                turb1.Rotate(0, rotSpdEngine, 0);
                turb2.Rotate(0, rotSpdEngine, 0);
                
            }
            else
            {
                angleEngine += rotSpdEngine;
                turb1.Rotate(0, -rotSpdEngine, 0);
                turb2.Rotate(0, -rotSpdEngine, 0);
            }
        }
    }
    
    public void DoRotateEngines(float dy)
    {
        //Debug.Log(dy);

        if (Mathf.Abs(dy) < 0.01f)
        {
            StabilizeEngines();
            return;
        }

        float sgn = Mathf.Sign(dy);
        
        if (angleEngine + sgn * rotSpdEngine < maxAngleEngine && angleEngine + sgn * rotSpdEngine > -maxAngleEngine)
        {
            turb1.Rotate(0, -sgn * rotSpdEngine, 0);
            turb2.Rotate(0, -sgn * rotSpdEngine, 0);
            angleEngine += sgn * rotSpdEngine;
        }
    }
    
    
    //

    public void CalcAccSpeed(float x, float y)
    {
        if (useAcc)
        {
            float mg = x * x + y * y;
            accSpeed += mg;
            if (accSpeed > maxAdd)
            {
                accSpeed = maxAdd;
            }

            accSpeed /= spdLoss;
        }
    }

    public Vector3 GetSpd()
    {
        float dx = 0;
        float dy = 0;
        if (useInp)
        {
            dx = Input.GetAxis(horAxis);
            //Debug.Log(dx);
            dy = Input.GetAxis(vertAxis);
        }

        var newVec = new Vector3((1 + accSpeed) * dx * speedX,
            (1 + accSpeed) * dy * speedY, 0);
        //speed minus camera
        
        
        return newVec;
    }
    // Update is called once per frame
    
    
    void Update()
    {

        float dx = 0;
        float dy = 0;

        if (useInp)
        {
            dx = Input.GetAxis(horAxis);
            //Debug.Log(dx);
            dy = Input.GetAxis(vertAxis);
            
            DoRotate(dy);
            DoRotateEngines(dx);
        }
        
        CalcAccSpeed(dx, dy);
        DecayHit();

        var newVec = transform.position + new Vector3((1 + accSpeed) * dx * Time.deltaTime * speedX,
                         (1 + accSpeed) * dy * Time.deltaTime * speedY) +
                     new Vector3(addSpeed * Time.deltaTime, addSpeedy * Time.deltaTime, 0) + hitSpeed * Time.deltaTime;

        if (useBlock)
        {
            RaycastHit2D rh;
            int q = 0;
            bool a1 = false;
            for (float i = -1; i < 1.5f; i += 1.0f)
            {
                if (visualize)
                {
                    prims[q].transform.position = transform.position + Vector3.right*raycLenHor + i * heighto;
                    q++;
                }
                
                if (rh = Physics2D.Raycast(transform.position+ i * heighto, Vector3.right , raycLenHor,
                    1 << LayerMask.NameToLayer("block")))
                {
                    a1 = true;
                    //Debug.Log("A");
                    if (dx >= 0)
                    {
                        newVec.x = transform.position.x;
                        newVec.x = rh.point.x - raycLenHor;
                    }
                    else
                    {
                        newVec.x = rh.point.x - raycLenHor - otlip;
                    }

                    if (newVec.x - CamBound.instance.lox.position.x < raycLenHor)
                    {
                        GetComponent<OneHealth>().Terminate();
                    }

                }
            }

            bool a2 = false;
            for (float i = -1; i < 1.5f; i += 1.0f)
            {
                if (visualize)
                {
                    prims[q].transform.position = transform.position - Vector3.right*raycLenHor + i * heighto;
                    q++;
                }
                
                if (rh = Physics2D.Raycast(transform.position+ i * heighto, -Vector3.right , raycLenHor,
                    1 << LayerMask.NameToLayer("block")))
                {
                    a2 = true;
                    //Debug.Log("A");
                    if (dx <= 0)
                    {
                        newVec.x = transform.position.x;
                        newVec.x = rh.point.x + raycLenHor;
                    }
                    else
                    {
                        newVec.x = rh.point.x + raycLenHor + otlip;
                    }


                }
            }

            if (a1 && a2)
            {
                GetComponent<OneHealth>().Terminate();
            }

            //verticals



            for (float i = -1; i < 1.5f; i += 1.0f)
            {
                if (visualize)
                {
                    prims[q].transform.position = transform.position + Vector3.down * raycLenVert + widtho * i;
                    q++;
                }
                
                if (rh = Physics2D.Raycast(transform.position+ widtho * i, Vector3.down , downRayScale * raycLenVert,
                    1 << LayerMask.NameToLayer("block")))
                {
                    //Debug.Log("A");
                    if (dy < 0)
                    {
                        newVec.y = transform.position.y;
                        newVec.y = rh.point.y + downRayScale * raycLenVert;
                    }
                    else
                    {
                        newVec.y = rh.point.y + downRayScale * raycLenVert + otlip;
                    }

                }
            }

            for (float i = -1; i < 1.5f; i += 1.0f)
            {
                if (visualize)
                {
                    prims[q].transform.position = transform.position + Vector3.up * raycLenVert + widtho * i;
                    q++;
                }
                
                if (rh = Physics2D.Raycast(transform.position+ widtho * i, Vector3.up , raycLenVert,
                    1 << LayerMask.NameToLayer("block")))
                {

                    //Debug.Log("A");
                    if (dy >= 0)
                    {
                        newVec.y = transform.position.y;
                        newVec.y = rh.point.y - raycLenVert;
                    }
                    else
                    {
                        newVec.y = rh.point.y - raycLenVert - otlip;
                    }

                }
            }

        }

            if (lox != null && lox.position.x + bx > newVec.x) newVec.x = lox.position.x + bx;
            if (hix != null && hix.position.x - bx < newVec.x) newVec.x = hix.position.x - bx;
            if (loy != null && loy.position.y + by > newVec.y) newVec.y = loy.position.y + by;
            if (hiy != null && hiy.position.y - by < newVec.y) newVec.y = hiy.position.y - by;

            transform.position = newVec;
        
    }

}