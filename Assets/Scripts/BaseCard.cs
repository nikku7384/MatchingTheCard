using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseCard : MonoBehaviour
{
    public int cardID { get; protected set; }      // Unique ID for the car
    [SerializeField]
    protected Animator cardAnim;
    [SerializeField]
    private Image backImage;
    [SerializeField]
    private Image frontImage;
    [SerializeField]
    private Button button;
    protected bool isFlipped = false;              // Track if the card is flipped
    protected bool isMatched = false;
    protected GameManager gameController;       // Reference to the GameController

    // Set the GameController reference and cardID, common to all cards
    public virtual void Initialize(int id, GameManager controller)
    {
        cardID = id;
        gameController = controller;
        frontImage.sprite = gameController.GetCardImageById(cardID);
    }

    public bool IsFlipped
    {
        get { return isFlipped; }
    }

    public bool IsMatched
    {
        get { return isMatched; }
    }
    // Abstract function to handle what happens when a card is clicked
    public abstract void OnCardClicked();

    // Abstract function to flip the card, to be implemented by derived classes
    public abstract void FlipCard();

    // Reset the card if it doesn't match (for unmatched cards)
    public virtual void ResetCard()
    {
        
        FlipCard(); // Flip it back
    }

    public virtual void HideCard()
    {
        frontImage.GetComponent<CanvasRenderer>().SetAlpha(0);
        backImage.GetComponent<CanvasRenderer>().SetAlpha(0);
        button.interactable = false;
        cardAnim.enabled = false;
    }
    public virtual void SetMatched()
    {
        isMatched = true;  
        HideCard();        
    }
}
