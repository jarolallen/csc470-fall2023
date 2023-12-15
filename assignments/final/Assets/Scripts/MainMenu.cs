using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayGame ()
    {
        SceneManager.LoadScene("Game");
    }
    public void StopGame ()
    {
        Debug.Log("Game Quit");
        Application.Quit();

    }
    public void LoadMainMenu ()
    {
        SceneManager.LoadScene("Start");
    }
}
