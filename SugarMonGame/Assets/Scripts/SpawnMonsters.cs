using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonsters : MonoBehaviour
{
    public static SpawnMonsters instance;

    public GameObject _monsterPrefab;
    public float _radius = 10;

    private Transform _cameraTransform;
    private int counter = 0;
    private List<GameObject> monsters = new List<GameObject>();

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

    public Vector3 GetNewPosition(GameObject obj)
    {
        GameObject newObj = null;

        do
        {
            newObj = monsters[Random.Range(0, monsters.Count)];
        } while (obj == newObj);


        return newObj.transform.position;
    }

    public void RemoveMonster(GameObject obj){monsters.Remove(obj);}
    public int GetNumOfMonstersAlive(){return monsters.Count;}
}
