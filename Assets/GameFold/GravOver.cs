using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravOver : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.tag != "Player") return;

        if (collision.GetComponent<CharacterAbility>()._movement.CurrentState == CharacterStates.MovementStates.Dashing)
        {
            collision.GetComponent<CharacterAbility>()._movement.ChangeState(CharacterStates.MovementStates.Idle);
        }
    }
}
