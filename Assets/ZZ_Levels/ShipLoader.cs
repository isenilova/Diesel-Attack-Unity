using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipLoader : MonoBehaviour
{
    /*
    public SkinnedMeshRenderer sm;

    public Mesh[] meshes;
    public Material[] mats;
    */

    public GameObject other;
    void Start()
    {
    
        //load ship
        //sm.sharedMesh = meshes[PlayerData.player.curShip];
        //sm.material = mats[PlayerData.player.curShip];

        if (PlayerData.player.curShip == 1)
        {
            var go = Instantiate(other);
            go.transform.position = transform.position;
            
            //filling back
            var wc = FindObjectOfType<WinController>();
            if (wc != null) wc.myShip = go;
            
            var wc1 = FindObjectOfType<EndMenu>();
            if (wc1 != null) wc1.myHealth[0] = go.GetComponent<OneHealth>();
            
            var wc2 = FindObjectOfType<GUIShields>();
            if (wc2 != null)
            {
                wc2.myPlayer = go;
                wc2.Start();
            }
            
            var wc3 = FindObjectOfType<GameController>();
            if (wc3 != null) wc3.ship = go.GetComponent<OneHealth>();

            var wc4 = FindObjectsOfType<BuffController>().ToList().Find(x => x.id == "1");
            if (wc4 != null) wc4.ship = go.GetComponent<MoveControl>();
            
            var wc5 = FindObjectOfType<GrayFader>();
            if (wc5 != null) wc5.oh[0] = go.GetComponent<OneHealth>();
            
            var wc6 = FindObjectOfType<PlayerHB>();
            if (wc6 != null) wc6.oh = go.GetComponent<OneHealth>();
            
            var wc7 = FindObjectOfType<DoRestart>();
            if (wc7 != null) wc7.myPlayer = go;
            if (wc7 != null) wc7.playerScr = go.GetComponent<OneHealth>();
            if (wc7 != null)
            {
                //wc7.Start();
                int p = 5;
            }
            
            var wc8 = FindObjectOfType<GUIWeapons>();
            var fr = go.GetComponent<RotControl>();
            if (wc8 != null)
            {
                wc8.second = true;
                wc8.mySlots[0] = fr.sock.gameObject;
                wc8.mySlots[1] = fr.rotators[0].gameObject;
                wc8.mySlots[2] = fr.rotators[1].gameObject;
                wc8.Start();
            }
            
            var wc9 = FindObjectsOfType<WeaponController>().ToList().Find(x => x.id == "1");
            if (wc9 != null)
            {
                wc9.slots[0] = fr.sock;
                wc9.slots[1] = fr.rotators[0];
                wc9.slots[2] = fr.rotators[1];
            }

            var wc10 = GameObject.FindGameObjectWithTag("MainCamera");
            var mc = go.GetComponent<MoveControl>();
            mc.lox = wc10.transform.GetChild(0);
            mc.hix = wc10.transform.GetChild(1);
            mc.loy = wc10.transform.GetChild(2);
            mc.hiy = wc10.transform.GetChild(3);
            


            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    
}
