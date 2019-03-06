using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;

    public float panSpeed;
    public float panBorderThickness;
    public Vector2 panLimit;

    public float scrollSpeed = 2;

    private void Awake()
    {
        cam = Camera.main;
        panSpeed = 20f;
        panBorderThickness = 10f;
    }

    private void Update()
    {
        Vector3 pos = transform.position;

        if(Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        cam.orthographicSize -= scroll * scrollSpeed * 100f * Time.deltaTime;

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 5, 20);

        pos.x = Mathf.Clamp(pos.x, 8.5f, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, 4.5f, panLimit.y);

        transform.position = pos;
    }
}
