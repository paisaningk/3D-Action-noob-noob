using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuUIManager : MonoBehaviour
    {
        public void ButtonStart()
        {
            SceneManager.LoadScene("Scenes/Gameplay");
        }

        public void ButtonQuit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }
}