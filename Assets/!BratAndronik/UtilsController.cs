using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

using UnityEngine.AI;
using UnityEngine.UI;

public class UtilsController : MonoBehaviour
{

    public static UtilsController instance;

    private void Awake()
    {
        instance = this;
    }
    
    
    //Two non-parallel lines which may or may not touch each other have a point on each line which are closest
    //to each other. This function finds those two points. If the lines are not parallel, the function 
    //outputs true, otherwise false.
    public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){
 
        closestPointLine1 = Vector3.zero;
        closestPointLine2 = Vector3.zero;
 
        float a = Vector3.Dot(lineVec1, lineVec1);
        float b = Vector3.Dot(lineVec1, lineVec2);
        float e = Vector3.Dot(lineVec2, lineVec2);
 
        float d = a*e - b*b;
 
        //lines are not parallel
        if(d != 0.0f){
 
            Vector3 r = linePoint1 - linePoint2;
            float c = Vector3.Dot(lineVec1, r);
            float f = Vector3.Dot(lineVec2, r);
 
            float s = (b*f - c*e) / d;
            float t = (a*f - c*b) / d;
 
            closestPointLine1 = linePoint1 + lineVec1 * s;
            closestPointLine2 = linePoint2 + lineVec2 * t;
 
            return true;
        }
 
        else{
            return false;
        }
    }
    
    //Convert a plane defined by 3 points to a plane defined by a vector and a point. 
    //The plane point is the middle of the triangle defined by the 3 points.
    public static void PlaneFrom3Points(out Vector3 planeNormal, out Vector3 planePoint, Vector3 pointA, Vector3 pointB, Vector3 pointC){
 
        planeNormal = Vector3.zero;
        planePoint = Vector3.zero;
 
        //Make two vectors from the 3 input points, originating from point A
        Vector3 AB = pointB - pointA;
        Vector3 AC = pointC - pointA;
 
        //Calculate the normal
        planeNormal = Vector3.Normalize(Vector3.Cross(AB, AC));
 
        //Get the points in the middle AB and AC
        Vector3 middleAB = pointA + (AB / 2.0f);
        Vector3 middleAC = pointA + (AC / 2.0f);
 
        //Get vectors from the middle of AB and AC to the point which is not on that line.
        Vector3 middleABtoC = pointC - middleAB;
        Vector3 middleACtoB = pointB - middleAC;
 
        //Calculate the intersection between the two lines. This will be the center 
        //of the triangle defined by the 3 points.
        //We could use LineLineIntersection instead of ClosestPointsOnTwoLines but due to rounding errors 
        //this sometimes doesn't work.
        Vector3 temp;
        ClosestPointsOnTwoLines(out planePoint, out temp, middleAB, middleABtoC, middleAC, middleACtoB);
    }
    
    public static float SignedDistancePlanePoint(Vector3 planeNormal, Vector3 planePoint, Vector3 point){
 
        return Vector3.Dot(planeNormal, (point - planePoint));
    }
    //create a vector of direction "vector" with length "size"
    public static Vector3 SetVectorLength(Vector3 vector, float size){
 
        //normalize the vector
        Vector3 vectorNormalized = Vector3.Normalize(vector);
 
        //scale the vector
        return vectorNormalized *= size;
    }
    
    //This function returns a point which is a projection from a point to a plane.
    public static Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point){
 
        float distance;
        Vector3 translationVector;
 
        //First calculate the distance from the point to the plane:
        distance = SignedDistancePlanePoint(planeNormal, planePoint, point);
 
        //Reverse the sign of the distance
        distance *= -1;
 
        //Get a translation vector
        translationVector = SetVectorLength(planeNormal, distance);
 
        //Translate the point to form a projection
        return point + translationVector;
    }


    public void GetMeRelCoords(RectTransform tr1, RectTransform uno, out Vector4 relCoords)
    {
        Vector3[] arv1 = new Vector3[4];
        tr1.GetWorldCorners(arv1);
        Debug.Log(tr1.name);
        
        //
        
        float yl = arv1[1].y - arv1[0].y;
        float xl = arv1[2].x - arv1[1].x;

        Vector3 mainNormal = Vector3.zero;
        Vector3 planePoint = Vector3.zero;
        UtilsController.PlaneFrom3Points(out mainNormal, out planePoint, arv1[0], arv1[1], arv1[2]);
        
        //
        var tr = uno;
        Vector3[] arv = new Vector3[4];
        tr.GetWorldCorners(arv);

        Debug.Log(tr.name);
        Debug.Log(tr.localEulerAngles);
        //
        
        relCoords = Vector4.zero;
        
        for (int j = 0; j < arv.Length; j++)
        {
            var vec1 = UtilsController.ProjectPointOnPlane(mainNormal, planePoint, arv[j]);
                
            //0 - vec1 to anhor min
            //3 - vec1 to anchormax

            if (j == 0)
            {
                var e1 = new Vector2((vec1.x - arv1[0].x)/xl, (vec1.y - arv1[0].y)/yl);
                //Debug.Log(e1);
                relCoords.x = e1.x;
                relCoords.y = e1.y;
            }

            if (j == 2)
            {
                var e2 = new Vector2((vec1.x - arv1[0].x)/xl, (vec1.y - arv1[0].y)/yl);
                //Debug.Log(e2);
                relCoords.z = e2.x;
                relCoords.w = e2.y;
            }
        }

        
        
    }
    

    public string TimeToTime(float t)
    {
        string s = "";
        int u = (int)(t / 60);

        if (u < 10) s += "0";

        s += u.ToString() + ":";
        int g = (int)(t % 60);
        if (g < 10) s += "0";

        s += g.ToString();

        return s;
    }

    public void FadeNDestroy(GameObject go, float t)
    {
        StartCoroutine(FadeNDestroyA(go, t));
    }

    public IEnumerator FadeNDestroyA(GameObject go, float t)
    {
        float t0 = 0;
        while (t0 < t)
        {
            t0 += Time.deltaTime;
            go.GetComponent<CanvasGroup>().alpha = 1 - t0 / t;
            yield return null;
        }
        
        Destroy(go);
    }

    public void FadeText(Text txt, float tm)
    {
        StartCoroutine(FadeTextA(txt, tm));
    }

    public IEnumerator FadeTextA(Text txt, float tm)
    {
        float t = 0;
        while (t < tm)
        {
            var f = 1 - t / tm;
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, f);
            yield return null;
            t += Time.deltaTime;

        }
    }

    public void FlashyFullA(Text flashy)
    {
        StartCoroutine(FlashyFull(flashy));
    }

    public static bool Checko(string a, string slot, string delim = "@")
    {
        bool q = false;

        var d = slot.Split(new string[] {delim}, StringSplitOptions.None);

        for (int i = 0; i < d.Length; i++)
        {
            if (d[i].ToLower() == a.ToLower()) q = true;
        }
        
        return q;
    }

    public static string GetSlot(string slot, string delim = "@")
    {
        var d = slot.Split(new string[] {delim}, StringSplitOptions.None);

        for (int i = 0; i < d.Length; i++)
        {
            if (d[i].ToLower() == "weapon") return "weapon";           
        }

        return d[0];
    }
            

    public IEnumerator FlashyFull(Text flashy)
    {
        flashy.text = "Inventory is FULL";
        flashy.color = new Color(1, 1, 1, 1);
        float f = 1.0f;
        while (f >= 0)
        {
            f -= 0.01f;
            flashy.color = new Color(1, 1, 1, f);
            yield return null;
        }
        flashy.text = "";
        flashy.color = new Color(1, 1, 1, 1);

        yield return null;
    }

    public void Waito(float t, Action act)
    {
        StartCoroutine(WaitoA(t, act));
    }

    public void StartChaseA(GameObject who, GameObject whom, float spd, float endDist, Action act, int faceForward = 0, int camFol = 0, Transform adFol = null)
    {
        StartCoroutine(StartChase(who, whom, spd, endDist, act, faceForward, camFol, adFol));
    }

    public void StartChaseAT(GameObject who, GameObject whom, float tm, float endDist, Action act, int faceForward = 0, int camFol = 0, Transform adFol = null)
    {
        StartCoroutine(StartChaseT(who, whom, tm, endDist, act, faceForward, camFol, adFol));
    }

    public void StartChaseB(GameObject who, GameObject whom, float spd, float endDist, Action act, int camFol = 0, Transform adFol = null)
    {
        StartCoroutine(StartChaseFol(who, whom, spd, endDist, act, camFol, adFol));
    }

    public void StartWalkA(GameObject who, Vector3 from, Vector3 to, float spd, float tm, Action act)
    {
        StartCoroutine(StartWalk(who, from, to, spd, tm, act));
    }

    public void MoveCamA(Transform from, Transform to, int cnt, Action action, float tm)
    {
        StartCoroutine(MoveCam(from, to, cnt, action, tm));
    }

    public void MoveCamB(float s0, Vector3 from, Vector3 to, Transform watch, int cnt, Action action, float tm)
    {
        StartCoroutine(MoveCamB0(s0, from, to, watch, cnt, action, tm));
    }

    public void ReturnHome(float s0, GameObject who, Vector3 where, float spd, Quaternion endRot, Action act0, Action action)
    {
        StartCoroutine(ReturnHomeA(s0, who, where, spd, endRot, act0, action));
    }


    public void FlyText(Text t, string val, Vector3 pos)
    {
        StartCoroutine(FlyTextA(t, val, pos));

    }

    public IEnumerator WaitoA(float t, Action act)
    {
        yield return new WaitForSeconds(t);

        if (act != null)
        {
            act();
        }
    }

    public IEnumerator FlyTextA(Text t, string val, Vector3 pos)
    {
        t.text = val;
        t.color = new Color(t.color.r, t.color.g, t.color.b, 1);
        int cnt = 50;
        float spd = 0.01f;
        t.transform.position = pos;
        for (int i = 0; i < cnt; i++)
        {
            yield return null;

            t.transform.localPosition += new Vector3(0,spd,0);
            float f = 1 - i / cnt;
            t.color = new Color(t.color.r, t.color.g, t.color.b, f);
        }

        t.text = "";
        t.transform.position = pos;

    }
    

    public IEnumerator ReturnHomeA(float s0, GameObject who, Vector3 where, float spd, Quaternion endRot, Action act0, Action action)
    {
        yield return new WaitForSeconds(s0);

        if (act0 != null)
        {
            act0();
        }

        Vector3 saved = Vector3.zero;


        while (true)
        {
            var vec = where - who.transform.position;
            if (vec.magnitude < 0.1f)
            {
                break;
            }

            who.transform.position += vec.normalized * spd;
            who.transform.forward = vec.normalized;


            yield return null;
        }

        who.transform.rotation = endRot;

        if (action != null)
        {
            action();
        }
    }

    public IEnumerator StartWalk(GameObject who, Vector3 from, Vector3 to, float spd, float tm, Action act)
    {
        if (who.GetComponent<NavMeshAgent>() == null)
        {
            who.AddComponent<NavMeshAgent>();
            who.GetComponent<NavMeshAgent>().baseOffset = 0f;
            who.GetComponent<NavMeshAgent>().radius = 0.28f;
            who.GetComponent<NavMeshAgent>().acceleration = 200;
            who.GetComponent<NavMeshAgent>().angularSpeed = 20000;


        }

        who.GetComponent<NavMeshAgent>().isStopped = false;
        who.GetComponent<NavMeshAgent>().SetDestination(to);
//        who.GetComponentInChildren<Animat>().SetState("walk", 0, 0);

        float t = 0;
        while (t < tm)
        {
            t += Time.deltaTime;
            yield return null;
        }

        if (act != null) act();


    }

    public IEnumerator MoveCam(Transform from, Transform to, int cnt, Action action, float tm)
    {
        yield return null;

        var tr1 = from.position;
        var rt1 = from.rotation;

        var tr2 = to.position + to.forward + new Vector3(0.4f, 1f, 0);
        var rt2 = to.rotation;


        //
        var go = new GameObject();
        go.transform.position = tr2;
        go.transform.rotation = rt2;
        go.transform.Rotate(0, 180, 0);
        go.name = "raudi";

        rt2 = go.transform.rotation;

        float k = 0.001f;

        Vector3 dlt = (tr2 - tr1) / cnt;
        float speed = 0.001f;



        for (float i = 0; i < cnt; i += 1)
        {
            Vector3 pos = tr1 + i * dlt;
            Vector3 nrm = new Vector3(rt2.y - rt1.y, rt1.x - rt2.x, 0);
            nrm.Normalize();

            pos += nrm * i * (i - cnt + 1) * k;

            //float step = speed * Time.deltaTime;
            //Camera.main.transform.rotation = Quaternion.RotateTowards(transform.rotation, rt2, step);

            //Camera.main.transform.forward = er2.transform.position - Camera.main.transform.position;

            Camera.main.transform.forward = to.position + new Vector3(0, 1, 0) - Camera.main.transform.position;

            /*
            var go1 = (GameObject)Instantiate(oneCube);
            go1.transform.position = pos;
            go1.name += i.ToString();
            */

            Camera.main.transform.position = pos;

            yield return null;

        }

        yield return new WaitForSeconds(tm);

        if (action != null)
        {
            action();
        }
        //Camera.main.transform.forward = go.transform.forward;

        //er2.GetComponentInChildren<Animat>().SetState("happy");

    }


    public IEnumerator MoveCamB0(float s0, Vector3 from, Vector3 to, Transform watch, int cnt, Action action, float tm)
    {
        yield return new WaitForSeconds(s0);

        var tr1 = from;
        //var rt1 = from.rotation;

        var tr2 = to;
        //var rt2 = to.rotation;


        //
        /*
        var go = new GameObject();
        go.transform.position = tr2;
        go.transform.rotation = rt2;
        go.transform.Rotate(0, 180, 0);
        go.name = "raudi";

        rt2 = go.transform.rotation;
        */


        float k = 0.001f;

        Vector3 dlt = (tr2 - tr1) / cnt;
        float speed = 0.001f;



        for (float i = 0; i < cnt; i += 1)
        {
            Vector3 pos = tr1 + i * dlt;
            Vector3 nrm = new Vector3(-tr2.y + tr1.y, -tr1.x + tr2.x, 0);
            nrm.Normalize();

            pos += nrm * i * (i - cnt + 1) * k;

            //float step = speed * Time.deltaTime;
            //Camera.main.transform.rotation = Quaternion.RotateTowards(transform.rotation, rt2, step);

            //Camera.main.transform.forward = er2.transform.position - Camera.main.transform.position;

            Camera.main.transform.forward = watch.position - Camera.main.transform.position;

            /*
            var go1 = (GameObject)Instantiate(oneCube);
            go1.transform.position = pos;
            go1.name += i.ToString();
            */

            Camera.main.transform.position = pos;

            yield return null;

        }

        yield return new WaitForSeconds(tm);

        if (action != null)
        {
            action();
        }
        //Camera.main.transform.forward = go.transform.forward;

        //er2.GetComponentInChildren<Animat>().SetState("happy");

    }


    public IEnumerator StartChase(GameObject who, GameObject whom, float spd, float endDist, Action act, int faceForward, int camFol, Transform adFol)
    {
        Vector3 saved = Vector3.zero;

        if (camFol == 1)
        {
            saved = Camera.main.transform.position - who.transform.position;
        }


        while (true)
        {
            var vec = whom.transform.position - who.transform.position;
            if (vec.magnitude < endDist)
            {
                break;
            }

            who.transform.position += vec.normalized * spd;

            if (faceForward == 1)
            {
                who.transform.forward = vec.normalized;
            }

            if (camFol == 1)
            {
                Camera.main.transform.position = who.transform.position + saved;
            }

            if (adFol != null)
            {
                adFol.position = who.transform.position;
            }

            yield return null;
        }

        if (act != null)
        {
            act();
        }

    }


    public IEnumerator StartChaseT(GameObject who, GameObject whom, float tm, float endDist, Action act, int faceForward, int camFol, Transform adFol)
    {
        Vector3 saved = Vector3.zero;

        if (camFol == 1)
        {
            saved = Camera.main.transform.position - who.transform.position;
        }

        float t = 0;

        while (true)
        {
            var vec = whom.transform.position - who.transform.position;
            if (vec.magnitude < endDist)
            {
                break;
            }

            float spd = 1;
            if (tm - t > 0)
            {
                spd = Time.deltaTime * vec.magnitude / (tm - t);
            }
            t += Time.deltaTime;

            who.transform.position += vec.normalized * spd;

            if (faceForward == 1)
            {
                who.transform.forward = vec.normalized;
            }

            if (camFol == 1)
            {
                Camera.main.transform.position = who.transform.position + saved;
            }

            if (adFol != null)
            {
                adFol.position = who.transform.position;
            }

            yield return null;
        }

        if (act != null)
        {
            act();
        }

    }

    public string GetStartPhrase(string filter)
    {
        /*
        foreach (var itm in DatabaseAll.instance.data["Dialogr"].Keys.ToList())
        {
            //ini check
            var rcp1 = (Dialogr) DatabaseAll.instance.data["Dialogr"][itm];

            if (rcp1.filter != filter) continue;

            if (rcp1.startPhrase != "") return rcp1.startPhrase;
        }
        */

        return "Hello";
    }


    public IEnumerator StartChaseFol(GameObject who, GameObject whom, float spd, float endDist, Action act, int camFol, Transform adFol)
    {
        Vector3 saved = Vector3.zero;

        if (who.GetComponent<NavMeshAgent>() == null)
        {
            who.AddComponent<NavMeshAgent>();
            who.GetComponent<NavMeshAgent>().baseOffset = 0f;
            who.GetComponent<NavMeshAgent>().radius = 0.28f;
            who.GetComponent<NavMeshAgent>().acceleration = 200;
            who.GetComponent<NavMeshAgent>().angularSpeed = 20000;


        }

        who.GetComponent<NavMeshAgent>().isStopped = false;
        who.GetComponent<NavMeshAgent>().SetDestination(whom.transform.position);
        //who.GetComponentInChildren<Animat>().SetState("walk", 0, 0);

        if (camFol == 1)
        {
            saved = Camera.main.transform.position - who.transform.position;
        }


        while (true)
        {
            var vec = whom.transform.position - who.transform.position;
            if (vec.magnitude < endDist)
            {
                break;
            }

            //who.transform.position += vec.normalized * spd;
            //who.transform.forward = vec.normalized;

            if (camFol == 1)
            {
                Camera.main.transform.position = who.transform.position + saved;
            }

            if (adFol != null)
            {
                adFol.position = who.transform.position;
            }

            yield return null;
        }

        who.GetComponent<NavMeshAgent>().isStopped = true;
        who.GetComponent<NavMeshAgent>().ResetPath();
        who.transform.forward = new Vector3(whom.transform.position.x - who.transform.position.x, who.transform.forward.y, whom.transform.position.z - who.transform.position.z);

        if (act != null)
        {
            act();
        }
    }


    public void ReceiveDamageX(Damage d, float dt, GameObject who, float kf = 1.0f)
    {
        StartCoroutine(ReceiveDamage(d, dt, who, kf));
    }

    public IEnumerator ReceiveDamage(Damage d, float dt, GameObject who, float kf = 1.0f)
    {
        yield return new WaitForSeconds(dt);

        //who.GetComponent<MMonster>().monster.ReceiveDamage(d, kf);
    }


    public static void GetClosestBone(ref float dst, Transform t, Vector3 what, ref Transform ans)
    {
        
        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).GetComponent<MeshRenderer>() != null) continue;
            if (t.GetChild(i).GetComponent<SkinnedMeshRenderer>() != null) continue;
            
            
            var d = Vector3.Distance(t.GetChild(i).position, what);
            if (d < dst)
            {
                dst = d;
                ans = t.GetChild(i);
            }
            
            GetClosestBone(ref dst, t.GetChild(i), what, ref ans);
        }
    }

    public IEnumerator ProjectileFly(GameObject go, Func<GameObject, bool> check, Action checkFailed, Action end, Vector3 dir, float spd)
    {
        float t = 0;
        float g = 9.81f;
        dir.Normalize();
        var savedDir = dir;
        bool breaked = false;
        //eps
        while (true)
        {
            yield return null;
            go.transform.position += spd * Time.deltaTime * dir;
            dir.y = savedDir.y - t * g;
            if (check(go))
            {
                breaked = true;
                break;
            }
            
            
        }

        if (breaked && checkFailed != null)
        {
            checkFailed();
        }
        
        
    }

}
