using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static WorldController;

public class WorldGen : MonoBehaviour
{
    //Chunks
    private Dictionary<(int, int), string> chunks;
    //public Grid[] topSoil;
    private Grid[] allsides;
    private Grid[] leftright;
    private Grid[] leftrighttop;
    private Grid[] leftrightbottom;
    private Grid[] special;
    private const int numChunks = 8;
    private const int chunksize = 16;

    //Level
    private bool level = true;
    private Tilemap myLevel;

    //Start
    void Start()
    {
        chunks = new Dictionary<(int, int), string>();
        allsides = GetGrid(Resources.LoadAll<GameObject>("Prefab/Dungeon/AllSides"));
        leftright = GetGrid(Resources.LoadAll<GameObject>("Prefab/Dungeon/LeftRight"));
        leftrighttop = GetGrid(Resources.LoadAll<GameObject>("Prefab/Dungeon/LeftRightTop"));
        leftrightbottom = GetGrid(Resources.LoadAll<GameObject>("Prefab/Dungeon/RightLeftBottom"));
        special = GetGrid(Resources.LoadAll<GameObject>("Prefab/Dungeon/Special"));

        ChunkGeneration();
        //CreateBackgrounds();
    }

    private Grid[] GetGrid(GameObject[] input)
    {
        //Debug.Log(input.Length);
        Grid[] output = new Grid[input.Length];
        for(int i=0; i<input.Length; i++)
        {
            output[i] = input[i].GetComponent<Grid>();
        }
        return output;
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
                if (chunkX == 1 && chunkY == 1) PlaceChunkPath(chunkX, chunkY - 1, "down");
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

        //if (chunkY == 1) return RandomRoom(topSoil);
        //if (chunkX == 1 && chunkY == 0) return caveEntrance;
        //if (chunkY == 0) return full;
        if (chunkType == 0) return RandomRoom(allsides);
        if (chunkType == 1) return RandomRoom(leftrighttop);
        if (chunkType == 2) return RandomRoom(leftrightbottom);
        if (chunkType == 3) return RandomRoom(leftright);
        if (chunkType == 4) return RandomRoom(special);

        return null;
    }

}
