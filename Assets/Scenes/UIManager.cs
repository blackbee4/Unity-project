using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
