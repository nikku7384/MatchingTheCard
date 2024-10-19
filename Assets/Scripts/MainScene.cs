using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using System.Collections;

public class MainScene : MonoBehaviour
{
    private  string filePath = "/savefile.json";
    [SerializeField]
    private GameObject noSavedFilePopup;

    public void NewGame()
    {
        SoundManager.Instance.PlaySound("ButtonClickSound");
        // If there's an existing save file, delete it to start a fresh game
        if (File.Exists(Application.persistentDataPath +filePath))
        {
            File.Delete(filePath);
            Debug.Log("Save file deleted. Starting a new game.");
        }
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void LoadGame()
    {
        SoundManager.Instance.PlaySound("ButtonClickSound");
        if (File.Exists(Application.persistentDataPath + filePath))
        {
            SceneManager.LoadSceneAsync("GameScene");
        }
        else
        {
            StartCoroutine(LoadNoFilePopup());
        }
    }

    private IEnumerator LoadNoFilePopup()
    {
        noSavedFilePopup.SetActive(true);
        yield return new WaitForSeconds(3);

        noSavedFilePopup.SetActive(false);
    }
}
