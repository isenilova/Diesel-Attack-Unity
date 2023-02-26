using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIWinAct : MonoBehaviour
{

    public GameObject myBoss;
    private OneHealth bossScr;

    public float myWinTimer = 3f;
    public bool win = false;

    public GameObject winWimdow;
    
    
    // Start is called before the first frame update
    void Start()
    {
        bossScr = myBoss.GetComponent<OneHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((bossScr.curHealth <= 0f)&&(!win))
        {

            win = true;

            StartCoroutine(myWinFunc());

        }


    }

    IEnumerator myWinFunc()
    {
        while (myWinTimer > 0f)
        {
            myWinTimer -= Time.deltaTime;

            yield return null;


        }


        winWimdow.SetActive(true);
        

        yield return new WaitForSeconds(3f);


        SceneManager.LoadScene("SelectLevel");


    }
    
    
  



}
