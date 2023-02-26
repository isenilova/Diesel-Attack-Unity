using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    public GameObject myBoss;

    private OneHealth bossScr;

    public GameObject myShip;

    public float speedShip;

    private MoveControl moveScr;

    private bool isWin = false;

    public string myScene;

    public float LoadTimer = 5f;

    public float WinShipY = 0f;

    public GameObject wino;

    public string lvlNum = "";

    public int UnlockSlot = -1;
    public int UnlockWeapon = -1;

    public int UnlockShip = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        bossScr = myBoss.GetComponent<OneHealth>();

        moveScr = myShip.GetComponent<MoveControl>();


    }

    // Update is called once per frame
    void Update()
    {
        if ((bossScr.curHealth <= 0f) && (!isWin))
        {

            isWin = true;

            moveScr.enabled = false;
            
            
            Pauser.instance.Unlock(UnlockSlot, UnlockWeapon, UnlockShip);


           // if (lvlNum != "") PlayerPrefs.SetString("LastLevel", lvlNum);

            StartCoroutine(shipGoAway());

            StartCoroutine(LoadSc());

            if (wino != null)
            {
                wino.SetActive(true);
            }

        }




    }


    IEnumerator shipGoAway()
    {
        if (myShip.transform.position.y >= WinShipY + 0.05f)
        {

            while (myShip.transform.position.y >= WinShipY)
            {
                myShip.transform.position += new Vector3(0f, -speedShip*Time.deltaTime, 0f);

                yield return null;
            }
            
            
            
        }
        else
        {
            while (myShip.transform.position.y <= WinShipY)
            {
                myShip.transform.position += new Vector3(0f, speedShip*Time.deltaTime, 0f);

                yield return null;
            } 
        }

            myShip.transform.position= new Vector3(myShip.transform.position.x, WinShipY, myShip.transform.position.z);


        while (true)
        {
            
            myShip.transform.position += new Vector3(speedShip*Time.deltaTime, 0f, 0f);
            yield return null;
        }


        yield return null;
    }

    IEnumerator LoadSc()
    {
        while (LoadTimer >= 0f)
        {
            LoadTimer -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(myScene);

        yield return null;
    }


}
