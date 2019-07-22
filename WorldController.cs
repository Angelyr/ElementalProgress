using System.Collections.Generic;
using UnityEngine;
using static TurnOrder;

public class WorldController : MonoBehaviour
{
    
    //Blocks
    private static GameObject block;
    private static GameObject world;
    private static GameObject player;
    private static GameObject slime;
    public static Dictionary<(int, int), GameObject> blocks;
    public static Dictionary<(int, int), GameObject> backBlocks;
    public static Dictionary<Vector2Int, int> distanceFromPlayer;
    private static Queue<Vector2Int> nextTile;
    
    //Initialize
    private void Awake()
    {
        blocks = new Dictionary<(int, int), GameObject>();
        backBlocks = new Dictionary<(int, int), GameObject>();
        distanceFromPlayer = new Dictionary<Vector2Int, int>();
        nextTile = new Queue<Vector2Int>();

        player = GameObject.Find("Player");
        world = GameObject.Find("World");
        block = (GameObject)Resources.Load("Prefab/Other/Block", typeof(GameObject));
        slime = (GameObject)Resources.Load("Prefab/Slime");
    }

    public static void DistanceFromPlayer()
    {
        nextTile.Clear();
        distanceFromPlayer.Clear();
        Vector2Int playerPosition = player.GetComponent<Entity>().GetPlayerPosition();
        nextTile.Enqueue(playerPosition);
        distanceFromPlayer.Add(playerPosition, 0);

        while(nextTile.Count > 0)
        {
            Vector2Int currTile = nextTile.Dequeue();
            int dist = distanceFromPlayer[currTile];
            Vector2Int[] adjacent = GetAdjacent(currTile);
            foreach(Vector2Int tile in adjacent)
            {
                if (distanceFromPlayer.ContainsKey(tile)) continue;
                if (blocks.ContainsKey((tile.x, tile.y))) continue;
                if (!backBlocks.ContainsKey((tile.x, tile.y))) continue;
                distanceFromPlayer[tile] = dist + 1;
                nextTile.Enqueue(tile);
            }
        }
    }

    public static Vector2Int GetClosestTileToPlayer(Vector2Int currTile)
    {
        Vector2Int[] adjacent = GetAdjacent(currTile);
        Vector2Int closest = new Vector2Int(-1, -1);
        int shortestDist = -1;
        foreach(Vector2Int tile in adjacent)
        {
            if (!distanceFromPlayer.ContainsKey(tile)) continue;
            if (blocks.ContainsKey((tile.x, tile.y))) continue;
            if (!backBlocks.ContainsKey((tile.x, tile.y))) continue;
            if (shortestDist == -1 || shortestDist > distanceFromPlayer[tile])
            {
                closest = tile;
                shortestDist = distanceFromPlayer[tile];
            }
        }
        return closest;
    }

    public static Vector2Int[] GetAdjacent(Vector2Int currTile)
    {
        Vector2Int[] adjacent = new Vector2Int[8];
        adjacent[0] = new Vector2Int(currTile.x - 1, currTile.y + 1);
        adjacent[1] = new Vector2Int(currTile.x + 0, currTile.y + 1);
        adjacent[2] = new Vector2Int(currTile.x + 1, currTile.y + 1);

        adjacent[3] = new Vector2Int(currTile.x - 1, currTile.y + 0);
        adjacent[4] = new Vector2Int(currTile.x + 1, currTile.y + 0);

        adjacent[5] = new Vector2Int(currTile.x - 1, currTile.y - 1);
        adjacent[6] = new Vector2Int(currTile.x + 0, currTile.y - 1);
        adjacent[7] = new Vector2Int(currTile.x + 1, currTile.y - 1);
        return adjacent;
    }

    public static void SpawnEnemy(int x, int y)
    {
        Instantiate(slime, new Vector2(x, y), Quaternion.identity);
    }

    //Block methods
    public static void Kill(GameObject curr)
    {
        RemoveFromTurn(curr);
        blocks.Remove(((int)curr.transform.position.x, (int)curr.transform.position.y));
        Destroy(curr);
    }

    public static void MoveWorldLocation(Transform curr, int xMove, int yMove)
    {
        int x = (int)curr.position.x;
        int y = (int)curr.position.y;
        if (!Empty(x + xMove, y + yMove)) return;
        curr.position = new Vector2(x + xMove, y + yMove);
        blocks[(x + xMove, y + yMove)] = curr.gameObject;
        blocks.Remove((x, y));
    }

    public static void MoveToWorldPoint(Transform curr, Vector2Int newLocation)
    {
        if (!Empty(newLocation.x, newLocation.y)) return;
        blocks.Remove(((int)curr.transform.position.x, (int)curr.transform.position.y));
        curr.position = new Vector2(newLocation.x, newLocation.y);
        blocks[(newLocation.x, newLocation.y)] = curr.gameObject;
    }

    public static void AddToWorld(GameObject curr, int x, int y)
    {
        if (!blocks.ContainsKey((x, y)))
        {
            blocks[(x, y)] = curr;
        }
    }

    public static void Enable(int x, int y)
    {
        if (blocks.ContainsKey((x, y)))
        {
            blocks[(x, y)].SetActive(true);
        }
        if (backBlocks.ContainsKey((x,y)))
        {
            backBlocks[(x, y)].SetActive(true);
        }
    }

    public static void Disable(int x, int y)
    {
        if (blocks.ContainsKey((x, y)))
        {
            blocks[(x, y)].SetActive(false);
        }
        if (backBlocks.ContainsKey((x, y)))
        {
            backBlocks[(x, y)].SetActive(false);
        }
    }

    public static bool Create(int x, int y, Sprite blockSprite, Vector3? rotation = null)
    {
        if (!blocks.ContainsKey((x, y)))
        {
            blocks[(x, y)] = block.GetComponent<Block>().Create(x, y, world.transform, blockSprite);
            
            if (rotation != null)
            {
                Vector3 blockRotation = blocks[(x, y)].transform.rotation.eulerAngles;
                blockRotation.z = rotation.Value.z;
                blocks[(x, y)].transform.rotation = Quaternion.Euler((Vector3)rotation);
            }
            //if(!level) blocks[(x, y)].SetActive(false);
            return true;
        }
        else return false;
    }

    public static bool CreateBackground(int x, int y, Sprite blockSprite)
    {
        if (!backBlocks.ContainsKey((x, y)))
        {
            backBlocks[(x, y)] = block.GetComponent<Block>().Create(x, y, world.transform, blockSprite);
            backBlocks[(x, y)].GetComponent<Block>().background = true;
            backBlocks[(x, y)].GetComponent<BoxCollider2D>().isTrigger = true;
            backBlocks[(x, y)].GetComponent<SpriteRenderer>().color = new Color32(120, 120, 120, 255);
            backBlocks[(x, y)].GetComponent<SpriteRenderer>().sortingOrder = -1;
            //if (!level) backBlocks[(x, y)].SetActive(false);
            return true;
        }
        else return false;
    }

    public static GameObject GetGround(int x, int y)
    {
        if (backBlocks.ContainsKey((x, y)))
        {
            return backBlocks[(x, y)];
        }
        else return null;
    }

    public static bool Place(int x, int y, GameObject b)
    {
        if (!blocks.ContainsKey((x, y)))
        {
            blocks[(x, y)] = b.GetComponent<Block>().Place(x, y);
            blocks[(x, y)].SetActive(true);
            return true;
        }
        else return false;
    }

    public static GameObject PickUp(int x, int y)
    {
        if (blocks.ContainsKey((x, y)))
        {
            GameObject b = blocks[(x, y)];
            blocks.Remove((x, y));
            b.GetComponent<Block>().PickUp();
            return b;
        }
        else return null;
    }

    public static GameObject Get(int x, int y)
    {
        if (blocks.ContainsKey((x, y)))
        {
            return blocks[(x, y)];
        }
        else return null;
    }

    public static bool Empty(int x, int y)
    {
        if (blocks.ContainsKey((x, y)))
        {
            return false;
        }
        else return true;
    }

   
}
