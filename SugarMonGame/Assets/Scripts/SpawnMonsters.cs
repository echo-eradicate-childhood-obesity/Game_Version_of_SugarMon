//This file was created by Mark Botaish 

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
    private Transform _cameraTransform;
    private int counter = 0;
    private List<GameObject> monsters = new List<GameObject>();
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
        if(++counter < 10)
        {
            GameObject monster = Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
            Vector3 vel = Random.onUnitSphere * Random.Range(2, 10);
            monster.GetComponent<Rigidbody>().velocity = vel;
            monster.GetComponent<MonsterScript>().InitMonster(_cameraTransform.position, _radius);
            monsters.Add(monster);
        }
    }

    /* This function gets the position of a random monster in the list. This allows the AI to look more 
     * randomizes. This position is used to determine the new direction of the gameobject
     * <This function gets called from the MonsterScript>
    */
    public Vector3 GetNewPosition(GameObject obj)
    {
        GameObject newObj = null;

        do
        {
            newObj = monsters[Random.Range(0, monsters.Count)];
        } while (obj == newObj);
        
        return newObj.transform.position;
    }

    //This function is used to remove a destroyed monster from the list
    //<This function gets called from the MonterScript>
    public void RemoveMonster(GameObject obj){monsters.Remove(obj);}

    //This function is used to determine the currebt number of alive monsters in the scene
    //<This function gets called from the MonsterScript>
    public int GetNumOfMonstersAlive(){return monsters.Count;}
}
