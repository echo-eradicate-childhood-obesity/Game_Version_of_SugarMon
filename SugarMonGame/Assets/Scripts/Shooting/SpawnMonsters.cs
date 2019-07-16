//This file was created by Mark Botaish on June 7th, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonsters : MonoBehaviour
{
        #region PUBLIC_VARS
        public static SpawnMonsters instance;

        [Tooltip("The minions prefab of a monster")] public GameObject _monsterPrefab;
        [Tooltip("The minions prefab of a monster v 2")] public GameObject _monsterCane;
        [Tooltip("The minions prefab of a monster v 2")] public GameObject _monsterDonut;
       // [Tooltip("The minions prefab of a monster v 2")] public GameObject _monsterCan;
        [Tooltip("The minions prefab of a monster v 2")] public GameObject _monsterConcentrate;
       //[Tooltip("The minions prefab of a monster v 2")] public GameObject _monsterGlucose;
        [Tooltip("The max distance from the camera a monster can get")] public float _radius = 10;
        #endregion

        #region PRIVATE_VARS
        private Transform _cameraTransform;                              // The transfrom of the camera in the scene
        private List<GameObject> _monsters = new List<GameObject>();     // The list of monsters
        private bool _shouldSpawn = true;
        #endregion

        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _cameraTransform = GameObject.Find("ARCore Device").transform.GetChild(0);
        }

        // Update is called once per frame
        void Update()
        {
            if (_shouldSpawn)
                SpawnMonster();
        }

        /// <summary>
        /// This fucntion is used to spawn 10 monsters. This function will get called when 
        /// all have died or the game has just started.
        /// </summary>
        void SpawnMonster()
        {
            if (_monsters.Count < 10)
            {
                System.Random rnd = new System.Random();
                int monType = rnd.Next(1, 6);
                if (monType == 1)
                {
                    GameObject monster = Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
                    Vector3 vel = Random.onUnitSphere * Random.Range(2, 10);
                    monster.GetComponent<Rigidbody>().velocity = vel;
                    monster.GetComponent<ChargingMonsterScript>().InitMonster(_cameraTransform.position, _radius);
                    _monsters.Add(monster);
                }
                if (monType == 2)
                {
                    GameObject monster = Instantiate(_monsterCane, transform.position, Quaternion.identity);
                    Vector3 vel = Random.onUnitSphere * Random.Range(2, 10);
                    monster.GetComponent<Rigidbody>().velocity = vel;
                    monster.GetComponent<ChargingMonsterScript>().InitMonster(_cameraTransform.position, _radius);
                    _monsters.Add(monster);
                }
                if (monType == 3)
                {
                    GameObject monster = Instantiate(_monsterDonut, transform.position, Quaternion.identity);
                    Vector3 vel = Random.onUnitSphere * Random.Range(2, 10);
                    monster.GetComponent<Rigidbody>().velocity = vel;
                    monster.GetComponent<ChargingMonsterScript>().InitMonster(_cameraTransform.position, _radius);
                    _monsters.Add(monster);
                }/*
                if (monType == 4)
                {
                    GameObject monster = Instantiate(_monsterCan, transform.position, Quaternion.identity);
                    Vector3 vel = Random.onUnitSphere * Random.Range(2, 10);
                    monster.GetComponent<Rigidbody>().velocity = vel;
                    monster.GetComponent<ChargingMonsterScript>().InitMonster(_cameraTransform.position, _radius);
                    _monsters.Add(monster);
                }
                if (monType == 5)
                {
                    GameObject monster = Instantiate(_monsterGlucose, transform.position, Quaternion.identity);
                    Vector3 vel = Random.onUnitSphere * Random.Range(2, 10);
                    monster.GetComponent<Rigidbody>().velocity = vel;
                    monster.GetComponent<ChargingMonsterScript>().InitMonster(_cameraTransform.position, _radius);
                    _monsters.Add(monster);
                }*/
                else
                {
                    GameObject monster = Instantiate(_monsterConcentrate, transform.position, Quaternion.identity);
                    Vector3 vel = Random.onUnitSphere * Random.Range(2, 10);
                    monster.GetComponent<Rigidbody>().velocity = vel;
                    monster.GetComponent<ChargingMonsterScript>().InitMonster(_cameraTransform.position, _radius);
                    _monsters.Add(monster);
                }
            }
            else
        {
            _shouldSpawn = false;
        }
    }

    /// <summary>
    ///  This function gets the position of a random monster in the list. This allows the AI to look more
    ///  randomizes. This position is used to determine the new direction of the gameobject
    /// </summary>
    ///  -This function gets called from the MonsterScript-
    public Vector3 GetNewPosition(GameObject obj)
    {
        GameObject newObj = null;

        do
        {
            newObj = _monsters[Random.Range(0, _monsters.Count)];

        } while (obj == newObj);
        
        return newObj.transform.position;
    }

    /// <summary>
    /// This function is used to remove a destroyed monster from the list.
    /// </summary>
    /// -This function gets called from the MonterScript-
    public void RemoveMonster(GameObject obj){
        _monsters.Remove(obj);
        if (_monsters.Count <= 0)
            _shouldSpawn = true;
    }

    /// <summary>
    /// This function is used to determine the currebt number of alive monsters in the scene.
    /// </summary>
    /// -This function gets called from the MonsterScript-
    public int GetNumOfMonstersAlive(){return _monsters.Count;}
}
