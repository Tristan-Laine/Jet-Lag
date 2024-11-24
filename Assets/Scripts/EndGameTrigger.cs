using UnityEngine;
using UnityEngine.SceneManagement;


public class EndGameTrigger : MonoBehaviour
{
    public GameObject endGameCanvas; 
    public GameTimer gameTimer;

    void Start()
    {
        if (endGameCanvas != null)
        {
            endGameCanvas.SetActive(false);
        }
        gameTimer.StartTimer();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            ShowEndGameCanvas();
        }
    }

    void ShowEndGameCanvas()
    {
        gameTimer.StopTimer();
        endGameCanvas.SetActive(true); 
        Debug.Log("End of the game!");
        Time.timeScale = 0f;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitter le jeu !");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
