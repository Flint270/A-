using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    Grid grid;
    Pathfinding pathfinder;

    [Range(2,100)]
    public int randMax = 75;

    public bool Enum;
    public bool loop;
    bool pathFound;
    public bool pathing;

    [Range(1,6)]
    public int loopTimer;

    public GameObject player;

    //public float dragSpeed = 2;
    //private Vector3 dragOrigin;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
        pathfinder = GetComponent<Pathfinding>();
        loop = false;
        pathFound = false;
        pathing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            loop = !loop;
            pathing = false;
            if (loop == true)
            {
                //Debug.Log("loop set true");
                StartCoroutine(Loop());
            }
            else if (loop == false)
            {
                StopAllCoroutines();
            }
        }

        

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
        else if (Input.GetMouseButtonDown(0))
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
                    pathfinder.openSet.Remove(node);
                    pathfinder.closedSet.Add(node);
                }
            }
        }

        else if(Input.GetKeyDown(KeyCode.Alpha1))
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
                    tileHit.tileTypeSwitch = 7;
                    tileHit.teleCode = 1;
                }
                else if(tileHit.tileTypeSwitch == 7)
                {
                    tileHit.tileTypeSwitch = 1;
                    tileHit.teleCode = 0;
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
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
                    tileHit.tileTypeSwitch = 7;
                    tileHit.teleCode = 2;
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.Q))
        {
            pathing = false;
            //pathfinder.StopAllCoroutines();
            grid.Clear();
            if(Enum == true)
            {
                pathfinder.PathFind(pathfinder.seeker.position, pathfinder.target.position);
            }
            else
            {
                pathfinder.FindPath(pathfinder.seeker.position, pathfinder.target.position);
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            pathing = false;
            pathfinder.StopAllCoroutines();
            grid.Clear();
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {

            pathing = false;
            foreach (GameObject tileGo in grid.tiles)
            {
                int r = Random.Range(1, 101);
                Tile tile = tileGo.GetComponent<Tile>();

                if(r <= randMax)
                {
                    tile.tileTypeSwitch = 1;
                }
                else
                {
                    tile.tileTypeSwitch = 4;
                }
            }
            grid.Clear();
        }

        #region Movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            Tile tile = grid.TileFromWorldPoint(new Vector3(player.transform.position.x, player.transform.position.y + 1)).GetComponent<Tile>();
            if (player.transform.position.y + 1 < grid.gridSizeY && tile.tileTypeSwitch != 4)
            {
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1);
                grid.Clear();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Tile tile = grid.TileFromWorldPoint(new Vector3(player.transform.position.x-1, player.transform.position.y)).GetComponent<Tile>();
            if (player.transform.position.x - 1 >= 0 && tile.tileTypeSwitch != 4)
            {
                player.transform.position = new Vector3(player.transform.position.x - 1, player.transform.position.y);
                grid.Clear();
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Tile tile = grid.TileFromWorldPoint(new Vector3(player.transform.position.x, player.transform.position.y - 1)).GetComponent<Tile>();
            if (player.transform.position.y - 1 >= 0 &&  tile.tileTypeSwitch != 4)
            {
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1);
                grid.Clear();
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Tile tile = grid.TileFromWorldPoint(new Vector3(player.transform.position.x+1, player.transform.position.y)).GetComponent<Tile>();
            if (player.transform.position.x + 1 < grid.gridSizeX && tile.tileTypeSwitch != 4)
            {
                player.transform.position = new Vector3(player.transform.position.x + 1, player.transform.position.y);
                grid.Clear();
            }
        }
        #endregion
    }

    IEnumerator Loop()
    {
        while (loop == true)
        {
            if(pathing == false)
            {
                grid.Clear();
                pathing = true;
                if (Enum == true)
                {
                    pathfinder.PathFindLoop(pathfinder.seeker.position, pathfinder.target.position, () =>
                    {
                        pathing = false;
                    });
                }
                else
                {
                    pathfinder.FindPathLoop(pathfinder.seeker.position, pathfinder.target.position, () =>
                    {
                        pathing = false;
                    });
                }
            }
            yield return new WaitForSeconds(loopTimer);
        }
    }
}
