using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUISelectLoadScene : MonoBehaviour
{
    public string SceneName = "Level_1_1";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void onMyClick()
    {

    if(SceneName == "") return;

        if (Time.timeScale <= 0.1f) Time.timeScale = 1;
    
        //destroy undestroyed
        var f = FindObjectOfType<DatabaseAll>();
        if (f != null)
        {
            Destroy(f.gameObject);
        }
        var f1 = FindObjectOfType<Savero>();
        if (f1 != null)
        {
            Destroy(f1.gameObject);
        }        
        
        var f2 = FindObjectOfType<GameManager>();
        if (f2 != null)
        {
            Destroy(f2.gameObject);
        }

        if (TimeController.instance != null)
        {
            TimeController.instance.tm = 0;
        }
        
        DoRestart.curTime = 0;

        var g = FindObjectOfType<LifePlayerControl>();
        if (g!=null) g.MaxCurLife();
    SceneManager.LoadScene(SceneName);


    }

}
