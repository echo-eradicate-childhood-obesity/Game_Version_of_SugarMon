/**
 * SpawnProjectile.cs - creates a projectile object
 * Created by Aidan Lee on 6/11/2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    public static SpawnProjectile instance;

    public GameObject _projectilePrefab;
    public float _radius = 2;

    private Transform _cameraTransform; // current camera position
    private List<GameObject> projectiles = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = GameObject.Find("Main Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 || Input.anyKey)
        {
            GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            Vector3 vel = Random.onUnitSphere * Random.Range(2, 10);
            projectile.GetComponent<Rigidbody>().velocity = vel;
            projectile.GetComponent<MonsterScript>().InitMonster(_cameraTransform.position, _radius);
            projectiles.Add(projectile);
        }
    }

    public void RemoveMonster(GameObject obj){monsters.Remove(obj);}
    public int GetNumOfMonstersAlive(){return monsters.Count;}
}
