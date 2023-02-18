using UnityEngine;
using UnityEngine.SceneManagement;

namespace WargameSystem.MenuSystem
{
    public class MainUIManager : MonoBehaviour
    {
        public void ChangeScene(string name)
        {
            SceneManager.LoadScene(name);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
