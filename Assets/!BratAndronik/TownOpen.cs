using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownOpen : MonoBehaviour
{

    public GameObject[] TopStick;
    public GameObject TopDoor;

    public GameObject[] BotStick;
    public GameObject BotDoor;

    public float openSpeed = 5f;


    public float downSpeedmin = 1.5f;
    public float downSpeedmax = 3f;


    public GameObject RotBarrel;
    public GameObject[] Barrels;

    private TownBossController controlScr;


    public float partDelayTimer = 3f;
    public GameObject myGun;
    public GameObject myGun2;

    public GameObject myTurbine;
    public GameObject myTurbEffect;


  public GameObject myExplosion;
    public float ExplDelay = 1f;


    public GameObject[] SticksObj;

    private float koef = 80.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        controlScr = gameObject.GetComponent<TownBossController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void stickMove(string to, string what)
    {
        if (to == "open")
        {

            if ((what == "top") || (what == "both")) StartCoroutine("Open");


        }

        if (to == "down")
        {
            for(int i =0; i < 4; i++)
            StartCoroutine(Down(i));

            StartCoroutine(CloseDoor("top"));

            Barrels[1].SetActive(false);
            //StartCoroutine("TurnBarrel");
        }

        if (to == "close")
        {
            if (myExplosion != null)
            {
                Instantiate(myExplosion, Barrels[1].transform.position, Barrels[1].transform.rotation);

            }  
              
            Barrels[1].SetActive(false);
            StartCoroutine("LastClose");
            //StartCoroutine("Close");


        }

        if (to == "turn")
        {
            TurbineOff();
            StartCoroutine("TurnBarrel");
            StartCoroutine("Close");

        }


    }

    IEnumerator LastClose()
    {
        float angel = 0f;
        
        while (angel<90)
        {
            var dlt = openSpeed * Time.deltaTime * koef;
            if (angel + dlt > 90) dlt = 90 - angel + 0.1f;
            
           BotDoor.transform.Rotate(0f, 0f, dlt);
            TopDoor.transform.Rotate(0f, 0f, -dlt);

            angel += dlt;

            yield return null;

        } 


        myGun.SetActive(false);
        myGun2.SetActive(false);

        yield return null;
    }


    IEnumerator Open()
    {
        float alpha = 0f;
        
        
        for (int i = 0; i < SticksObj.Length; i++)
        {

            SticksObj[i].SetActive(true);
        }

        while (alpha < 90f)
        {
            var dlt = openSpeed * Time.deltaTime * koef;
            if (alpha + dlt > 90) dlt = 90 - alpha + 0.1f;
            
            TopDoor.transform.Rotate(0f, 0f, dlt);
            BotDoor.transform.Rotate(0f, 0f, -dlt);
            
            alpha += dlt;
            
            yield return 0;
        }

        alpha = 0f;
        
        
        
        while (alpha < 90f)
        {
            var dlt = openSpeed * Time.deltaTime * koef;
            if (alpha + dlt > 90) dlt = 90 - alpha + 0.1f;
            
            TopStick[0].transform.Rotate(0f, 0f, dlt);
            BotStick[0].transform.Rotate(0f, 0f, -dlt);
            
            alpha += dlt;
            
            yield return 0;
        }

        alpha = 0f;

        for (int i = 1; i < 4; i++)
        {
            
            while (alpha < 180f)
            {
                var dlt = openSpeed * Time.deltaTime * koef;
                if (alpha + dlt > 180) dlt = 180 - alpha + 0.1f;
                
                TopStick[i].transform.Rotate(0f, 0f, dlt);
                BotStick[i].transform.Rotate(0f, 0f, -dlt);

                alpha += dlt;
            
                yield return 0;
            }


            alpha = 0f;

        }

        controlScr.myState = "open";

        yield return 0;
    }

    IEnumerator Down(int k)
    {
        float sp = Random.Range(downSpeedmin, downSpeedmax);
        Debug.Log("spd:"+sp);

        float muTimer = 0f;

        while (true)
        {
            
//            TopStick[k].transform.Translate(0f, sp, 0f);
            TopStick[k].transform.position -= new Vector3(0,sp * Time.deltaTime * koef,0);

            yield return 0;
        }
            
       // Destroy(TopStick[k]);

        yield return 0;
    }

    IEnumerator CloseDoor(string num)
    {
        float angel = 0f;
        float timer = 3f;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            yield return null;

        }
        
        
        
        if (num == "top")
        {

            while (angel<90)
            {
                var dlt = openSpeed * Time.deltaTime * koef;
                if (angel + dlt > 90) dlt = 90 - angel + 0.1f;
                
                TopDoor.transform.Rotate(0f, 0f, -dlt);

                angel += dlt;

                yield return null;

            } 
            
            
        }
        
        
        if (num == "bot")
        {

            while (angel<90)
            {
                var dlt = openSpeed * Time.deltaTime * koef;
                if (angel + dlt > 90) dlt = 90 - angel + 0.1f;
                TopDoor.transform.Rotate(0f, 0f, dlt);

                angel += dlt;

                yield return null;

            } 
            
            
        }



    }


    IEnumerator DoorGun()
    {
        myGun.SetActive(true);
        myGun2.SetActive(true);
        
        
        float angel = 0f;
        float timer = 0f;

        while (timer < partDelayTimer)
        {
            timer += Time.deltaTime;

            yield return null;

        }
        
        
        while (angel<90)
        {
            var dlt = openSpeed * Time.deltaTime * koef;
            if (angel + dlt > 90) dlt = 90 - angel + 0.1f;
            
            BotDoor.transform.Rotate(0f, 0f, -dlt);
            TopDoor.transform.Rotate(0f, 0f, dlt);
            
           // myGun.transform.Rotate(0f, 0f, -openSpeed);

            angel += dlt;

            yield return null;

        } 



        controlScr.myState = "gungo";

        yield return null;
    }


    void TurbineOff()
    {
        myTurbine.SetActive(false);
        myTurbEffect.SetActive(false);
        
        if (myExplosion != null)
        {
            Instantiate(myExplosion, myTurbine.transform.position, myTurbine.transform.rotation);

        }  
        
        
    }

    IEnumerator TurnBarrel()
    {
        if (myExplosion != null)
        {
            Instantiate(myExplosion, Barrels[0].transform.position, Barrels[0].transform.rotation);

        }

        Barrels[0].SetActive(false);
        
        
        float timer = 0f;

        while (timer < ExplDelay)
        {
            timer += Time.deltaTime;

            yield return null;

        }
        float angel = 0;

        while (angel< 180)
        {
            var dlt = openSpeed * Time.deltaTime * koef;
            if (angel + dlt > 180) dlt = 180 - angel + 0.1f;
            RotBarrel.transform.Rotate(0f, 0f, -dlt);

            angel += dlt;

            yield return null;
        }



        yield return null;
    }

   
        IEnumerator Close()
        {
            float alpha = 0f;

           
        
        
        

            

            for (int i = 3; i > 0; i--)
            {
            
                while (alpha < 180f)
                {
                    var dlt = openSpeed * Time.deltaTime * koef;
                    if (alpha + dlt > 180) dlt = 180 - alpha + 0.1f;
                    
                    BotStick[i].transform.Rotate(0f, 0f, dlt);
                    TopStick[i].transform.Rotate(0f, 0f, -dlt);

                    alpha += dlt;
            
                    yield return 0;
                }


                alpha = 0f;

            }
            
            
            while (alpha < 90f)
            {
                var dlt = openSpeed * Time.deltaTime * koef;
                if (alpha + dlt > 90) dlt = 90 - alpha + 0.1f;
                
                BotStick[0].transform.Rotate(0f, 0f, dlt);
                TopStick[0].transform.Rotate(0f, 0f, -dlt);
            
                alpha += dlt;
            
                yield return 0;
            }
            

            alpha = 0f;
            
            while (alpha < 90f)
            {
                var dlt = openSpeed * Time.deltaTime * koef;
                if (alpha + dlt > 90) dlt = 90 - alpha + 0.1f;
                
                BotDoor.transform.Rotate(0f, 0f, dlt);
                TopDoor.transform.Rotate(0f, 0f, -dlt);
            
                alpha += dlt;
            
                yield return 0;
            }


            for (int i = 0; i < SticksObj.Length; i++)
            {

                SticksObj[i].SetActive(false);
            }
            //controlScr.myState = "gun";

            StartCoroutine("DoorGun");
            
            yield return 0;
        }




}
