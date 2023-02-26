using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour, IPointerClickHandler  {

    public Character plr;
    public AudioClip clip;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        PlayerInfo.instance.player = plr;
    }
}
