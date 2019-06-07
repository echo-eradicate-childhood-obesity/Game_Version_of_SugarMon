using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{

    private Vector3 _cameraPosition;
    private Vector3 _forwardDirection;
    private float _radius;
    private Rigidbody _rigid;
    private bool _hasBeenInitted = false;
    private int _MoveCounter = 0;
    private int _ChargeAtMove;
    private SpawnMonsters sm;

    public void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        sm = SpawnMonsters.instance;
        _ChargeAtMove = Random.Range(2,7);
    }

    public void InitMonster(Vector3 pos, float radius)
    {
        _cameraPosition = pos;
        Vector3 temp = new Vector3(transform.position.x, pos.y, transform.position.z);
        _forwardDirection = (temp - pos).normalized;
        _radius = radius;
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

    bool TestPoint(Vector3 pos)
    {
        float mag = equation(new Vector2(pos.x, pos.z).magnitude);

        if (pos.y < mag && pos.y > -mag)
            return true;

        return false;
    }

    //y = x
    float equation(float x)
    {
        return x;
    }

}
