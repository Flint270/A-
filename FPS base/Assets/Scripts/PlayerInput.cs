using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerInput : MonoBehaviour
{
    public float speed = 10.0f;

    public bool isPaused;
    PlayerMovement movement;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        movement = GetComponent<PlayerMovement>();
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f1"))
        {
            isPaused = !isPaused;
            if (isPaused == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (isPaused == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (isPaused == false)
        {
            movement.translation = Input.GetAxis("Vertical") * speed;
            movement.straffe = Input.GetAxis("Horizontal") * speed;
            
        }
    }
}
