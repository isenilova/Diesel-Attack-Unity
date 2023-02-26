using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoystMaker : MonoBehaviour
{
    public bool done;
    private float deltaNum = 4.0f;
    
    public List<GameObject> objs = new List<GameObject>();

    public GameObject defo;
    public GameObject accept;
    public GameObject reject;
    // Start is called before the first frame update
    public Transform container;
    
    public Transform pause;
    public Transform endMenu;
    public Transform optionsMenu;
    
    
    private float lastTmH;
    private float lastTmV;

    private float lastA;

    private bool reqFirst = false;
    private bool reqSame = false;
    Vector3 near = Vector3.zero;
    
    private GameObject _curSelected = null;

    public static JoystMaker instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject curSelected
    {
        get { return _curSelected; }
        set
        {
            if (_curSelected != null && _curSelected.GetComponent<Justhl>() != null)
            {
                HighlighterUI.instance.UnhighlightAll();
            }
            
            Debug.Log("+++++++++++ " + value);
            _curSelected = value;
        }
    }

    public void RecalcAcc()
    {
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            accept = defo;
        }
    }

    public void Doing()
    {
        objs.Clear();
        FindButtons(container);

        if (reqFirst)
        {
            GetNextHorizontal();
        }

        if (reqSame)
        {
            SetClosest();
        }
        
        reqFirst = false;
        reqSame = false;
        near = Vector3.zero;
    }

    public void FindButtons(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            if (!t.GetChild(i).gameObject.activeInHierarchy) continue;

            if (t.GetChild(i).GetComponent<Button>() != null)
            {
                objs.Add(t.GetChild(i).gameObject);
            }
            else if (t.GetChild(i).GetComponent<IPointerClickHandler>() != null)
            {
                objs.Add(t.GetChild(i).gameObject);
            }
            else if (t.GetChild(i).GetComponent<Slider>() != null)
            {
                objs.Add(t.GetChild(i).gameObject);                
            }
            
            FindButtons(t.GetChild(i));
            
        }
    }

    public void SetClosest()
    {
        //near
        var min = 1e+10f;
        GameObject tmp = null;
        foreach (var b in objs)
        {
            if ((b.transform.position - near).magnitude < min)
            {
                tmp = b;
                min = (b.transform.position - near).magnitude;
            }
        }

        curSelected = tmp;
    }

    public void GetNextHorizontal()
    {
        if (curSelected == null)
        {
            if (objs.Count == 0) return;
            var m = objs.Max(x => x.transform.position.y);
            curSelected = objs.Find(x => x.transform.position.y == m).gameObject;
            
            if (curSelected.GetComponent<Justhl>() != null)
            {
                HighlighterUI.instance.HighlightObj(curSelected, true);
                accept = curSelected;
                return;
            }
            else RecalcAcc();
            
            if (curSelected.GetComponent<Button>() != null)
            {
                curSelected.GetComponent<Button>().onClick.Invoke();
            }
            
            if (curSelected.GetComponent<IPointerClickHandler>() != null)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(curSelected, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }

            if (curSelected.GetComponent<Rejoy>())
            {
                Invoker.InvokeDelayed(Doing, 0.1f);
            }
            

        }
        else
        {
            //it possibly can be a slider 
            if (curSelected.GetComponent<Slider>() != null)
            {
                curSelected.GetComponent<Slider>().value += 0.1f;
                return;
            }
            
            int q = -1;

            
            Vector3[] v = new Vector3[4];
            GameObject dWhich = curSelected;
            
            if (curSelected.GetComponent<JoyDir>() != null && curSelected.GetComponent<JoyDir>().dRight != null)
            {
                dWhich = curSelected.GetComponent<JoyDir>().dRight;
                for (int i = 0; i < objs.Count; i++)
                {
                    if (objs[i] == dWhich) q = i;
                }
            }

            if (q < 0)
            {
                dWhich.GetComponent<RectTransform>().GetWorldCorners(v);
                for (float kf = 1; kf < deltaNum; kf += 1.0f)
                {
                    float min = 1e+10f;
                    Vector3 ep = new Vector3(v[3].x + kf * (v[3].x - v[0].x) / 2, (v[3].y + v[2].y) / 2,
                        (v[3].z + v[2].z) / 2);

                    for (int i = 0; i < objs.Count; i++)
                    {
                        if (curSelected == objs[i]) continue;
                        if ((objs[i].transform.position - ep).magnitude < min)
                        {
                            min = (objs[i].transform.position - ep).magnitude;
                            q = i;

                        }
                    }

                    if (q >= 0) break;

                }
            }


            if (q < 0 || curSelected == objs[q])
            {
                
            }
            else
            {
                curSelected = objs[q].gameObject;
                
                if (curSelected.GetComponent<Justhl>() != null)
                {
                    HighlighterUI.instance.HighlightObj(curSelected, true);
                    accept = curSelected;
                    return;
                }
                else RecalcAcc();
                
                if (curSelected.GetComponent<Button>() != null)
                {
                    curSelected.GetComponent<Button>().onClick.Invoke();
                }
            
                if (curSelected.GetComponent<IPointerClickHandler>() != null)
                {
                    ExecuteEvents.Execute<IPointerClickHandler>(curSelected, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                }
                
                if (curSelected.GetComponent<Rejoy>())
                {
                    Invoker.InvokeDelayed(Doing, 0.1f);
                }
            }
        }
    }


    public void GetPrevHorizontal()
    {
        if (curSelected == null)
        {
            if (objs.Count == 0) return;
            var m = objs.Max(x => x.transform.position.y);
            curSelected = objs.Find(x => x.transform.position.y == m).gameObject;

            if (curSelected.GetComponent<Justhl>() != null)
            {
                HighlighterUI.instance.HighlightObj(curSelected, true);
                accept = curSelected;
                return;
            }
            else RecalcAcc();
            
            if (curSelected.GetComponent<Button>() != null)
            {
                curSelected.GetComponent<Button>().onClick.Invoke();
            }
            
            if (curSelected.GetComponent<IPointerClickHandler>() != null)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(curSelected, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }
            
            if (curSelected.GetComponent<Rejoy>())
            {
                Invoker.InvokeDelayed(Doing, 0.1f);
            }
            
        }
        else
        {
            
            //it possibly can be a slider 
            if (curSelected.GetComponent<Slider>() != null)
            {
                curSelected.GetComponent<Slider>().value -= 0.1f;
                return;
            }
            
            int q = -1;

            
            Vector3[] v = new Vector3[4];
            GameObject dWhich = curSelected;
            
            if (curSelected.GetComponent<JoyDir>() != null && curSelected.GetComponent<JoyDir>().dLeft != null)
            {
                dWhich = curSelected.GetComponent<JoyDir>().dLeft;
                for (int i = 0; i < objs.Count; i++)
                {
                    if (objs[i] == dWhich) q = i;
                }
            }

            if (q < 0)
            {
                dWhich.GetComponent<RectTransform>().GetWorldCorners(v);
                for (float kf = 1; kf < deltaNum; kf += 1.0f)
                {
                    float min = 1e+10f;
                    Vector3 ep = new Vector3(v[0].x - kf * (v[3].x - v[0].x) / 2, (v[3].y + v[2].y) / 2,
                        (v[3].z + v[2].z) / 2);

                    for (int i = 0; i < objs.Count; i++)
                    {
                        if (curSelected == objs[i]) continue;
                        if ((objs[i].transform.position - ep).magnitude < min)
                        {
                            min = (objs[i].transform.position - ep).magnitude;
                            q = i;

                        }
                    }

                    if (q >= 0) break;

                }
            }

            if (q < 0 || curSelected == objs[q])
            {
                
            }
            else
            {
                curSelected = objs[q].gameObject;
                
                if (curSelected.GetComponent<Justhl>() != null)
                {
                    HighlighterUI.instance.HighlightObj(curSelected, true);
                    accept = curSelected;
                    return;
                }
                else RecalcAcc();
                
                if (curSelected.GetComponent<Button>() != null)
                {
                    curSelected.GetComponent<Button>().onClick.Invoke();
                }
            
                if (curSelected.GetComponent<IPointerClickHandler>() != null)
                {
                    ExecuteEvents.Execute<IPointerClickHandler>(curSelected, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                }
                
                if (curSelected.GetComponent<Rejoy>())
                {
                    Invoker.InvokeDelayed(Doing, 0.1f);
                }
            }
        }
    }

    public void GetNextVertical()
    {
        if (curSelected == null)
        {
            if (objs.Count == 0) return;
            var m = objs.Max(x => x.transform.position.y);
            curSelected = objs.Find(x => x.transform.position.y == m).gameObject;
            
            if (curSelected.GetComponent<Justhl>() != null)
            {
                HighlighterUI.instance.HighlightObj(curSelected, true);
                accept = curSelected;
                return;
            }
            else RecalcAcc();
            
            if (curSelected.GetComponent<Button>() != null)
            {
                curSelected.GetComponent<Button>().onClick.Invoke();
            }
            
            if (curSelected.GetComponent<IPointerClickHandler>() != null)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(curSelected, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }
            
            if (curSelected.GetComponent<Rejoy>())
            {
                Invoker.InvokeDelayed(Doing, 0.1f);
            }
            
        }
        else
        {
            int q = -1;


            Vector3[] v = new Vector3[4];
            GameObject dWhich = curSelected;
            
            if (curSelected.GetComponent<JoyDir>() != null && curSelected.GetComponent<JoyDir>().dUp != null)
            {
                dWhich = curSelected.GetComponent<JoyDir>().dUp;
                for (int i = 0; i < objs.Count; i++)
                {
                    if (objs[i] == dWhich) q = i;
                }
            }

            if (q < 0)
            {
                dWhich.GetComponent<RectTransform>().GetWorldCorners(v);

                for (float kf = 1; kf < deltaNum; kf += 1.0f)
                {
                    float min = 1e+10f;
                    Vector3 ep = new Vector3((v[3].x + v[0].x) / 2, v[2].y + kf * (v[2].y - v[3].y) / 2,
                        (v[3].z + v[2].z) / 2);

                    for (int i = 0; i < objs.Count; i++)
                    {
                        if (curSelected == objs[i]) continue;
                        if ((objs[i].transform.position - ep).magnitude < min)
                        {
                            min = (objs[i].transform.position - ep).magnitude;
                            q = i;

                        }
                    }

                    if (q >= 0) break;

                }
            }


            if (q < 0 || curSelected == objs[q])
            {
                
            }
            else
            {
                curSelected = objs[q].gameObject;
                
                if (curSelected.GetComponent<Justhl>() != null)
                {
                    HighlighterUI.instance.HighlightObj(curSelected, true);
                    accept = curSelected;
                    return;
                }
                else RecalcAcc();
                
                if (curSelected.GetComponent<Button>() != null)
                {
                    curSelected.GetComponent<Button>().onClick.Invoke();
                }
            
                if (curSelected.GetComponent<IPointerClickHandler>() != null)
                {
                    ExecuteEvents.Execute<IPointerClickHandler>(curSelected, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                }
                
                if (curSelected.GetComponent<Rejoy>())
                {
                    Invoker.InvokeDelayed(Doing, 0.1f);
                }
            }
        }
    }


    public void GetPrevVertical()
    { 
        if (curSelected == null)
        {
            if (objs.Count == 0) return;
            var m = objs.Max(x => x.transform.position.y);
            curSelected = objs.Find(x => x.transform.position.y == m).gameObject;
            
            if (curSelected.GetComponent<Justhl>() != null)
            {
                HighlighterUI.instance.HighlightObj(curSelected, true);
                accept = curSelected;
                return;
            }
            else RecalcAcc();
            
            if (curSelected.GetComponent<Button>() != null)
            {
                curSelected.GetComponent<Button>().onClick.Invoke();
            }
            
            if (curSelected.GetComponent<IPointerClickHandler>() != null)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(curSelected, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }
            
            if (curSelected.GetComponent<Rejoy>())
            {
                Invoker.InvokeDelayed(Doing, 0.1f);
            }
            
        }
        else
        {
            int q = -1;

            
            Vector3[] v = new Vector3[4];
            GameObject dWhich = curSelected;
            
            if (curSelected.GetComponent<JoyDir>() != null && curSelected.GetComponent<JoyDir>().dDown != null)
            {
                dWhich = curSelected.GetComponent<JoyDir>().dDown;
                for (int i = 0; i < objs.Count; i++)
                {
                    if (objs[i] == dWhich) q = i;
                }
            }

            if (q < 0)
            {
                dWhich.GetComponent<RectTransform>().GetWorldCorners(v);

                for (float kf = 1; kf < deltaNum; kf += 1.0f)
                {
                    float min = 1e+10f;
                    Vector3 ep = new Vector3((v[3].x + v[0].x) / 2, v[3].y - kf * (v[2].y - v[3].y) / 2,
                        (v[3].z + v[2].z) / 2);
                    for (int i = 0; i < objs.Count; i++)
                    {
                        if (curSelected == objs[i]) continue;
                        if ((objs[i].transform.position - ep).magnitude < min)
                        {
                            min = (objs[i].transform.position - ep).magnitude;
                            q = i;

                        }
                    }

                    if (q >= 0) break;

                }
            }

            if (q < 0 || curSelected == objs[q])
            {
                
            }
            else
            {
                curSelected = objs[q].gameObject;
                
                if (curSelected.GetComponent<Justhl>() != null)
                {
                    HighlighterUI.instance.HighlightObj(curSelected, true);
                    accept = curSelected;
                    return;
                }
                else RecalcAcc();
                
                if (curSelected.GetComponent<Button>() != null)
                {
                    curSelected.GetComponent<Button>().onClick.Invoke();
                }
            
                if (curSelected.GetComponent<IPointerClickHandler>() != null)
                {
                    ExecuteEvents.Execute<IPointerClickHandler>(curSelected, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                }
                
                if (curSelected.GetComponent<Rejoy>())
                {
                    reqFirst = curSelected.GetComponent<Rejoy>().reqFirst;
                    Invoker.InvokeDelayed(Doing, 0.1f);
                }
            }
        }
    }
    
    public static bool wasJoyst = false;
    
    [ContextMenu("Delete Prefs")]
    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    // Update is called once per frame
    void Update()
    {

        var rt = Input.GetJoystickNames();
        if (rt.Length == 0 || (rt.Length == 1 && rt[0].Length < 2))
        {
            if (wasJoyst)
            {
                var cf = FindObjectOfType<Pauser>();
                if (cf != null && !cf.view.activeInHierarchy)
                {
                    Time.timeScale = 0;
                    cf.view.SetActive(true);
                    wasJoyst = false;
                }
            }
            
            return;
        }

        wasJoyst = true;
        Cursor.visible = false;
        /*
        if (InventoryView.instance != null && InventoryView.instance.isOpen && !done)
        {
            done = true;
            objs.Clear();
            container = InventoryView.instance.inv.transform;
            accept = InventoryView.instance.use;
            reject = InventoryView.instance.trash;
            reqFirst = true;
            Invoke("Doing", 0.1f);
            curSelected = null;
        }*/
        
        if (SceneManager.GetActiveScene().name == "MainMenu" && !done)
        {
            done = true;
            objs.Clear();
            container = GameObject.Find("Canvas").transform;
            accept = GameObject.Find("NewGame");
            reject = GameObject.Find("NewGame");
            reqFirst = true;
            Invoker.InvokeDelayed(Doing, 0.1f);
            curSelected = null;
        }
        
        if (SceneManager.GetActiveScene().name.IndexOf("LoadLevel") >= 0 && !done)
        {
            done = true;
            objs.Clear();
            container = GameObject.Find("Canvas").transform;
            accept = GameObject.Find("continurbut");
            reject = GameObject.Find("continurbut");
            reqFirst = true;
            Invoker.InvokeDelayed(Doing, 0.1f);
            curSelected = null;
        }
        
        if (SceneManager.GetActiveScene().name == "SelectLevel" && !done)
        {
            done = true;
            objs.Clear();
            container = GameObject.Find("Canvas").transform;
            accept = GameObject.Find("Menu");
            reject = GameObject.Find("Menu");
            reqFirst = true;
            Invoker.InvokeDelayed(Doing, 0.1f);
            curSelected = null;
        }
        
        if (SceneManager.GetActiveScene().name == "Shop" && !done)
        {
            done = true;
            objs.Clear();
            container = GameObject.Find("Canvas").transform;
            accept = GameObject.Find("Buy");
            reject = GameObject.Find("Equip");
            reqFirst = true;
            Invoker.InvokeDelayed(Doing, 0.1f);
            curSelected = null;
        }
        
        
        if (pause != null && pause.gameObject.activeSelf && !done)
        {
            done = true;
            objs.Clear();
            container = pause;
            accept = GameObject.Find("Continue");
            reject = GameObject.Find("Continue");
            reqFirst = true;
            Invoker.InvokeDelayed(Doing, 0.1f);
            curSelected = null;
        }
        
        if (optionsMenu != null && optionsMenu.gameObject.activeSelf && !done)
        {
            done = true;
            objs.Clear();
            container = optionsMenu;
            accept = GameObject.Find("Continue");
            reject = GameObject.Find("Continue");
            reqFirst = true;
            Invoker.InvokeDelayed(Doing, 0.1f);
            curSelected = null;
        }
        
        if (endMenu != null && endMenu.gameObject.activeSelf && !done)
        {
            done = true;
            objs.Clear();
            container = endMenu;
            accept = GameObject.Find("Restart");
            reject = GameObject.Find("Restart");
            reqFirst = true;
            Invoker.InvokeDelayed(Doing, 0.1f);
            curSelected = null;
        }
        
        if (Input.GetAxis("D-Pad Vertical") != 0 && Time.unscaledTime > lastTmV + 0.3f)
        {
            lastTmV = Time.unscaledTime;
            Debug.Log(Input.GetAxis("D-Pad Vertical"));
            if (!done) return;

            if (Input.GetAxis("D-Pad Vertical") > 0)
            {
                GetNextVertical();
            }
            else
            {
                GetPrevVertical();                
            }
        }
        
        if (Input.GetAxis("D-Pad Horizontal") != 0 && Time.unscaledTime > lastTmH + 0.3f)
        {
            lastTmH = Time.unscaledTime;
            Debug.Log(Input.GetAxis("D-Pad Horizontal"));
            if (!done) return;
            
            if (Input.GetAxis("D-Pad Horizontal") > 0)
            {
                GetNextHorizontal();
            }
            else
            {
                GetPrevHorizontal();                
            }
        }

        var dt = Input.GetAxis("X");
        var gy = Input.GetKeyDown("joystick button 2");
        
        //Debug.Log(gy);
        if (Input.GetKeyDown("joystick button 2") /*Input.GetAxis("X") != 0*/ && Time.unscaledTime > lastA + 0.3f)
        {
            lastA = Time.unscaledTime;
            Debug.Log(Input.GetAxis("X"));
            if (accept != null && done && accept.activeInHierarchy)
            {
                near = curSelected.transform.position;
                reqSame = true;
                //this is most always rejoy
                if (curSelected.GetComponent<Rejoy>() != null)
                {
                    reqSame = curSelected.GetComponent<Rejoy>().reqSame;
                    reqFirst = curSelected.GetComponent<Rejoy>().reqFirst;
                }
                
                accept.GetComponent<Button>().onClick.Invoke();
                Invoker.InvokeDelayed(Doing, 0.1f);
                if (accept.GetComponent<Undoen>() != null) done = false;
            }
        }
        
        if (Input.GetAxis("B") != 0 && Time.unscaledTime > lastA + 0.3f)
        {
            lastA = Time.unscaledTime;
            Debug.Log(Input.GetAxis("B"));
            if (reject != null && done && reject.activeInHierarchy)
            {
                near = curSelected.transform.position;
                reqSame = true;
                //this is most always rejoy
                if (curSelected.GetComponent<Rejoy>() != null)
                {
                    reqSame = curSelected.GetComponent<Rejoy>().reqSame;
                    reqFirst = curSelected.GetComponent<Rejoy>().reqFirst;
                }
                
                reject.GetComponent<Button>().onClick.Invoke();
                Invoker.InvokeDelayed(Doing, 0.1f);
            }
        }
    }
}
