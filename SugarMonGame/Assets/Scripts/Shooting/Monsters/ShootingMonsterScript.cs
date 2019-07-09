//This script was created by Mark Botaish on July 2nd, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingMonsterScript : MonsterScript
{
    public GameObject _bulletPrefab;
    public float _bulletSpeed;

    private bool _isTurning = false;
    private bool _canShoot = false;

    public override void Attack()
    {
        if (_moveCounter < _chargeAtMove)
        {
            checkPosition();
            gameObject.transform.LookAt(gameObject.transform.position + _rigid.velocity);
        }           
        else
        {
            _rigid.velocity = Vector3.zero;
            if (!_isTurning)
            {
                StartCoroutine(TurnToPlayer());
            }

            if (_canShoot)
            {
                StartCoroutine(Shoot());
            }            
        }            
    }

    private IEnumerator TurnToPlayer()
    {
        _isTurning = true;
        Quaternion lookRotation = Quaternion.LookRotation(_cameraPosition - transform.position);
        while (Quaternion.Angle(lookRotation, gameObject.transform.rotation) > 0.5f)
        {
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.0f);
            yield return null;
        }   

        yield return null;
        _canShoot = true;
    }

    private IEnumerator Shoot()
    {
        _canShoot = false;
        for(int i = 0; i < 2; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, gameObject.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = (_cameraPosition - gameObject.transform.position).normalized * _bulletSpeed;
            bullet.GetComponent<EnemyBulletScript>()._damage = _damage;
            StartCoroutine(CanvasScript.instance.CreateWarning(bullet));
            yield return new WaitForSeconds(1.0f);
        }

        yield return null;
        _isTurning = false;
        _moveCounter = 0;
        _chargeAtMove = Random.Range(3, 7);
        _rigid.velocity = (_sm.GetNewPosition(gameObject) - transform.position).normalized * Random.Range(1, 4); ;
    }
}
