using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    public Sprite top;
    public Sprite bottom;
    public Sprite grass;
    public bool background = false;

    private SpriteRenderer spriteComponent;

    protected override void Awake()
    {
        base.Awake();
        spriteComponent = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SpawnEnemyOnThisBlock();
    }

    private void SpawnEnemyOnThisBlock()
    {
        if (!background) return;
        if (WorldController.GetTile(MyPosition()) != null) return;
        int spawnChance = 1;
        if (Random.Range(0,100) < spawnChance)
        {
            WorldController.SpawnEnemy((int)transform.position.x, (int)transform.position.y);
        }
    }

    public GameObject Create(int x, int y, Transform parent, Sprite newSprite)
    {
        GameObject newBlock = Instantiate(gameObject, new Vector2(x, y), Quaternion.identity, parent);
        newBlock.GetComponent<SpriteRenderer>().sprite = newSprite;
        newBlock.GetComponent<Block>().top = newSprite;
        newBlock.GetComponent<Block>().bottom = newSprite;

        if (newSprite && newSprite.name == "Dirt") newBlock.GetComponent<Block>().top = grass;

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

    public override void CreateHover()
    {
    }

    public override void DestroyHover()
    {
    }

    private GameObject GetDirection(int x, int y)
    {
        x = x + Mathf.RoundToInt(transform.position.x);
        y = y + Mathf.RoundToInt(transform.position.y);
        return WorldController.Get(x, y);
    }

}
