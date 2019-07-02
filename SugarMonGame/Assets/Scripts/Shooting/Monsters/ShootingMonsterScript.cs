using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingMonsterScript : MonsterScript
{
    public override void Attack()
    {
        if (_moveCounter < _chargeAtMove)
            checkPosition();
        else
            _rigid.velocity = Vector3.zero;
    }
}
