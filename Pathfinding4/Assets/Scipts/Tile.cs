using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public enum tileType { Empty, Open, Locked, Wall, Target, Path};

    public Sprite empty;
    Sprite open;
    Sprite locked;
    Sprite wall;
    Sprite target;
    Sprite path;

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

    public TextMeshPro gText;
    public TextMeshPro hText;
    public TextMeshPro fText;

    public Tile thisTile;

    public GameObject tile;

    private void Awake()
    {
        //AddTile();
    }

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
        path = Resources.Load<Sprite>("Sprites/pathTile");

        tileTypeSwitch = 1;
    }

    private void Update()
    {
        TileSwitch(tileTypeSwitch);
        if (grid != null)
        {
            node = grid.NodeFromWorldPoint(tile.transform.position);
        }

        gCost = node.gCost;
        hCost = node.hCost;
        fCost = node.fCost;

        if(gText != null)
            gText.text = "" + gCost;
        if (hText != null)
            hText.text = "" + hCost;
        if (fText != null)
            fText.text = "" + fCost;
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
            case 6:
                type = tileType.Path;
                sprRen.sprite = path;
                break;
        }
    }

    public void AddTile()
    {
        thisTile = this;

        tile = this.transform.gameObject;

        if (boxCol != null)
        {
            boxCol = tile.GetComponent<BoxCollider2D>();
        }
        else
        {
            boxCol = tile.AddComponent<BoxCollider2D>();
        }

        if (sprRen != null)
        {
            sprRen = tile.GetComponent<SpriteRenderer>();
        }
        else
        {
            sprRen = tile.AddComponent<SpriteRenderer>();
        }

        empty = Resources.Load<Sprite>("Sprites/emptyTile");
        open = Resources.Load<Sprite>("Sprites/openTile");
        locked = Resources.Load<Sprite>("Sprites/lockedTile");
        wall = Resources.Load<Sprite>("Sprites/wall");
        target = Resources.Load<Sprite>("Sprites/target");
        path = Resources.Load<Sprite>("Sprites/pathTile");
    }
}
