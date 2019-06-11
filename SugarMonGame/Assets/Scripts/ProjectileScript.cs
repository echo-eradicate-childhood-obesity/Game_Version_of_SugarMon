/**
 * ProjectileScript.cs - determines the behavior of a projectile from when it was created until it is destroyed
 * Created by Aidan Lee on 6/11/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Vector3 _cameraPosition;
    private Vector3 _forwardDirection;
    private float _radius;
    private Rigidbody _rigid;
    private bool _hasBeenInitted = false; 
    private int _MoveCounter = 0;
    private SpawnProjectile sp;

    public void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        sm = SpawnProjectile.instance;
    }

    public void InitProjectile(Vector3 pos, float radius)
    {
        _cameraPosition = pos;
        _radius = radius;
        _forwardDirection = sm._cameraTransform.foreward;
        _hasBeenInitted = true; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_hasBeenInitted)
        {
            if(_MoveCounter < _ChargeAtMove)
                checkPosition();
            else
            {
                _rigid.velocity = (_cameraPosition - transform.position).normalized * 3.0f;
            }
        }
       
    }

    void checkPosition()
    {
        if((transform.position - _cameraPosition).sqrMagnitude >= (_radius * _radius) || Vector3.Dot(_forwardDirection, transform.position - _cameraPosition) < 0 )//|| !TestPoint(gameObject.transform.position))
        {
            if(sm.GetNumOfMonstersAlive() > 1)
            {
                Vector3 vel = (sm.GetNewPosition(gameObject) - transform.position).normalized * Random.Range(2,10);
                _rigid.velocity = vel;
            }
            else
            {
                _rigid.velocity = -_rigid.velocity;
            }
            _MoveCounter++;
        }
    }

}
