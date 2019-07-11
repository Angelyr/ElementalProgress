using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class Block : MonoBehaviour
{
    public Sprite top;
    public Sprite bottom;
    public Sprite grass;
    public bool background = false;

    private SpriteRenderer spriteComponent;

    private void Awake()
    {
        spriteComponent = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(!background) UpdateSprite();
    }

    public GameObject Create(int x, int y, Transform parent, Sprite newSprite)
    {
        GameObject newBlock = Instantiate(gameObject, new Vector2(x, y), Quaternion.identity, parent);
        newBlock.GetComponent<SpriteRenderer>().sprite = newSprite;
        newBlock.GetComponent<Block>().top = newSprite;
        newBlock.GetComponent<Block>().bottom = newSprite;

        if (newSprite.name == "Dirt") newBlock.GetComponent<Block>().top = grass;

        return newBlock;
    }

    public GameObject Place(int x, int y)
    {
        transform.position = new Vector2(x, y);
        return gameObject;
    }

    public void PickUp()
    {
        gameObject.SetActive(false);
    }

    private void UpdateSprite()
    {
        bool up = GetDirection(0, 1);

        if (up && spriteComponent.sprite != bottom) spriteComponent.sprite = bottom;
        if (!up && spriteComponent.sprite != top)   spriteComponent.sprite = top;
    }

    private GameObject GetDirection(int x, int y)
    {
        x = x + Mathf.RoundToInt(transform.position.x);
        y = y + Mathf.RoundToInt(transform.position.y);
        return Get(x, y);
    }

}
