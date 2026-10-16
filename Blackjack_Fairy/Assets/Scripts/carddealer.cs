using UnityEngine;

public class CardDealer : MonoBehaviour
{
    public Sprite[] cardSprites; // Array to hold all card images
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        DealRandomCard();
    }

    void DealRandomCard()
    {
        int randomIndex = Random.Range(0, cardSprites.Length);
        spriteRenderer.sprite = cardSprites[randomIndex]; // Assign a random card sprite
    }
}
