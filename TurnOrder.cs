using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class TurnOrder : MonoBehaviour
{
    private static List<GameObject> turnOrder;
    private GameObject player;
    private static GameObject turnUI;
    private static GameObject myUI;
    private static List<GameObject> concurrentTurns;

    private void Awake()
    {
        turnOrder = new List<GameObject>();
        concurrentTurns = new List<GameObject>();
        player = GameObject.Find("Player");
        turnUI = (GameObject)Resources.Load("Prefab/Turn");
        myUI = GameObject.Find("TurnOrder");
    }

    //Turn Methods
    public static void AddTurn(GameObject newTurn)
    {
        turnOrder.Add(newTurn);
        if (turnOrder.Count == 1) turnOrder[0].GetComponent<Character>().StartTurn();

        GameObject uiTurn = Instantiate(turnUI, myUI.transform);
        uiTurn.GetComponent<UnityEngine.UI.Image>().sprite = newTurn.GetComponent<SpriteRenderer>().sprite;
        newTurn.GetComponent<Character>().myTurnUI = uiTurn;
    }

    public static void EndTurn(GameObject currTurn)
    {
        if (turnOrder[0] != currTurn) return;
        GameObject currentTurn = turnOrder[0];
        turnOrder.RemoveAt(0);
        turnOrder.Add(currentTurn);
        turnOrder[0].GetComponent<Character>().StartTurn();

        myUI.transform.GetChild(0).SetAsLastSibling();

    }

    public static void StartTurn()
    {
        turnOrder[0].GetComponent<Character>().StartTurn();
    }

    public static bool MyTurn(GameObject character)
    {
        if (turnOrder[0] == character) return true;
        else return false;
    }

    public static void RemoveFromTurn(GameObject curr)
    {
        int index = turnOrder.IndexOf(curr);
        turnOrder.Remove(curr);
        Destroy(curr.GetComponent<Character>().myTurnUI);
        if (index == 0) StartTurn();
    }

    public static bool ConcurrentTurns()
    {
        if (turnOrder.Count == 1) return true;
        else return false;
    }

    public static void AddConcurrentTurn(GameObject newTurn)
    {
        concurrentTurns.Add(newTurn);
        if (concurrentTurns.Count == 1) concurrentTurns[0].GetComponent<Character>().StartConcurrentTurn();
    }

    public static void EndConcurrentTurn(GameObject currTurn)
    {
        if (concurrentTurns[0] != currTurn) return;
        GameObject currentTurn = concurrentTurns[0];
        concurrentTurns.RemoveAt(0);
        concurrentTurns.Add(currentTurn);
        concurrentTurns[0].GetComponent<Character>().StartConcurrentTurn();
            
    }

    public void EndTurnButton()
    {
        EndTurn(player);
    }
}
