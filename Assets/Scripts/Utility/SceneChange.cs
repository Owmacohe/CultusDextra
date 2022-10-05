using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public static void StaticChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void Change(string sceneName)
    {
        StaticChange(sceneName);
    }
}