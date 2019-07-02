using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingMonsterScript : MonsterScript
{
    public override void Attack()
    {
        if (_moveCounter < _chargeAtMove)
            checkPosition();
        else
            _rigid.velocity = (_cameraPosition - transform.position).normalized * 3.0f;
    }
}
