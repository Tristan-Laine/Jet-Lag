using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool checkpointsEnabled = true;

    public void PlayGame()
    {
        SceneManager.LoadScene("Planet");
    }

    public void StartWithCheckpoints()
    {
        checkpointsEnabled = true;
        Debug.Log("Checkpoint activated !  " + checkpointsEnabled);
        SceneManager.LoadScene("Planet"); 
    }

    public void StartWithoutCheckpoints()
    {
        checkpointsEnabled = false;
        Debug.Log("Checkpoint NO activated  " + checkpointsEnabled);
        SceneManager.LoadScene("Planet");
    }

    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

