using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ScreenFader))]
public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;

    public const string StartScene = "Scenes/Start";
    public const string Game = "Scenes/Game";

    public static bool LoadingScene;
    private ScreenFader _screenFader;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _screenFader = GetComponent<ScreenFader>();
    }
    
    public void Load(string scene, Action then = null)
    {
        if (LoadingScene) return;
        LoadingScene = true;

        PlayerPrefs.Save();
        _screenFader.FadeIn(ScreenFader.DefaultDuration,
            () => StartCoroutine(LoadScene(scene,then)));
    }

    private IEnumerator LoadScene(string scene, Action then)
    {
        var sceneAsync = SceneManager.LoadSceneAsync(scene);
        sceneAsync.allowSceneActivation = false;
        while (!sceneAsync.isDone && sceneAsync.progress < 0.9f)
            yield return null;

        sceneAsync.allowSceneActivation = true;

        if (then != null)
            then();
        
        _screenFader.FadeOut(ScreenFader.DefaultDuration, () => { LoadingScene = false; });
    }
}