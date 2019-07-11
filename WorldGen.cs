using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static WorldController;

public class WorldGen : MonoBehaviour
{

    //Chunks
    private Dictionary<(int, int), string> chunks;
    public Grid caveEntrance;
    public Grid full;
    public Grid[] topSoil;
    public Grid[] allsides;
    public Grid[] leftright;
    public Grid[] leftrighttop;
    public Grid[] leftrightbottom;
    public Grid[] special;
    private const int numChunks = 8;
    private const int chunksize = 16;

    //Level
    private bool level = true;
    private Tilemap myLevel;

    //Backgrounds
    private Dictionary<(int, int), GameObject> backgrounds;
    public Sprite sky;
    public GameObject backgroundParent;
    public Sprite stonebackground;
    public GameObject background;

    private void Awake()
    {
        myLevel = GameObject.Find("Level").GetComponent<Tilemap>();    
    }

    //Start
    void Start()
    {
        if (!level)
        {
            chunks = new Dictionary<(int, int), string>();
            backgrounds = new Dictionary<(int, int), GameObject>();
            ChunkGeneration();
            CreateBackgrounds();
        }

        if (level)
        {
            CreateLevel();
        }
    }

    //Level methods
    public void CreateLevel()
    {
        myLevel.gameObject.SetActive(false);
        BoundsInt bounds = myLevel.cellBounds;

        for (int x = -bounds.size.x; x < bounds.size.x; x++)
        {
            for (int y = -bounds.size.y; y < bounds.size.y; y++)
            {
                TileBase currTile = myLevel.GetTile(new Vector3Int(x, y, 0));
                Sprite tileSprite = myLevel.GetSprite(new Vector3Int(x, y, 0));
                Vector3 rotation = myLevel.GetTransformMatrix(new Vector3Int(x, y, 0)).rotation.eulerAngles;

                if (currTile)
                {
                    if (currTile.name == "Background")
                    {
                        CreateBackground(x, y, tileSprite);
                    }
                    else
                    {
                        Create(x, y, tileSprite, rotation);
                    }
                }
            }
        }
    }

    //Chunk methods
    //================================================================

    private void ChunkGeneration()
    {
        for (int chunkY = 1; chunkY > -numChunks; chunkY--)
        {
            for (int chunkX = -numChunks; chunkX < numChunks; chunkX++)
            {
                Grid chunkGrid = PickChunk(chunkX, chunkY);
                PlaceChunk(chunkX, chunkY, chunkGrid);
                if (chunkGrid.name == "CaveEntrance") PlaceChunkPath(chunkX, chunkY - 1, "down");
            }
        }
    }

    private void PlaceChunk(int chunkX, int chunkY, Grid chunkGrid)
    {
        if (chunks.ContainsKey((chunkX, chunkY))) return;
        Tilemap chunk = chunkGrid.transform.GetChild(0).gameObject.GetComponent<Tilemap>();
        chunks[(chunkX, chunkY)] = chunkGrid.name;

        for (int x = -chunksize / 2; x < chunksize / 2; x++)
        {
            for (int y = 0; y > -chunksize; y--)
            {
                Tile currTile = (Tile)chunk.GetTile(new Vector3Int(x, y, 0));
                int currX = x + (chunkX * chunksize);
                int currY = y + (chunkY * chunksize);

                if (currTile)
                {
                    Create(currX, currY, currTile.sprite);
                }
            }
        }
    }

    private void PlaceChunkPath(int chunkx, int chunky, string prevDirection)
    {
        Grid chunkGrid = GetChunkType(chunkx, chunky, prevDirection);
        PlaceChunk(chunkx, chunky, chunkGrid);

        string newDirection = GetDirection(chunkx, chunky, chunkGrid);
        if (newDirection == "down") chunky -= 1;
        if (newDirection == "left") chunkx -= 1;
        if (newDirection == "right") chunkx += 1;
        if (newDirection != null) PlaceChunkPath(chunkx, chunky, newDirection);
    }

    private string GetDirection(int chunkx, int chunky, Grid chunkType)
    {
        bool leftChunk = chunks.ContainsKey((chunkx - 1, chunky));
        bool rightChunk = chunks.ContainsKey((chunkx + 1, chunky));
        int direction = -1; //null=-1, down=0, left=1, right=2, down=3
        //if chunk blocking
        bool canLeft = !leftChunk;
        bool canRight = !rightChunk;
        bool canDown = true;
        //if reached end of map
        if (Contains(leftrighttop, chunkType) || Contains(leftright, chunkType)) canDown = false;
        //if (chunkType == leftrighttop[0] || chunkType == leftright[0]) canDown = false;
        if (chunkx == -numChunks) canLeft = false;
        if (chunkx == numChunks - 1) canRight = false;
        if (chunky == -numChunks + 1) canDown = false;
        //get direction
        if (canLeft && canRight && canDown) direction = Random.Range(0, 3);
        if (!canLeft && canRight && canDown) direction = Random.Range(2, 4);
        if (canLeft && !canRight && canDown) direction = Random.Range(0, 2);
        if (canLeft && canRight && !canDown) direction = Random.Range(1, 3);
        if (canLeft && !canRight && !canDown) direction = 1;
        if (!canLeft && canRight && !canDown) direction = 2;
        if (!canLeft && !canRight && canDown) direction = 0;
        //return direction
        if (direction == 0) return "down";
        if (direction == 1) return "left";
        if (direction == 2) return "right";
        if (direction == 3) return "down";
        else return null;
    }

    private bool Contains(Grid[] rooms, Grid item)
    {
        foreach (Grid room in rooms)
        {
            if (room == item) return true;
        }
        return false;
    }

    private Grid GetChunkType(int chunkx, int chunky, string prevDirection)
    {
        bool canLeft = !chunks.ContainsKey((chunkx - 1, chunky));
        bool canRight = !chunks.ContainsKey((chunkx + 1, chunky));
        if (chunkx == -numChunks) canLeft = false;
        if (chunkx == numChunks - 1) canRight = false;

        int chunkType = 0; //all=0; LRT=1, LRB=2, LR=3
        if (prevDirection == "down") chunkType = Random.Range(0, 2);
        else if (!canLeft && !canRight) chunkType = Random.Range(0, 3);
        else chunkType = Random.Range(0, 4);
        if (!canLeft && !canRight && chunkType == 1) chunkType = 2;

        if (chunkType == 0) return RandomRoom(allsides);
        if (chunkType == 1) return RandomRoom(leftrighttop);
        if (chunkType == 2) return RandomRoom(leftrightbottom);
        if (chunkType == 3) return RandomRoom(leftright);
        else return null;
    }

    private Grid RandomRoom(Grid[] rooms)
    {
        int rand = Random.Range(0, rooms.Length);
        return rooms[rand];
    }

    private Grid PickChunk(int chunkX, int chunkY)
    {
        int chunkType = Random.Range(0, 5);

        if (chunkY == 1) return RandomRoom(topSoil);
        if (chunkX == 1 && chunkY == 0) return caveEntrance;
        if (chunkY == 0) return full;
        if (chunkType == 0) return RandomRoom(allsides);
        if (chunkType == 1) return RandomRoom(leftrighttop);
        if (chunkType == 2) return RandomRoom(leftrightbottom);
        if (chunkType == 3) return RandomRoom(leftright);
        if (chunkType == 4) return RandomRoom(special);

        return null;
    }


    //Background Methods 

    public void NewScale(GameObject theGameObject, float newSize)
    {
        float size = theGameObject.GetComponent<Renderer>().bounds.size.x;
        Vector3 rescale = theGameObject.transform.localScale;
        rescale.x = newSize * rescale.x / size;
        theGameObject.transform.localScale = rescale;
    }

    private void CreateBackgrounds()
    {
        for (int x = -numChunks * chunksize - chunksize / 2; x < (numChunks - 1) * chunksize; x++)
        {
            for (int y = numChunks * chunksize; y > -numChunks * chunksize; y--)
            {
                if (!backgrounds.ContainsKey((x, y)))
                {
                    int width = (int)background.GetComponent<SpriteRenderer>().bounds.size.x;
                    int height = (int)background.GetComponent<SpriteRenderer>().bounds.size.y;
                    Vector2 center = new Vector2(x - .5f + width / 2, y + .5f - height / 2);
                    GameObject newBackground = Instantiate(background, center, Quaternion.identity, backgroundParent.transform);

                    if (y >= 1)
                    {
                        newBackground.GetComponent<SpriteRenderer>().sprite = sky;
                    }
                    if (y < 1) newBackground.GetComponent<SpriteRenderer>().sprite = stonebackground;

                    for (int i = x; i < x + width; i++)
                    {
                        for (int j = y; j > y - height; j--)
                        {
                            backgrounds[(i, j)] = newBackground;
                        }
                    }

                }
            }
        }

    }

}
