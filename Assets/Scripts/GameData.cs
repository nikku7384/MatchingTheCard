using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int score;
    public List<CardState> cardStates;  // Add a list to store each card's state

    public GameData(int score, List<CardState> cardStates)
    {
        this.score = score;
        this.cardStates = cardStates;
    }
}

[System.Serializable]
public class CardState
{
    public int cardID;
    public bool isFlipped;
    public bool isMatched;
    public Vector3 position; // If cards have a specific position, you can store this as well

    public CardState(int cardID, bool isFlipped,bool isMatched ,Vector3 position)
    {
        this.cardID = cardID;
        this.isFlipped = isFlipped;
        this.isMatched = isMatched;
        this.position = position;
    }
}
