//This file was created by Mark Botaish on June 7th

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonsters : MonoBehaviour
{
    #region PUBLIC_VARS
    public static SpawnMonsters instance;

    [Tooltip("The minions prefab of a monster")]                        public GameObject _monsterPrefab;
    [Tooltip("The max distance from the camera a monster can get")]     public float _radius = 10;
    #endregion

    #region PRIVATE_VARS
    private Transform _cameraTransform;                             // The transfrom of the camera in the scene
    private int _counter = 0;                                        // The counter of monsters that have been spawned in the scene
    private List<GameObject> _monsters = new List<GameObject>();     // The list of monsters
    #endregion

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
        if(++_counter < 10)
        {
            GameObject monster = Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
            Vector3 vel = Random.onUnitSphere * Random.Range(2, 10);
            monster.GetComponent<Rigidbody>().velocity = vel;
            monster.GetComponent<MonsterScript>().InitMonster(_cameraTransform.position, _radius);
            _monsters.Add(monster);
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
    public void RemoveMonster(GameObject obj){_monsters.Remove(obj);}

    /// <summary>
    /// This function is used to determine the currebt number of alive monsters in the scene.
    /// </summary>
    /// -This function gets called from the MonsterScript-
    public int GetNumOfMonstersAlive(){return _monsters.Count;}
}
