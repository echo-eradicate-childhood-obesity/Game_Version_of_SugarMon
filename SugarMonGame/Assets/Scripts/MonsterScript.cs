//This file was created by Mark Botaish

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{

    #region PRIVATE_VARS
    private Vector3 _cameraPosition;
    private Vector3 _forwardDirection;
    private float _radius;
    private Rigidbody _rigid;
    private bool _hasBeenInitted = false;
    private int _MoveCounter = 0;
    private int _ChargeAtMove;
    private SpawnMonsters sm;
    #endregion

    public void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        sm = SpawnMonsters.instance;
        _ChargeAtMove = Random.Range(2,7);
    }

    /*
     * Init the variables need for the monster AI to work. Pos is the postion of the camera and the radius
     * is the max distance the monster can travel from the camera
     * <This file gets called from the MonsterSciprt>
     */
    public void InitMonster(Vector3 pos, float radius)
    {
        _cameraPosition = pos;

        //Create a position that is at the same height of the camera
        Vector3 temp = new Vector3(transform.position.x, pos.y, transform.position.z); 

        //Create a horizontal ray from the camera to the the spawn point
        _forwardDirection = (temp - pos).normalized;
        _radius = radius;
        _hasBeenInitted = true; 
    }

    void FixedUpdate()
    {
        if (_hasBeenInitted)
        {
            if(_MoveCounter < _ChargeAtMove)
                checkPosition();
            else           
                _rigid.velocity = (_cameraPosition - transform.position).normalized * 3.0f;    
        }      
    }
    /*
     * This function stops the monsters from going out of view of the camera. A new velocity is set
     * when the distance is outside of the radius, the monster position is behind the camera and 
     * when the monster is not in the correct downward/upward view
     * <This function gets called from the Update function>
     */
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

    /*
     * This function is determines if the position is inside of the roaming area, specifically
     * the downward/upward view
     * <This function gets called from the checkPosition function>
    */
    bool TestPoint(Vector3 pos)
    {
        float mag = equation(new Vector2(pos.x, pos.z).magnitude);

        if (pos.y < mag && pos.y > -mag)
            return true;

        return false;
    }

    /* This function is to get the y value from an x value of an equation. This current equation 
     * y = x, but can easily be changed later
     * <This fucntion gets called from the TestPoint function>
    */
    float equation(float x)
    {
        return x;
    }

}
