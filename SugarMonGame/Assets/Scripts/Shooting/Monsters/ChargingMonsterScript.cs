//This script was created by Mark Botaish on July 2nd, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingMonsterScript : MonsterScript
{
    private bool _sendNot = false;

    public override void Attack()
    {
        gameObject.transform.LookAt(_cameraPosition);
        if (_moveCounter < _chargeAtMove)
            checkPosition();
        else
        {
            if (!_sendNot)
            {
                StartCoroutine(CanvasScript.instance.CreateWarning(gameObject));
                _sendNot = true;
            }
            _rigid.velocity = (_cameraPosition - transform.position).normalized * 3.0f;
        }
           
    }
}
