using UnityEngine;
using UnityEngine.UI;

public class CardHolderHandler : BaseCard
{
    
    public override void OnCardClicked()
    {
        cardAnim.SetTrigger("ForFront");
        if (isFlipped)
            return;

        // Flip the card visually
        FlipCard();

        // Notify the GameController
        gameController.OnCardSelected(this);

    }

    
    public override void FlipCard()
    {
        // Here you can put the animation logic for flipping the card
        // For example, using an animation or changing the sprite
        isFlipped = !isFlipped;
        cardAnim.SetTrigger("ForBackt");

        Debug.Log($"Card {cardID} flipped. IsFlipped: {isFlipped}");
        
    }
}
