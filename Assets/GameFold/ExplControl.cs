using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Exploder;

public class ExplControl : MonoBehaviour {

    public GameObject[] DestroyableObjects;
    public ExploderObject Exploder;

    public GameObject skel;

    public static ExplControl instance;

    private void Awake()
    {
        instance = this;
    }

    public  void ExplodeObject(GameObject obj)
    {
        // activate exploder
        ExploderUtils.SetActive(Exploder.gameObject, true);

        // move exploder object to the same position
        Exploder.transform.position = ExploderUtils.GetCentroid(obj);

        // decrease the radius so the exploder is not interfering other objects
        Exploder.Radius = 1.0f;

        // DONE!
#if ENABLE_CRACK_AND_EXPLODE
        Exploder.Crack(OnCracked);
#else
        Exploder.Explode(OnExplosion);

#endif
    }

    private void OnExplosion(float time, ExploderObject.ExplosionState state)
    {
        if (state == ExploderObject.ExplosionState.ExplosionFinished)
        {
            //Utils.Log("Exploded");
        }
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown("q"))
        {
            /*
            Debug.Log("---------------------------------------------------");
            Debug.Log("should explode");
            skel.transform.SetParent(null);
            skel.tag = "Exploder";
            ExplodeObject(skel);
            */
        }
		
	}
}
