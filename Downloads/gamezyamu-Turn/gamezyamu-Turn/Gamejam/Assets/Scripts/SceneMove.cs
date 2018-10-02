using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour {

    [SerializeField]
    private string sceneName;

   void Update()
    {
        if (Input.anyKeyDown)
        {
            Loading.Instance.LoadingStart(sceneName);
            SoundManager.SoundInstance.PlaySE(SoundList.GetSoundString(SoundType.FixButton), false, false);
        }
    }
}
