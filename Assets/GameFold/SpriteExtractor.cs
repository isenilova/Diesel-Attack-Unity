using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpriteExtractor : MonoBehaviour
{

    public Texture2D tex;
    public Sprite texS;
    public Sprite[] sprites;

    [ContextMenu("CopyTex")]
    public void CopyTex()
    {
        sprites = Resources.LoadAll<Sprite>("RetroProps3");

        Debug.Log(texS.rect);
        tex = texS.texture;
        int sizeX = 16;
        int sizeY = 16;
        var t = new Texture2D(sizeX, sizeY);

        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
            {
                t.SetPixel(i, j, tex.GetPixel((int)texS.rect.x + i, (int)texS.rect.y + j));

            }

        t.Apply();
        var bt = t.EncodeToPNG();
        Debug.Log(Application.dataPath + "/" + "rect.png");
        File.WriteAllBytes(Application.dataPath + "/" + "rect.png", bt);
    }

    [ContextMenu("MakeTransp")]
    public void MakeTransp()
    {
        int sizeX = 16;
        int sizeY = 16;
        var t = new Texture2D(sizeX, sizeY);

        for (int i = 0; i < sizeX; i++)
            for (int j = 0; j < sizeY; j++)
            {
                var c = tex.GetPixel(i,j);
                if (c != Color.white)
                t.SetPixel(i, j, tex.GetPixel( i, j));
                else
                t.SetPixel(i, j, new Color(0,0,0,0));
            }

        t.Apply();
        var bt = t.EncodeToPNG();
        Debug.Log(Application.dataPath + "/" + "rect.png");
        File.WriteAllBytes(Application.dataPath + "/" + "rect.png", bt);
    }

}
