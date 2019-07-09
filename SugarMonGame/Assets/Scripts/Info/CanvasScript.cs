using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    public static CanvasScript instance;
    public GameObject _warningIndicationPrefab;


    private Vector2 dim;
    private Vector2 offset = new Vector2(10, 20);

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        dim = new Vector2(Screen.width, Screen.height) - offset;

    }

    public IEnumerator CreateWarning(GameObject obj)
    { 
        GameObject _warningIndication = Instantiate(_warningIndicationPrefab, transform);
        while (obj != null)
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(obj.transform.position);
            if(pos.x < dim.x && pos.x > offset.x && pos.y < dim.y && pos.y > offset.y)
            {
                _warningIndication.SetActive(false);
            }
            else
            {
                if(!_warningIndication.activeSelf)
                    _warningIndication.SetActive(true);
            }

            pos.x = Mathf.Clamp(pos.x, offset.x, dim.x);
            pos.y = Mathf.Clamp(pos.y, offset.y, dim.y);
            _warningIndication.transform.position = pos;

            yield return null;
        }
        Destroy(_warningIndication);
    }
}
