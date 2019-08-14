using System.Collections.Generic;
using UnityEngine;
using static TurnOrder;

public class WorldController : MonoBehaviour
{
    
    //Blocks
    private static GameObject block;
    private static GameObject world;
    private static GameObject player;
    private static List<Enemy> enemies;
    private static Dictionary<(int, int), GameObject> tiles;
    private static Dictionary<(int, int), GameObject> floorTiles;
    private static Dictionary<Vector2Int, int> distanceFromPlayer;
    private static Queue<Vector2Int> nextTile;
    
    //Initialize
    private void Awake()
    {
        tiles = new Dictionary<(int, int), GameObject>();
        floorTiles = new Dictionary<(int, int), GameObject>();
        distanceFromPlayer = new Dictionary<Vector2Int, int>();
        nextTile = new Queue<Vector2Int>();

        player = GameObject.Find("Player");
        world = GameObject.Find("World");
        block = (GameObject)Resources.Load("Prefab/Other/Block", typeof(GameObject));

        enemies = new List<Enemy>();
        enemies.Add(Resources.Load<GameObject>("Prefab/Slime").GetComponent<Enemy>());
        enemies.Add(Resources.Load<GameObject>("Prefab/Spirit").GetComponent<Enemy>());

        GetComponent<WorldGen>().GenerateWorld();
    }

    public static void SetDistanceFromPlayer()
    {
        nextTile.Clear();
        distanceFromPlayer.Clear();
        Vector2Int playerPosition = player.GetComponent<Entity>().PlayerPosition();
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
                if (GetTile(tile) != null && GetTile(tile).GetComponent<Block>() != null) continue;
                //if (tiles.ContainsKey((tile.x, tile.y))) continue;
                if (!floorTiles.ContainsKey((tile.x, tile.y))) continue;
                distanceFromPlayer[tile] = dist + 1;
                nextTile.Enqueue(tile);
            }
        }
    }

    public static int GetDistanceFromPlayer(Vector2Int currTile)
    {
        if (distanceFromPlayer.ContainsKey(currTile) == false) return -1;
        return distanceFromPlayer[currTile];
    }

    public static Vector2Int FarthestTileFromPlayer(Vector2Int currTile)
    {
        Vector2Int[] adjacent = GetAdjacent(currTile);
        Vector2Int farthest = currTile;
        int farthestDist = distanceFromPlayer[currTile];
        foreach (Vector2Int tile in adjacent)
        {
            if (!distanceFromPlayer.ContainsKey(tile)) continue;
            if (tiles.ContainsKey((tile.x, tile.y))) continue;
            if (!floorTiles.ContainsKey((tile.x, tile.y))) continue;
            if (farthestDist < distanceFromPlayer[tile])
            {
                farthest = tile;
                farthestDist = distanceFromPlayer[tile];
            }
            if (farthestDist == distanceFromPlayer[tile] && farthest != currTile)
            {
                if (PlayerPosition().x == tile.x || PlayerPosition().y == tile.y) farthest = tile;
            }
        }
        return farthest;
    }

    public static Vector2Int GetClosestTileToPlayer(Vector2Int currTile)
    {
        Vector2Int[] adjacent = GetAdjacent(currTile);
        Vector2Int closest = currTile;
        int closestDist = distanceFromPlayer[currTile];
        foreach(Vector2Int tile in adjacent)
        {
            if (!distanceFromPlayer.ContainsKey(tile)) continue;
            if (tiles.ContainsKey((tile.x, tile.y))) continue;
            if (!floorTiles.ContainsKey((tile.x, tile.y))) continue;
            if (closestDist > distanceFromPlayer[tile])
            {
                closest = tile;
                closestDist = distanceFromPlayer[tile];
            }
            if (closestDist == distanceFromPlayer[tile] && closest != currTile)
            {
                if (PlayerPosition().x == tile.x || PlayerPosition().y == tile.y) closest = tile;
            }
        }
        return closest;
    }

    protected static Vector2Int PlayerPosition()
    {
        return new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
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
        int index = Random.Range(0, enemies.Count);
        Instantiate(enemies[index].gameObject, new Vector2(x, y), Quaternion.identity);
    }

    //Block methods
    public static void Kill(GameObject curr)
    {
        RemoveFromTurn(curr);
        tiles.Remove(((int)curr.transform.position.x, (int)curr.transform.position.y));
        Destroy(curr);
    }

    public static void MoveWorldLocation(Transform curr, int xMove, int yMove)
    {
        int x = (int)curr.position.x;
        int y = (int)curr.position.y;
        if (!Empty(x + xMove, y + yMove)) return;
        curr.position = new Vector2(x + xMove, y + yMove);
        tiles[(x + xMove, y + yMove)] = curr.gameObject;
        tiles.Remove((x, y));
    }

    public static void MoveToWorldPoint(Transform curr, Vector2Int newLocation)
    {
        if (!Empty(newLocation.x, newLocation.y)) return;
        tiles.Remove(((int)curr.transform.position.x, (int)curr.transform.position.y));
        curr.position = new Vector2(newLocation.x, newLocation.y);
        tiles[(newLocation.x, newLocation.y)] = curr.gameObject;
    }

    public static void AddToWorld(GameObject curr, int x, int y)
    {
        if (!tiles.ContainsKey((x, y)))
        {
            tiles[(x, y)] = curr;
        }
    }

    public static void Enable(int x, int y)
    {
        if (tiles.ContainsKey((x, y)))
        {
            tiles[(x, y)].SetActive(true);
        }
        if (floorTiles.ContainsKey((x,y)))
        {
            floorTiles[(x, y)].SetActive(true);
        }
    }

    public static void Disable(int x, int y)
    {
        if (tiles.ContainsKey((x, y)))
        {
            tiles[(x, y)].SetActive(false);
        }
        if (floorTiles.ContainsKey((x, y)))
        {
            floorTiles[(x, y)].SetActive(false);
        }
    }

    public static bool Create(int x, int y, Sprite blockSprite, Vector3? rotation = null)
    {
        if (!tiles.ContainsKey((x, y)))
        {
            tiles[(x, y)] = block.GetComponent<Block>().Create(x, y, world.transform, blockSprite);
            
            if (rotation != null)
            {
                Vector3 blockRotation = tiles[(x, y)].transform.rotation.eulerAngles;
                blockRotation.z = rotation.Value.z;
                tiles[(x, y)].transform.rotation = Quaternion.Euler((Vector3)rotation);
            }
            //if(!level) blocks[(x, y)].SetActive(false);
            return true;
        }
        else return false;
    }

    public static bool CreateBackground(int x, int y, Sprite blockSprite)
    {
        if (!floorTiles.ContainsKey((x, y)))
        {
            floorTiles[(x, y)] = block.GetComponent<Block>().Create(x, y, world.transform, blockSprite);
            floorTiles[(x, y)].GetComponent<Block>().background = true;
            floorTiles[(x, y)].GetComponent<BoxCollider2D>().isTrigger = true;
            floorTiles[(x, y)].GetComponent<SpriteRenderer>().color = new Color32(120, 120, 120, 255);
            floorTiles[(x, y)].GetComponent<SpriteRenderer>().sortingOrder = -1;
            //if (!level) backBlocks[(x, y)].SetActive(false);
            return true;
        }
        else return false;
    }

    public static GameObject GetGround(int x, int y)
    {
        if (floorTiles.ContainsKey((x, y)))
        {
            return floorTiles[(x, y)];
        }
        else return null;
    }

    public static bool Place(int x, int y, GameObject b)
    {
        if (!tiles.ContainsKey((x, y)))
        {
            tiles[(x, y)] = b.GetComponent<Block>().Place(x, y);
            tiles[(x, y)].SetActive(true);
            return true;
        }
        else return false;
    }

    public static GameObject PickUp(int x, int y)
    {
        if (tiles.ContainsKey((x, y)))
        {
            GameObject b = tiles[(x, y)];
            tiles.Remove((x, y));
            b.GetComponent<Block>().PickUp();
            return b;
        }
        else return null;
    }

    public static GameObject Get(int x, int y)
    {
        if (tiles.ContainsKey((x, y)))
        {
            return tiles[(x, y)];
        }
        else return null;
    }

    public static bool Empty(int x, int y)
    {
        if (tiles.ContainsKey((x, y)))
        {
            return false;
        }
        else return true;
    }

    public static GameObject GetTile(Vector2Int position)
    {
        if (tiles.ContainsKey((position.x, position.y))) return tiles[(position.x, position.y)];
        return null;
    }

    public static GameObject GetGround(Vector2Int position)
    {
        if (floorTiles.ContainsKey((position.x, position.y))) return floorTiles[(position.x, position.y)];
        return null;
    }

    public static List<GameObject> GetAll(Vector2Int position)
    {
        List<GameObject> result = new List<GameObject>();
        if (tiles.ContainsKey((position.x, position.y))) result.Add(tiles[(position.x, position.y)]);
        if (floorTiles.ContainsKey((position.x, position.y))) result.Add(floorTiles[(position.x, position.y)]);
        return result;
    }
}
