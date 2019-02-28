using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;
    public Transform target;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;
    List<Node> oldNeighbors;

    public bool playerStart = true;
    public bool targetStart = true;

    //public GameObject prefab;
    GameObject[,] tiles;

    GameObject Tiles;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        Tiles = new GameObject();
        Tiles.name = "Tiles";
        oldNeighbors = new List<Node>();

        CreateGrid();
        
    }

    private void Update()
    {
        if (tiles != null)
        {
            GameObject playerPos = TileFromWorldPoint(player.position);
            GameObject targetPos = TileFromWorldPoint(target.position);


            Node node = NodeFromWorldPoint(player.position);

            foreach (GameObject tile in tiles)
            {
                Tile tileThis = tile.GetComponent<Tile>();
                Node thisNode = NodeFromWorldPoint(tile.transform.position);

                if (tile == playerPos && playerStart == true)
                {
                    tileThis.tileTypeSwitch = 3;
                    playerStart = false;
                    GetNeighbor(node);
                }
                else if (tile == targetPos && targetStart == true)
                {
                    tileThis.tileTypeSwitch = 5;
                    targetStart = false;
                }

                if(tileThis.tileTypeSwitch == 4)
                {
                    thisNode.walkable = false;
                }
                else
                {
                    thisNode.walkable = true;
                }
            }
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        tiles = new GameObject[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - (Vector3.right * gridWorldSize.x / 2) - (Vector3.up * gridWorldSize.y / 2);

        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);

                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));

                grid[x, y] = new Node(walkable, worldPoint, x, y);

                tiles[x, y] = new GameObject();
                tiles[x, y].AddComponent<Tile>();

                tiles[x, y].GetComponent<Tile>().grid = this;

                tiles[x, y].transform.parent = Tiles.transform;
                tiles[x, y].name = "Tile: " + x + ", " + y;
                tiles[x, y].transform.position = worldPoint;
            }
        }
    }

    public List<Node> GetNeighbor(Node node)
    {
        List<Node> neighbors = new List<Node>();
        //Debug.Log("test1");
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                Debug.Log("Node X:" + node.gridX + ", x: " + x + ", checkX: " + checkX);


                GameObject tile = TileFromWorldPoint(new Vector3(checkX , checkY));
                Tile thisTile = tile.GetComponent<Tile>();

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    if (!neighbors.Contains(grid[checkX, checkY]) && thisTile.tileTypeSwitch != 3 && thisTile.tileTypeSwitch != 4)
                    {
                        neighbors.Add(grid[checkX, checkY]);

                        thisTile.tileTypeSwitch = 2;
                    }
                }
            }
        }
        //Debug.Log("end loop");
        return neighbors;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f - (nodeRadius / gridWorldSize.x + 0.001f);
        float percentY = (worldPosition.y - transform.position.y) / gridWorldSize.y + 0.5f - (nodeRadius / gridWorldSize.y);

        //float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        //float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = (int)Mathf.Ceil((gridSizeX - 1) * percentX);
        int y = (int)Mathf.Ceil((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public GameObject TileFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f - (nodeRadius / gridWorldSize.x + 0.001f);
        float percentY = (worldPosition.y - transform.position.y) / gridWorldSize.y + 0.5f - (nodeRadius / gridWorldSize.y);
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = (int)Mathf.Ceil((gridSizeX - 1) * percentX);
        int y = (int)Mathf.Ceil((gridSizeY - 1) * percentY);
        return tiles[x, y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridWorldSize);

        if (grid != null)
        {
            Node playerNode = NodeFromWorldPoint(player.position);
            Node targetNode = NodeFromWorldPoint(target.position);
            foreach(Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if(playerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                if(targetNode == n)
                {
                    Gizmos.color = Color.black;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-0.1f));

            }
        }
    }
}
