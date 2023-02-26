using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HighlighterUI : MonoBehaviour {

    public static HighlighterUI instance;

    public bool isHighl = false;
    public GameObject[] hls;
    public GameObject[] slts;


    public Dictionary<string, GameObject> hlT = new Dictionary<string, GameObject>();

    public Dictionary<GameObject, GameObject> hlo = new Dictionary<GameObject, GameObject>();
    
    
    private void Awake()
    {
        instance = this;
    }

    public void UnHighlight()
    {
        if (!isHighl) return;

        isHighl = false;
        for (int i = 0; i < hls.Length; i++)
        {
            hls[i].transform.position = new Vector3(10000, 10000, hls[i].transform.position.z);
        }
    }


    public GameObject ReturnByName(string nm)
    {
        var fg = GameObject.Find(nm);
        return fg;
    }

    public void HLSlots(string slot)
    {
        if (slot == "") return;
        if (isHighl) return;

        isHighl = true;
        //find the slots
        slts = GameObject.FindGameObjectsWithTag("slot");
        int q = 0;
        for (int i = 0; i < slts.Length; i++)
        {
           // if (UtilsController.Checko(slts[i].GetComponent<OneSlot>().slotName,slot))
            {
                Vector4 relo = Vector4.zero;
                UtilsController.instance.GetMeRelCoords(transform.GetComponent<RectTransform>(), slts[i].GetComponent<RectTransform>(), out relo);
                hls[q].transform.position = slts[i].transform.position;
                /*
                hls[q].transform.position = slts[i].transform.position;
                hls[q].transform.GetComponent<RectTransform>().sizeDelta = slts[i].transform.GetComponent<RectTransform>().sizeDelta;
                */
                

                hls[q].GetComponent<RectTransform>().anchorMin = new Vector2(relo.x, relo.y);
                hls[q].GetComponent<RectTransform>().anchorMax = new Vector2(relo.z, relo.w);
                hls[q].GetComponent<RectTransform>().offsetMin = Vector2.zero;
                hls[q].GetComponent<RectTransform>().offsetMax = Vector2.zero;
                
                
                
                
                
                hls[q].GetComponent<TwinkleUI>().StartTwinkle();
                q++;
            }
        }

        //place highlightings on them

        //start highlighting
    }

    public void UnhighlightAll()
    {
        foreach (var st in hlT.Keys.ToList())
        {
            if (hlT[st] != null)
            {
                //Destroy(hlT[st]);
                hlT[st].SetActive(false);
                hlT[st] = null;
            }
        }
        
        foreach (var st in hlo.Keys.ToList())
        {
            if (hlo[st] != null)
            {
                //Destroy(hlT[st]);
                hlo[st].SetActive(false);
                hlo[st] = null;
                hlo.Remove(st);
            }
        }
    }


    public void HighlightByName(string nm, bool val)
    {
        var tr1 = transform.GetComponent<RectTransform>();
        Vector3[] arv1 = new Vector3[4];
        tr1.GetWorldCorners(arv1);
        Debug.Log(tr1.name);
        for (int j = 0; j < arv1.Length; j++)
        {
            Debug.Log(arv1[j]);
        }

        float yl = arv1[1].y - arv1[0].y;
        float xl = arv1[2].x - arv1[1].x;

        Vector3 mainNormal = Vector3.zero;
        Vector3 planePoint = Vector3.zero;
        UtilsController.PlaneFrom3Points(out mainNormal, out planePoint, arv1[0], arv1[1], arv1[2]);
        
        
        if (val)
        {
            var ft = (GameObject)Instantiate(hls[0], hls[0].transform.parent);
            var fg = GameObject.Find(nm);
            Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            Debug.Log(fg);
            
            var tr = fg.GetComponent<RectTransform>();
            Vector3[] arv = new Vector3[4];
            tr.GetWorldCorners(arv);

            Debug.Log(tr.name);
            for (int j = 0; j < arv.Length; j++)
            {
                Debug.Log(arv[j]);
            }
            
            
            //
            for (int j = 0; j < arv.Length; j++)
            {
                var vec1 = UtilsController.ProjectPointOnPlane(mainNormal, planePoint, arv[j]);
                
                //0 - vec1 to anhor min
                //3 - vec1 to anchormax

                if (j == 0)
                {
                    var e1 = new Vector2((vec1.x - arv1[0].x)/xl, (vec1.y - arv1[0].y)/yl);
                    Debug.Log(e1);
                    //tr.anchorMin = new Vector2(vec1.x - arv[j].x, vec1.y - arv[j].y);
                    ft.GetComponent<RectTransform>().anchorMin = new Vector2(e1.x, e1.y);
                }

                if (j == 2)
                {
                    var e2 = new Vector2((vec1.x - arv1[0].x)/xl, (vec1.y - arv1[0].y)/yl);
                    Debug.Log(e2);
                    //tr.anchorMax = new Vector2(vec1.x - arv[j].x, vec1.y - arv[j].y);
                    ft.GetComponent<RectTransform>().anchorMax = new Vector2(e2.x, e2.y);
                }
            }
            ft.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
            ft.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
            
            /*
            ft.transform.position = fg.transform.position;
            //ft.transform.GetComponent<RectTransform>().sizeDelta = fg.transform.GetComponent<RectTransform>().sizeDelta;
            ft.transform.GetComponent<RectTransform>().anchorMin = fg.transform.GetComponent<RectTransform>().anchorMin;
            ft.transform.GetComponent<RectTransform>().anchorMax = fg.transform.GetComponent<RectTransform>().anchorMax;
            ft.transform.GetComponent<RectTransform>().offsetMin = fg.transform.GetComponent<RectTransform>().offsetMin;
            ft.transform.GetComponent<RectTransform>().offsetMax = fg.transform.GetComponent<RectTransform>().offsetMax;
            */
            
            
            ft.GetComponent<TwinkleUI>().StartTwinkle();
            hlT.Add(nm, ft);
        }
        else
        {
            if (hlT.ContainsKey(nm))
            {
                Destroy(hlT[nm]);
                hlT.Remove(nm);
            }
        }
    }
    
    public void HighlightObj(GameObject nm, bool val)
    {
        var tr1 = transform.GetComponent<RectTransform>();
        Vector3[] arv1 = new Vector3[4];
        tr1.GetWorldCorners(arv1);
        Debug.Log(tr1.name);
        for (int j = 0; j < arv1.Length; j++)
        {
            Debug.Log(arv1[j]);
        }

        float yl = arv1[1].y - arv1[0].y;
        float xl = arv1[2].x - arv1[1].x;

        Vector3 mainNormal = Vector3.zero;
        Vector3 planePoint = Vector3.zero;
        UtilsController.PlaneFrom3Points(out mainNormal, out planePoint, arv1[0], arv1[1], arv1[2]);
        
        
        if (val)
        {
            GameObject ft = null;
            if (!hlo.ContainsKey(nm))
            {
                ft = (GameObject) Instantiate(hls[0], hls[0].transform.parent);
            }
            else
            {
                ft = hlo[nm];
            }

            var fg = nm;
            Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            Debug.Log(fg);
            
            var tr = fg.GetComponent<RectTransform>();
            Vector3[] arv = new Vector3[4];
            tr.GetWorldCorners(arv);

            Debug.Log(tr.name);
            for (int j = 0; j < arv.Length; j++)
            {
                Debug.Log(arv[j]);
            }
            
            
            Vector3[] vec1 = new Vector3[4];
            for (int j = 0; j < arv.Length; j++)
            {
                vec1[j] = UtilsController.ProjectPointOnPlane(mainNormal, planePoint, arv[j]);
                
                //0 - vec1 to anhor min
                //3 - vec1 to anchormax
            
                /*
                if (j == 0)
                {
                    var e1 = new Vector2((vec1.x - arv1[0].x)/xl, (vec1.y - arv1[0].y)/yl);
                    //Debug.Log(e1);
                    tr.anchorMin = new Vector2(e1.x, e1.y);
                }
    
                if (j == 2)
                {
                    var e2 = new Vector2((vec1.x - arv1[0].x)/xl, (vec1.y - arv1[0].y)/yl);
                    //Debug.Log(e2);
                    tr.anchorMax = new Vector2(e2.x, e2.y);
                }
                */
            }

        
            //
        
            Vector3 v1 = arv1[1] - arv1[0];
            Vector3 v2 = arv1[3] - arv1[0];
            Vector3 v3 = vec1[0] - arv1[0];

            float t2 = 0;
            float t1 = 0;
            
            t2 = (v3.y * v1.x - v1.y * v3.x) / (v2.y * v1.x - v2.x * v1.y);

            if (v1.x != 0)
            {
                t1 = (v3.x - v2.x * t2) / v1.x;
            }
            else
            {
                t1 = (v3.y - v2.y * t2) / v1.y;
            }

            var e1 = new Vector2(t2, t1);
            //Debug.Log(e1);
            ft.GetComponent<RectTransform>().anchorMin = new Vector2(e1.x, e1.y);
        
            //
        

            v3 = vec1[2] - arv1[0];
            t2 = (v3.y * v1.x - v1.y * v3.x) / (v2.y * v1.x - v2.x * v1.y);

            if (v1.x != 0)
            {
                t1 = (v3.x - v2.x * t2) / v1.x;
            }
            else
            {
                t1 = (v3.y - v2.y * t2) / v1.y;                
            }

            e1 = new Vector2(t2, t1);
            //Debug.Log(e1);
            ft.GetComponent<RectTransform>().anchorMax = new Vector2(e1.x, e1.y);
            
            
            ft.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
            ft.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
            
            /*
            ft.transform.position = fg.transform.position;
            //ft.transform.GetComponent<RectTransform>().sizeDelta = fg.transform.GetComponent<RectTransform>().sizeDelta;
            ft.transform.GetComponent<RectTransform>().anchorMin = fg.transform.GetComponent<RectTransform>().anchorMin;
            ft.transform.GetComponent<RectTransform>().anchorMax = fg.transform.GetComponent<RectTransform>().anchorMax;
            ft.transform.GetComponent<RectTransform>().offsetMin = fg.transform.GetComponent<RectTransform>().offsetMin;
            ft.transform.GetComponent<RectTransform>().offsetMax = fg.transform.GetComponent<RectTransform>().offsetMax;
            */
            
            
            ft.GetComponent<TwinkleUI>().StartTwinkle();
            if (!hlo.ContainsKey(nm))
            {
                hlo.Add(nm, ft);
            }
        }
        else
        {
            if (hlo.ContainsKey(nm))
            {
                Destroy(hlo[nm]);
                hlo.Remove(nm);
            }
        }
    }
}
