using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }
    
    private static Scene nextLoadingScene;

    public static void Load(Scene targetScene)
    {
        Loader.nextLoadingScene = targetScene;
        
        // Show loading scene before loading another scene
        // When loading scene is loaded, it will invoke LoaderCallback() in the first frame to load desired scene
        // It works because the loading scene is simple so it won't freeze (load assets, prefabs etc.)
        // After the load scene is loaded, it will immediately load the target scene, but since the target scene is complex
        // and can cause the game to freeze, WE ARE FREEZING WHEN SHOWING THE LOADING SCENE
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(Loader.nextLoadingScene.ToString());
    }
}
