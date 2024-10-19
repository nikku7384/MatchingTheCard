using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private List<BaseCard> selectedCards = new List<BaseCard>(); // Track selected cards
    public int score = 0;
    public Text scoreText;
    public List<BaseCard> allCards = new List<BaseCard>();
    private string saveFilePath;
    public GameObject cardPrefab;
    public GameObject winPop;
    public List<Sprite> cardImages;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
    }
    // Called when a card is selected
    public void OnCardSelected(BaseCard selectedCard)
    {
        if (selectedCards.Contains(selectedCard) || selectedCards.Count >= 2)
        {
            return; // Ignore if already selected or more than 2 cards are selected
        }

        selectedCards.Add(selectedCard);

        if (selectedCards.Count == 2)
        {
            StartCoroutine(CheckForMatch());
        }
    }

    // Coroutine to check if the selected cards match
    private IEnumerator CheckForMatch()
    {
        yield return new WaitForSeconds(1f); // Add delay before checking

        if (selectedCards[0].cardID == selectedCards[1].cardID)
        {
            Debug.Log("Match found!");
            UpdateScore(10);
            selectedCards[0].SetMatched();
            selectedCards[1].SetMatched();
            CheckForWin();
            // Handle success logic here (e.g., remove or disable matched cards)
        }
        else
        {
            Debug.Log("No match.");
            selectedCards[0].ResetCard();
            selectedCards[1].ResetCard();
        }

        selectedCards.Clear();
    }

    private void UpdateScore(int value)
    {
        score += value;
        scoreText.text = "Score:- " + score.ToString();
    }

    public void UpdateScoreText(int value)
    {
        scoreText.text = "Score:- " + value.ToString();
        score = value;
    }

    public void RegisterCard(BaseCard card)
    {
        allCards.Add(card);
    }
    public void SaveGame()
    {
        List<CardState> cardStates = new List<CardState>();

        // Loop through the cached list of all cards
        foreach (BaseCard card in allCards)
        {
            CardState state = new CardState(
                card.cardID,            // Store card ID
                card.IsFlipped,
                card.IsMatched,
                card.transform.position // Store its position
            );
            cardStates.Add(state);
        }

        // Create a GameData object with the score and card states
        GameData gameData = new GameData(score, cardStates);

        // Convert the GameData object to JSON and save it to a file
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("Game Saved: " + json);
    }

    // Load the saved game state from a JSON file
    public void LoadGameFromFile(string filePath)
    {
        // Read save data from file
        string json = File.ReadAllText(filePath);
        GameData loadedData = JsonUtility.FromJson<GameData>(json);

        // Set the score from the loaded data
        scoreText.text = loadedData.score.ToString();
         // Update the score UI

        // Generate cards from saved data
        foreach (CardState cardData in loadedData.cardStates)
        {
            // Instantiate the card and set its state
            GameObject newCard = Instantiate(cardPrefab, transform); // Assuming you have a reference to the prefab in GameManager
            BaseCard cardManager = newCard.GetComponent<BaseCard>();

            cardManager.Initialize(cardData.cardID, this); // Initialize card with its ID and GameManager

            // Restore card's flipped state
            if (cardData.isFlipped && !cardManager.IsFlipped)
            {
                cardManager.FlipCard(); // Flip if it was saved as flipped but is currently not flipped
            }

            // Restore card's position
            cardManager.transform.position = cardData.position;

            // Register the card in the game manager
            RegisterCard(cardManager);
        }
    }

    public BaseCard GetCardById(int cardID)
    {
        foreach (BaseCard card in allCards)
        {
            if (card.cardID == cardID)
            {
                return card;
            }
        }

        // If no card with the given ID is found, return null
        return null;
    }

    public void NewGame()
    {
        string filePath = Application.persistentDataPath + "/savefile.json";

        // If there's an existing save file, delete it to start a fresh game
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Save file deleted. Starting a new game.");
        }

        // Reload the scene to start a fresh game
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void CheckForWin()
    {
        foreach (BaseCard card in allCards)
        {
            if (!card.IsMatched)
            {
                // If any card is not matched, the game is still ongoing
                return;
            }
        }

        // If all cards are matched, trigger the win popup
        ShowWinPopup();
    }

    private void ShowWinPopup()
    {
        winPop.SetActive(true); // Assuming you have a Win popup reference
                                  // Optionally, you can pause the game or stop further interaction
    }

    public Sprite GetCardImageById(int cardID)
    {
        // Ensure the cardID is within bounds
        if (cardID >= 0 && cardID < cardImages.Count)
        {
            return cardImages[cardID];
        }
        else
        {
            Debug.LogError("Invalid card ID: " + cardID);
            return null;
        }
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

}

