using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField]
    bool loadOnStart;
    [SerializeField]
    string scene;
    [SerializeField]
    float bufferSeconds;

    void Start()
    {
        if (loadOnStart)
        {
            StaticChange(scene);
        }
    }

    public static void StaticChange(string sceneName)
    {
        Debug.Log("<b>SCENE:</b> change to " + sceneName);
        
        SceneManager.LoadScene(sceneName);
    }
    
    public void Change(string sceneName)
    {
        if (Time.time > bufferSeconds)
        {
            StaticChange(sceneName);
        }
    }

    public void Exit()
    {
        Debug.Log("<b>SCENE:</b> exit");
        
        Application.Quit(0);
    }
}