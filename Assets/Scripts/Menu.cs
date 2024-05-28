using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); // Load your first level scene
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit(); // Quits the game
    }
}
