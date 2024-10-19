using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameManager gameManager; // Prefab for the card
    public int rows = 4;                  // Default rows (e.g., 4)
    public int cols = 4;                  // Default columns (e.g., 4)
    public float spacing = 10f;           // Spacing between cards

    private RectTransform gridRectTransform;
    private GridLayoutGroup gridLayoutGroup;

    private void Start()
    {
        gridRectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        CalculateGridLayout();

        string filePath = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(filePath))
        {
            Debug.Log("Save file found, loading the game.");
            LoadGameFromFile(filePath); // Call the GameManager's load method
        }
        else
        {
            Debug.Log("No save file found, generating new game.");
            GenerateGrid();
        }
    }

    // Function to calculate grid layout based on screen size
    private void CalculateGridLayout()
    {
        // Get the available width and height for the grid (based on the parent canvas)
        float gridWidth = gridRectTransform.rect.width;
        float gridHeight = gridRectTransform.rect.height;

        // Calculate the maximum width and height for a single card
        float maxCardWidth = (gridWidth - (spacing * (cols - 1))) / cols;
        float maxCardHeight = (gridHeight - (spacing * (rows - 1))) / rows;

        // Take the smaller value to make sure cards fit within the screen
        float cardSize = Mathf.Min(maxCardWidth, maxCardHeight);

        // Apply the calculated card size and spacing to the GridLayoutGroup
        gridLayoutGroup.cellSize = new Vector2(cardSize, cardSize);
        gridLayoutGroup.spacing = new Vector2(spacing, spacing);
    }

    // Function to generate the card grid
    public void GenerateGrid()
    {
        // Calculate total number of cards
        int totalCards = rows * cols;

        // Ensure we have an even number of cards for matching pairs
        if (totalCards % 2 != 0)
        {
            Debug.LogError("Total cards must be even!");
            return;
        }

        // Generate card IDs and shuffle
        List<int> cardIDs = new List<int>();
        for (int i = 0; i < totalCards / 2; i++)
        {
            cardIDs.Add(i);
            cardIDs.Add(i);
        }
        Shuffle(cardIDs);

        // Instantiate cards
        for (int i = 0; i < totalCards; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, transform);
            BaseCard cardManager = newCard.GetComponent<BaseCard>();
            cardManager.Initialize(cardIDs[i], gameManager);
            gameManager.RegisterCard(cardManager);
        }
    }

    // Load the saved game state from a JSON file
    public void LoadGameFromFile(string filePath)
    {

        string json = File.ReadAllText(filePath);
        GameData loadedData = JsonUtility.FromJson<GameData>(json);

        gameManager.UpdateScoreText(loadedData.score);
        foreach (CardState cardData in loadedData.cardStates)
        {
            
            GameObject newCard = Instantiate(cardPrefab, transform); 
            BaseCard cardManager = newCard.GetComponent<BaseCard>();

            cardManager.Initialize(cardData.cardID, gameManager); 

            if (cardData.isFlipped && !cardManager.IsFlipped)
            {
                cardManager.FlipCard(); 
            }
            if (cardData.isMatched)
            {
                cardManager.SetMatched(); 
            }

            cardManager.transform.position = cardData.position;
            gameManager.RegisterCard(cardManager);
        }
    }

    /// <summary>
    /// For Shuffling the ids 
    /// </summary>
    /// <param name="list"></param>
    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
