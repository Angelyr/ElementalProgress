using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    
    //Blocks
    private static GameObject block;
    private static GameObject world;
    private static GameObject player;
    private static Dictionary<(int, int), GameObject> blocks;
    private static Dictionary<(int, int), GameObject> backBlocks;
    private static List<GameObject> turnOrder;
    
    //Initialize
    private void Awake()
    {
        blocks = new Dictionary<(int, int), GameObject>();
        backBlocks = new Dictionary<(int, int), GameObject>();
        turnOrder = new List<GameObject>();

        player = GameObject.Find("Player");
        world = GameObject.Find("World");
        block = (GameObject)Resources.Load("Prefab/Other/Block", typeof(GameObject));
    }

    //Turn Methods
    public static void AddTurn(GameObject newTurn)
    {
        turnOrder.Add(newTurn);
        if (turnOrder.Count == 1) turnOrder[0].GetComponent<Character>().StartTurn();
    }

    public static void EndTurn(GameObject currTurn)
    {
        if (turnOrder[0] != currTurn) return;
        GameObject currentTurn = turnOrder[0];
        turnOrder.RemoveAt(0);
        turnOrder.Add(currentTurn);
        turnOrder[0].GetComponent<Character>().StartTurn();
    }

    public static void RemoveFromTurn(GameObject curr)
    {
        if (turnOrder[0] == curr) EndTurn(curr);
        turnOrder.Remove(curr);
    }

    public static bool InfinteTurn()
    {
        if (turnOrder.Count == 1) return true;
        else return false;
    }

    public void EndTurnButton()
    {
        EndTurn(player);
    }

    //Block methods
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
            backBlocks[(x, y)].GetComponent<BoxCollider2D>().enabled = false;
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
