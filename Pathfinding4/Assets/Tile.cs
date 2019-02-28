using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum tileType { Empty, Open, Locked, Wall, Target};

    Sprite empty;
    Sprite open;
    Sprite locked;
    Sprite wall;
    Sprite target;

    public tileType type;
    public SpriteRenderer sprRen;
    public BoxCollider2D boxCol;
    public int tileTypeSwitch;

    public Grid grid;
    Node node;

    [SerializeField]
    int gCost;
    [SerializeField]
    int hCost;
    [SerializeField]
    int fCost;

    public Tile thisTile;

    public GameObject tile;

    private void Start()
    {
        thisTile = this;

        tile = this.transform.gameObject;

        boxCol = tile.AddComponent<BoxCollider2D>();
        sprRen = tile.AddComponent<SpriteRenderer>();

        empty = Resources.Load<Sprite>("Sprites/emptyTile");
        open = Resources.Load<Sprite>("Sprites/openTile");
        locked = Resources.Load<Sprite>("Sprites/lockedTile");
        wall = Resources.Load<Sprite>("Sprites/wall");
        target = Resources.Load<Sprite>("Sprites/target");
        tileTypeSwitch = 1;
        TileSwitch(tileTypeSwitch);
    }

    private void Update()
    {
        TileSwitch(tileTypeSwitch);
        node = grid.NodeFromWorldPoint(tile.transform.position);

        gCost = node.gCost;
        hCost = node.hCost;
        fCost = node.fCost;
    }

    public void TileSwitch(int trigger)
    {
        switch (trigger)
        {
            case 1:
                type = tileType.Empty;
                sprRen.sprite = empty;
                break;
            case 2:
                type = tileType.Open;
                sprRen.sprite = open;
                break;
            case 3:
                type = tileType.Locked;
                sprRen.sprite = locked;
                break;
            case 4:
                type = tileType.Wall;
                sprRen.sprite = wall;
                break;
            case 5:
                type = tileType.Target;
                sprRen.sprite = target;
                break;
        }
    }
}
