using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grid : MonoBehaviour
{
    public Transform player;
    public Transform target;
    public LayerMask unwalkableMask;
    Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;
    List<Node> oldNeighbors;

    CameraController camCon;
    
    public bool playerStart = true;
    public bool targetStart = true;

    //public GameObject prefab;
    public GameObject[,] tiles;

    GameObject Tiles;

    Pathfinding pathfinder;

    public List<Node> path;

    float nodeDiameter;
    public int gridSizeX, gridSizeY;

    //[Range(17, 30)]
    public int mapSizeX;
    //[Range(9,30)]
    public int mapSizeY;

    private void Awake()
    {

        Camera cam = Camera.main;
        camCon = cam.GetComponent<CameraController>();

        gridWorldSize.x = mapSizeX;
        gridWorldSize.y = mapSizeY;

        this.transform.position = new Vector3((gridWorldSize.x / 2) - .5f, (gridWorldSize.y / 2) - .5f);

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        camCon.panLimit = new Vector2(gridSizeX - 9.5f, gridSizeY - 5.5f);

        Tiles = new GameObject();
        Tiles.name = "Tiles";
        oldNeighbors = new List<Node>();
        pathfinder = GetComponent<Pathfinding>();
        path = new List<Node>();

        CreateGrid();
        Clear();

    }

    private void Update()
    {
        if (tiles != null)
        {
            GameObject playerPos = TileFromWorldPoint(player.position);
            GameObject targetPos = TileFromWorldPoint(target.position);

            Node startNode = NodeFromWorldPoint(player.position);
            Node targetNode = NodeFromWorldPoint(target.position);

            foreach (GameObject tile in tiles)
            {
                Tile tileThis = tile.GetComponent<Tile>();
                Node thisNode = NodeFromWorldPoint(tile.transform.position);

                if (tile == playerPos && playerStart == true)
                {
                    tileThis.tileTypeSwitch = 3;
                    playerStart = false;
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
            
            if(pathfinder.closedSet != null)
            {
                foreach (Node n in pathfinder.closedSet)
                {
                    GetNeighbor(n);
                    pathfinder.SetNeighbor(n, targetNode);
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

                
                    tiles[x, y] = new GameObject();
                    Tile tile = tiles[x, y].AddComponent<Tile>();
                    
                    tiles[x, y].GetComponent<Tile>().grid = this;
                    
                    tiles[x, y].transform.position = worldPoint;
                    tiles[x, y].transform.SetParent(Tiles.transform);
                
                tiles[x, y].name = "Tile: " + x + ", " + y;

                grid[x, y] = new Node(walkable, worldPoint, x, y);

                #region tileText
                /*GameObject gText = new GameObject();
                gText.transform.SetParent(tiles[x, y].transform);
                GameObject hText = new GameObject();
                hText.transform.SetParent(tiles[x, y].transform);
                GameObject fText = new GameObject();
                fText.transform.SetParent(tiles[x, y].transform);

                TextMeshPro textG = gText.AddComponent<TextMeshPro>();
                TextMeshPro textH = hText.AddComponent<TextMeshPro>();
                TextMeshPro textF = fText.AddComponent<TextMeshPro>();

                gText.name = "Gtext";
                tile.gText = textG;
                textG.alignment = TextAlignmentOptions.Center;
                gText.GetComponent<RectTransform>().sizeDelta = new Vector2(0.9f, 0.6f);
                gText.transform.localPosition = new Vector3(-0.20f, 0.25f);
                textG.fontSize = 2;
                textG.color = Color.black;

                hText.name = "Htext";
                tile.hText = textH;
                textH.alignment = TextAlignmentOptions.Center;
                hText.GetComponent<RectTransform>().sizeDelta = new Vector2(0.9f, 0.6f);
                hText.transform.localPosition = new Vector3(0.25f, 0.25f);
                textH.fontSize = 2;
                textH.color = Color.black;

                fText.name = "Ftext";
                tile.fText = textF;
                textF.alignment = TextAlignmentOptions.Center;
                fText.GetComponent<RectTransform>().sizeDelta = new Vector2(0.9f, 0.6f);
                fText.transform.localPosition = new Vector3(0.04f, -0.2f);
                textF.fontSize = 3;
                textF.color = Color.black;*/
                #endregion
            }
        }
    }

    public List<Node> GetNeighbor(Node node)
    {
        List<Node> neighbors = new List<Node>();

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

                GameObject tile = TileFromWorldPoint(new Vector3(checkX , checkY));
                Tile thisTile = tile.GetComponent<Tile>();

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    if (!neighbors.Contains(grid[checkX, checkY]) && thisTile.tileTypeSwitch != 3 && thisTile.tileTypeSwitch != 4  && thisTile.tileTypeSwitch != 6)
                    {
                        neighbors.Add(grid[checkX, checkY]);
                        thisTile.tileTypeSwitch = 2;

                        if (pathfinder.openSet != null && !pathfinder.openSet.Contains(grid[checkX, checkY]))
                        {
                            pathfinder.openSet.Add(grid[checkX, checkY]);
                        }
                    }
                }
            }
        }
        return neighbors;
    }

    public void Clear()
    {
        pathfinder.StopAllCoroutines();

        Node startNode = NodeFromWorldPoint(player.position);
        Node targetNode = NodeFromWorldPoint(target.position);

        if (pathfinder.openSet != null)
        {
            pathfinder.openSet.Clear();
        }
        if (pathfinder.closedSet != null)
        {
            pathfinder.closedSet.Clear();
        }
        path.Clear();

        if (pathfinder.openSet != null)
        {
            pathfinder.closedSet.Add(startNode);
        }

        if (pathfinder.closedSet != null)
        {
            pathfinder.SetNeighbor(startNode, targetNode);
        }
        startNode.hCost = pathfinder.GetDistance(startNode, targetNode);

        foreach (GameObject tile in tiles)
        {
            Tile tileThis = tile.GetComponent<Tile>();
            Node thisNode = NodeFromWorldPoint(tile.transform.position);


            thisNode.gCost = 0;
            thisNode.hCost = 0;

            if (tileThis.tileTypeSwitch == 3 || tileThis.tileTypeSwitch == 2 || tileThis.tileTypeSwitch == 6 && !pathfinder.openSet.Contains(thisNode) && !pathfinder.closedSet.Contains(thisNode))
            {
                tileThis.tileTypeSwitch = 1;
            }

            playerStart = true;
            targetStart = true;
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {


        float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f - (nodeRadius / gridWorldSize.x + 0.0000001f);
        float percentY = (worldPosition.y - transform.position.y) / gridWorldSize.y + 0.5f - (nodeRadius / gridWorldSize.y + 0.0000001f);

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
        float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f - (nodeRadius / gridWorldSize.x + 0.0000001f);
        float percentY = (worldPosition.y - transform.position.y) / gridWorldSize.y + 0.5f - (nodeRadius / gridWorldSize.y + 0.0000001f);
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
                if(path!=null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.green;
                    }
                }
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
