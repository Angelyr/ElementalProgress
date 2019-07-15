using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using static TurnOrder;

public class WorldController : MonoBehaviour
{
    
    //Blocks
    private static GameObject block;
    private static GameObject world;
    private static GameObject player;
    private static GameObject slime;
    private static Dictionary<(int, int), GameObject> blocks;
    private static Dictionary<(int, int), GameObject> backBlocks;
    
    //Initialize
    private void Awake()
    {
        blocks = new Dictionary<(int, int), GameObject>();
        backBlocks = new Dictionary<(int, int), GameObject>();

        player = GameObject.Find("Player");
        world = GameObject.Find("World");
        block = (GameObject)Resources.Load("Prefab/Other/Block", typeof(GameObject));
        slime = (GameObject)Resources.Load("Prefab/Slime");
    }

    //Enemies
    private static int spawnTimer = 3;
    private static int maxTimer = 3;
    public static void SpawnEnemies()
    {
        spawnTimer -= 1;
        if (spawnTimer < 1)
        {
            Instantiate(slime);
            spawnTimer = maxTimer;
        }
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
