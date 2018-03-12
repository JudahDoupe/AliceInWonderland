using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour {

    public static Scene CurrentScene;
    public Scene StartingScene;
    public Scene EndingScene;

    void Start()
    {
        CurrentScene = StartingScene;
        ChangeScene(StartingScene);
    }

    public static void ChangeScene(Scene newScene)
    {
        if (newScene != null && newScene.Construct(CurrentScene.name))
        {
            CurrentScene.Destroy(newScene.SceneName);
            CurrentScene = newScene;
        }
    }
}
