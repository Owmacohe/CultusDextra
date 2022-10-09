using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    bool loadOnStart;
    [SerializeField]
    string scene;

    void Start()
    {
        if (loadOnStart)
        {
            StaticChange(scene);
        }
    }

    public static void StaticChange(string sceneName)
    {
        Debug.Log("<b>SCENE:</b> change");
        
        SceneManager.LoadScene(sceneName);
    }
    
    public void Change(string sceneName)
    {
        StaticChange(sceneName);
    }

    public void Exit()
    {
        Application.Quit(0);
    }
}