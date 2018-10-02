using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

    AsyncOperation async;
    CanvasGroup canvasGroup;
    private string nextScene;
    public bool IsShow { get; private set; }
    public bool IsManualShow { get; set; }
    public bool IsLoading { get; private set; }

    public static Loading Instance { get; private set; }

    //シーンロードスタート変数関連
    private bool isLoadStart;
    private float timer;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Show(false);

    }

    

    public void LoadingStart(string sceneName)
    {
        nextScene = sceneName;
        IsLoading = true;
        isLoadStart = true;
        Show(true);
    }

    private void Update()
    {
        if (!IsLoading) { return; }

        //フェードイン
        if(isLoadStart == true)
        {
            timer += Time.deltaTime;
            if(timer >= 1)
            {
                timer = 1;
                canvasGroup.alpha = timer;
                isLoadStart = false;
                async = SceneManager.LoadSceneAsync(nextScene);
                async.allowSceneActivation = false;
                return;
            }
            canvasGroup.alpha = timer;
            return;
        }

        //シーン切り替え
        if(async.progress < 0.8f) { return; }
        async.allowSceneActivation = true;
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != nextScene) { return; }

        //フェードアウト
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 0;
            canvasGroup.alpha = timer;
            IsLoading = false;
            Show(false);
            return;
        }
        canvasGroup.alpha = timer;
    }
    
    public void Show(bool show)
    {
        GetComponent<Canvas>().enabled = show;
        IsShow = show;
    }
}
