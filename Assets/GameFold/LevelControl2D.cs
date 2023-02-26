using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MoreMountains.CorgiEngine;

public class LevelControl2D : MonoBehaviour {

    public static LevelControl2D instance;
    public bool isLoaded = false;

    public Transform container;
    public Transform enemyCont;
    public GameObject endPoint;
    public GameObject startPoint;
    public GameObject levelManager;
    public GameObject camManager;

    float dx = 1.28f;
    float dy = 1.28f;
    int n;
    int m;
    char[][] mtr;

    public Transform g0;
    public Transform g0x;
    public Transform g0y;

    //parsing values back
    public GameObject innerBlock;
    public GameObject outerBlock;
    public GameObject ladder;
    public GameObject[] enemies;
    public GameObject crate;
    public bool doInst = false;
    public Dictionary<char, GameObject> dictGo = new Dictionary<char, GameObject>();


    private void Awake()
    {
        instance = this;
    }

    public void GetMe(Transform t, char a)
    {
        int x = (int)Mathf.Round((t.position.x - g0.position.x) / dx);
        int y = (int)Mathf.Round((t.position.y - g0.position.y) / dy);

        mtr[y][x] = a;
    }

    public void SetMe(GameObject uno, float x, float y)
    {
        //z  = 10
        Vector3 pos = new Vector3(g0.transform.position.x + dx * x, g0.transform.position.y + dy * y, 10);
        uno.transform.position = pos;
    }


    private void Start()
    {
        dictGo.Add('1', innerBlock);
        dictGo.Add('2', ladder);
        dictGo.Add('3', outerBlock);
        dictGo.Add('4', crate);
        dictGo.Add('b', enemies[0]);
        dictGo.Add('c', enemies[1]);
        dictGo.Add('d', enemies[2]);
        dictGo.Add('e', enemies[3]);
        dictGo.Add('f', enemies[4]);



        if (doInst)
        {
            StreamReader sr = new StreamReader("Config/level.txt");
            List<string> sar = new List<string>();
            while (!sr.EndOfStream)
            {
                var s = sr.ReadLine();
                sar.Add(s);
            }
            sr.Close();

            n = sar.Count;
            m = sar[0].Length;
            mtr = new char[n][];
            for (int i = 0; i < n; i++)
            {
                mtr[i] = new char[m];
                for (int j = 0; j < m; j++)
                    mtr[i][j] = ' ';
            }


            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    mtr[i][j] = sar[n-i-1][j];

            //we read params in mtr
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    if (mtr[i][j] == '0') continue;
                    if (mtr[i][j] == ' ') continue;

                    if (mtr[i][j] == 'a')
                    {
                        SetMe(startPoint, j, i);
                        continue;
                    }
                    if (mtr[i][j] == 'z')
                    {
                        SetMe(endPoint, j, i);
                        continue;
                    }

                    //int val = (int)System.Char.GetNumericValue(mtr[i][j]);
                    int val = (int)mtr[i][j];
                    if (mtr[i][j] == '\n') continue;
                    if (mtr[i][j] == '\r') continue;
                    if (val < 0) continue;

                    //Debug.Log(val);
                    //Debug.Log(mtr[i][j]);
                    var go = (GameObject)Instantiate(dictGo[mtr[i][j]], container);
                    SetMe(go, j, i);

                }

            isLoaded = true;
            //levelManager.SetActive(true);
            //camManager.GetComponent<CameraController>().Start();

        }
    }

    public void ParseToFile()
    {

        dx = g0x.position.x - g0.position.x;
        dy = g0y.position.y - g0.position.y;
        Debug.Log(dx);
        Debug.Log(dy);
        //we get bounds
        Vector2 minx = new Vector2(1000, 1000);
        Vector3 maxx = new Vector2(-1000, -1000);

        for (int i = 0; i < container.childCount; i++)
        {
            if (container.GetChild(i).position.x < minx.x) minx.x = container.GetChild(i).position.x;
            if (container.GetChild(i).position.y < minx.y) minx.y = container.GetChild(i).position.y;
            if (container.GetChild(i).position.x > maxx.x) maxx.x = container.GetChild(i).position.x;
            if (container.GetChild(i).position.y > maxx.y) maxx.y = container.GetChild(i).position.y;
        }


        n = (int)Mathf.Round((maxx.y - minx.y) / dy) + 1;
        m = (int)Mathf.Round((maxx.x - minx.x) / dx) + 1;

        mtr = new char[n][];
        for (int i = 0; i < n; i++)
        {
            mtr[i] = new char[m];
            for (int j = 0; j < m; j++)
                mtr[i][j] = ' ';
        }

        //legend
        /*
         * 0 - empty
         * 1 - inner block
         * 2 - ladder
         * 3 - close block
         * 4 - crate
         * a - player start
         * z - end
         * b---- enemies
         * 
         * */


        for (int i = 0; i < container.childCount; i++)
        {
            int x = (int)Mathf.Round((container.GetChild(i).position.x - g0.position.x) / dx);
            int y = (int)Mathf.Round((container.GetChild(i).position.y - g0.position.y) / dy);

            if (container.GetChild(i).GetComponent<Ladder>() != null)
            {
                mtr[y][x] = '2';
            }
            else if (container.GetChild(i).GetComponent<RandomSprite>() != null)
            {
                if (container.GetChild(i).GetComponent<RandomSprite>().SpriteCollection.Length >= 10)
                {
                    mtr[y][x] = '1';
                }
                else
                {
                    mtr[y][x] = '3';
                }
            }
            else
            {
                mtr[y][x] = '4';
            }
        }

        //enemies turn
        for (int i = 0; i < enemyCont.childCount; i++)
        {
            GetMe(enemyCont.GetChild(i), 'b');
        }

        //start point
        GetMe(startPoint.transform, 'a');
        GetMe(endPoint.transform, 'z');



        StreamWriter sw = new StreamWriter("Config/level.txt");
        for (int i = n-1; i>=0; i--)
        {
            string s = "";
            for (int j = 0; j < m; j++) s += mtr[i][j].ToString();
            sw.WriteLine(s);
        }



        sw.Flush();
        sw.Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            ParseToFile();
        }
    }
}
