using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;      // Drag the PauseMenu panel here in the Inspector
    private bool isPaused = false;
    [SerializeField]
    private GameManager gameManager;    // Cache the GameManager reference

    private void Start()
    {
       
    }

    private void Update()
    {
        // You can also use the Escape key to pause/unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void OnPauseButtonClicked()
    {
        PauseGame();
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);  // Show the Pause Menu
        Time.timeScale = 0f;          // Freeze the game
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide the Pause Menu
        Time.timeScale = 1f;          // Resume the game
        isPaused = false;
    }

    public void SaveGame()
    {
        if (gameManager != null)
        {
            gameManager.SaveGame();
            Debug.Log("Game Saved");
        }
        ResumeGame();
    }

    public void LoadGame()
    {
        if (gameManager != null)
        {
            gameManager.NewGame();
            Debug.Log("Game Loaded");
        }
        ResumeGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the Game");
        Application.Quit(); // Quits the game
    }
}
