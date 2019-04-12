using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerInput : MonoBehaviour
{
    public float speed = 10.0f;

    PlayerMovement movement;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.translation = Input.GetAxis("Vertical") * speed;
        movement.straffe = Input.GetAxis("Horizontal") * speed;
        

        if(Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
