using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour {
    //キャラを置く場所 Set[0] プレイヤー1　Set[1] プレイヤー2
    GameObject[] Set = new GameObject[2];
    //プレイヤー1
    GameObject Player1;
    //プレイヤー2
    GameObject Player2;
    //シングルトンのプレイヤー読み込むやつ
    TurnControl TC;
    //winとloseのテキストイメージwin0 lose1
    [SerializeField]
    Sprite[] sp = new Sprite[2];
    //紙吹雪のプレハブを入れるやつ
    [SerializeField]
    private GameObject Effect;
    //紙吹雪のパーティクル
    private ParticleSystem particle;
    //プレイヤーが呼び込まれるまでの仮のprefab
    [SerializeField]
    GameObject[] testPrefab = new GameObject[2];
    // 表のCanvas
    Canvas cv;
    //アニメーター　0がプレイヤー1　1がプレイヤー2
    Animator[] anim = new Animator[2];
    //リザルト用のアニメーターコントローラー
    [SerializeField]
    RuntimeAnimatorController animControl;
    void Start()
    {
        TC = TurnControl.Instance;
        SoundManager.SoundInstance.PlayBgm("Result", false, true);
        //キャラを置く場所にキャラを置く場所を入れる（は？）
        ObjSet();
        // TCからプレイヤーのプレハブ読み込んで出現させる
        Player1 = Instantiate(TC.GetCharacter(PlayerNumber.P1), Vector3.zero, Quaternion.identity);
        Player2 = Instantiate(TC.GetCharacter(PlayerNumber.P2), Vector3.zero, Quaternion.identity);
        // テスト用
        //Player1 = Instantiate(testPrefab[0], Vector3.zero, Quaternion.identity);
        //Player2 = Instantiate(testPrefab[1], Vector3.zero, Quaternion.identity);
        // 使わなかったやつ
        //anim[0] = Player1.AddComponent(typeof(Animator)) as Animator;
        //anim[1] = Player2.AddComponent(typeof(Animator)) as Animator;
        // プレイヤー１のプレハブの子供を探してアニメーターをつける
        foreach (Transform child in Player1.transform)
        {
            anim[0] = child.gameObject.GetComponent<Animator>();
            anim[0].runtimeAnimatorController = animControl;
            
        }
        // プレイヤー２のプレハブの子供を探してアニメーターをつける
        foreach (Transform child in Player2.transform)
        {
            anim[1] = child.gameObject.GetComponent<Animator>();
            anim[1].runtimeAnimatorController = animControl;
            
        }
        // プレハブをキャラ置く場所の子供にする
        Player1.transform.SetParent(Set[0].transform, false);
        Player2.transform.SetParent(Set[1].transform, false);
        //Player1.transform.parent = Set[0].transform;
        //Player2.transform.parent = Set[1].transform;
        cv = GameObject.Find("Canvas").GetComponent<Canvas>();
        TextSet(cv);
    }
    void Update()
    {
        // 何かボタン押したらタイトルに戻る
        if (Input.anyKeyDown)
        {
            Loading loadObject = Loading.Instance;
            loadObject.LoadingStart("Title");
            if (loadObject == null) SceneManager.LoadScene("Title");
        }
    }
    /// <summary>
    /// このオブジェクトを検索してキャラを置く場所を探す
    /// </summary>
    void ObjSet()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.name == "1Player")
            {
                Set[0] = child.gameObject;
            }
            else if (child.name == "2Player")
            {
                Set[1] = child.gameObject;
            }
        }
    }

    /// <summary>
    /// 勝った負けたのテキストを配置
    /// </summary>
    /// <param name="canvas"></param>
    void TextSet(Canvas canvas)
    {
        Image target = null;
        // Canvas内の子供を検索
        foreach (Transform child in canvas.transform)
        {
            //プレイヤー１のテキストを検索した場合
            if (child.name == "1PlayerText")
            {
                target = child.gameObject.GetComponent<Image>();
                target.preserveAspect = true;
                if(TC.IsPlayer1Win){
                target.sprite = sp[0];
                anim[0].SetTrigger("Win");
                GameObject obj = Instantiate(Effect);
                particle = obj.GetComponent<ParticleSystem>();
                particle.Play();
            }
            else{
                target.sprite = sp[1];
                anim[0].SetTrigger("Lose");
                }
            }
            //プレイヤー２のテキストを検索した場合
            else if (child.name == "2PlayerText")
            {
                target = child.gameObject.GetComponent<Image>();
                target.preserveAspect = true;
                if (TC.IsPlayer2Win)
                {
                    target.sprite = sp[0];
                    anim[1].SetTrigger("Win");
                    GameObject obj = Instantiate(Effect);
                    obj.transform.position = new Vector3(-obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
                    particle = obj.GetComponent<ParticleSystem>();
                    particle.Play();
                }
                else
                {
                    target.sprite = sp[1];
                    anim[1].SetTrigger("Lose");
                }
            }
        }
    }
}
