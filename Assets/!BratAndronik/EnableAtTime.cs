using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAtTime : MonoBehaviour
{
    public GameObject[] enableObj;
    public float myTimer = 10f;
    public float myY = 4f;
    private bool enab = false;


    public float SpeedTime = -1f;

    private bool chSpeed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((TimeController.instance.tm >= myTimer) && (!enab))
        {
            transform.parent.transform.position =  new Vector3(CamBound.instance.lox.transform.position.x, myY, 0);
            
            enab = true;
            
              if(enableObj != null)  for(int i =0; i < enableObj.Length; i++) enableObj[i].SetActive(true);

        }



        if ((SpeedTime >= 0)&& (!chSpeed)&&(TimeController.instance.tm >= SpeedTime))
        {

            chSpeed = true;

            var scr = transform.parent.GetComponent<ChangeSpead>();

                scr.ChSp();
        }
    }
}
