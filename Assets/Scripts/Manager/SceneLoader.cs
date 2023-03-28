using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    private void Awake()
    {
        Instance = this;
    }
    public int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public void LoadScene(int sceneIndex) 
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void QuitButtonPressed()
    {
        Application.Quit();
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(GetCurrentSceneIndex());
    }
}
