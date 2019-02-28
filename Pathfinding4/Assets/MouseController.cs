using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Tile tileHit = hit.transform.gameObject.GetComponent<Tile>();
                Node node = grid.NodeFromWorldPoint(hit.transform.position);

                if (tileHit.tileTypeSwitch == 1)
                {
                    tileHit.tileTypeSwitch = 4;
                }
                else if (tileHit.tileTypeSwitch == 4)
                {
                    tileHit.tileTypeSwitch = 1;
                }
            }
        }
        else if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Tile tileHit = hit.transform.gameObject.GetComponent<Tile>();
                Node node = grid.NodeFromWorldPoint(hit.transform.position);

                if (tileHit.tileTypeSwitch == 2)
                {
                    tileHit.tileTypeSwitch = 3;
                    grid.GetNeighbor(node);
                }
            }
        }
    }
}
