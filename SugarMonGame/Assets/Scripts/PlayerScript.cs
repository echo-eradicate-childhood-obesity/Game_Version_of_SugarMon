//This file was created by Mark Botaish on June 14th, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Singleton instance
    public static PlayerScript instance;

    #region PUBLIC_VARS
    [Header("Testing")]
    [Tooltip("Enables the ability to test on computers")]   public bool _IsTesting = false;   

    [Header("Mouse Settings")]
    [Tooltip("Horizontal mouse sensitivity")]               public float _mouseSensitivityX = 3.0f;    
    [Tooltip("Vertical mouse sensitivity")]                 public float _mouseSensitivityY = 3.0f;    

    [Header("Shooting Settings")]
    [Tooltip("The projectile prefab")]                      public GameObject _projectile;             
    [Tooltip("Damage of the projectile")]                   public float _damage = 50;                 
    [Tooltip("How fast the projectile is shot")]            public float _projectileSpeed = 10;        
    [Tooltip("The delay between shots")]                    public float _bulletDelay = 0.15f;         

    #endregion

    #region PRIVATE_VARS
    //Bullet settings
    private bool _canShoot = true;                  // A check to see if the bullet can shoot. This allows for a delayed shot
    private float _bulletTimer = 0;                 // The bullet timer to wait the time between shots

    //Mouse settings
    private float _mouseX = 0.0f;                   // The current x rotation based on the mouse movement
    private float _mouseY = 0.0f;                   // The current y rotation based on the mouse movement
    #endregion

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_IsTesting) //If you are using a computer, lock the mouse to the middle of the Game window
            Cursor.lockState = CursorLockMode.Locked;        
    }

    void Update()
    {
        if (_IsTesting) //If you are testing on computer, then the camera moves with the mouse
        {
            _mouseX += Input.GetAxis("Mouse X") * _mouseSensitivityX;
            _mouseY -= Input.GetAxis("Mouse Y") * _mouseSensitivityY;

            transform.eulerAngles = new Vector3(_mouseY, _mouseX, 0.0f);
        }
        Shoot();
    }

    /// <summary>
    /// This function is used to shoot a projectile. For now, holding/clicking the left mouse button will shoot.
    /// </summary>
    void Shoot()
    {
        //If between shots update the timer
        if (!_canShoot) 
        {
            _bulletTimer += Time.deltaTime;
            //If the timer is about the delay time, enable the ability to shoot
            if (_bulletTimer > _bulletDelay)
                _canShoot = true;
        }

        //If you can shoot and you are holding the left mouse button down
        if (_canShoot && Input.GetMouseButton(0)) 
        {
            Vector3 shootPos = transform.position - transform.up * 0.25f + transform.forward * 0.1f; //Shooting the projectile slightly below and infront of the camera
            GameObject obj = Instantiate(_projectile, shootPos, Quaternion.identity); //Spawn the projectil

            //This makes it look like the projectile it moving at the crosshair location
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity, 9)) //If the crosshair hits something move the projectile to that postion
                obj.GetComponent<Rigidbody>().velocity = (hit.point - shootPos).normalized * _projectileSpeed;
            else // Move the projectile at a position that is 60 meters direction in the path of the crosshair
                obj.GetComponent<Rigidbody>().velocity = ((transform.forward * 60 + transform.position) - transform.position).normalized * _projectileSpeed;
            
            
            _canShoot = false;//Enable the day 
            _bulletTimer = 0;//Reset the timer

            //Cleanup for the projectile if it doesnt hit anything within 3 seconds
            Destroy(obj, 3); 
        }
    }
}
