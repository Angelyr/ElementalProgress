using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    public bool background = false;

    private SpriteRenderer spriteComponent;

    //MonoBehavoir

    protected override void Awake()
    {
        base.Awake();
        spriteComponent = gameObject.GetComponent<SpriteRenderer>();
        outline = transform.Find("OutlineCanvas").Find("Outline").gameObject;
    }

    //Private

    private void Start()
    {
        SpawnEnemyOnThisBlock();
        UpdateSprite();
        name = "";
    }

    private GameObject GetDirection(int x, int y)
    {
        x = x + Mathf.RoundToInt(transform.position.x);
        y = y + Mathf.RoundToInt(transform.position.y);
        return WorldController.Get(x, y);
    }

    private void SpawnEnemyOnThisBlock()
    {
        if (!background) return;
        if (WorldController.GetTile(MyPosition()) != null) return;
        
        if (Random.Range(0,100) < Settings.spawnChance)
        {
            WorldController.SpawnEnemy((int)transform.position.x, (int)transform.position.y);
        }
    }

    private void UpdateSprite()
    {
        if (Random.Range(0, 100) < Settings.variationChance)
        {
            spriteComponent.sprite = Variations.GetRandomVariation(spriteComponent.sprite);
        }
    }


    private void SetSortingOrder(int sortingOrder)
    {
        spriteComponent.sortingOrder = (int)-transform.position.y + sortingOrder;
        transform.GetChild(1).GetComponent<Canvas>().sortingOrder = spriteComponent.sortingOrder;
    }

    //Public

    public GameObject Create(int x, int y, Transform parent, Sprite newSprite, int sortingOrder = 0)
    {
        GameObject newBlock = Instantiate(gameObject, new Vector2(x, y), Quaternion.identity, parent);

        newBlock.GetComponent<Block>().SetSortingOrder(sortingOrder);
        newBlock.GetComponent<SpriteRenderer>().sprite = newSprite;

        if (newSprite && newSprite.name == "Barrier")
        {
            newBlock.GetComponent<SpriteRenderer>().enabled = false;
        }

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

    public void SteppedOn(GameObject target)
    {
        foreach (Effect effect in myEffects)
        {
            effect.SteppedOn(target);
        }
    }
}
