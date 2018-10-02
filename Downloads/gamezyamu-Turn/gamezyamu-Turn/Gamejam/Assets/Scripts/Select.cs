using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG;

public class Select : MonoBehaviour {

    const int charaValue = 7;

    [SerializeField] float intervalTime1P = 1f;
    [SerializeField] float intervalTime2P = 1f;

    [SerializeField] GameObject[] charaPrefabs = new GameObject[charaValue];

    [SerializeField] GameObject[] icons = new GameObject[charaValue];

    [SerializeField] Image cursor1P;
    [SerializeField] Image cursor2P;

    [SerializeField] Transform chara1P;
    [SerializeField] Transform chara2P;

    [SerializeField]List<GameObject> characters = new List<GameObject>();

    [SerializeField] Text name1P;
    [SerializeField] Text name2P;

    [SerializeField] string[] ModelNames = new string[charaValue];

    [SerializeField] GameObject signBoard;

    int afterChara1P = 0;
    int afterChara2P = 3;

    int iconCount1P = 0;
    int iconCount2P = 3;

    float intervalTemp1P;
    float intervalTemp2P;

    bool ready1P;
    bool ready2P;

    bool move1P;
    bool move2P;

    enum PlayerNum
    {
        Player1,
        Player2,
    };

    private void Start()
    {
        SoundManager.SoundInstance.PlayBgm("CharacterSelect", false,true);
        for (int i = 0; i < charaValue;i++) {
            characters.Add(null);
        }
        ChangeName(PlayerNum.Player1);
        ChangeName(PlayerNum.Player2);
        ChangeModel(PlayerNum.Player1,iconCount1P);
        ChangeModel(PlayerNum.Player2, iconCount2P);
    }

    void Update()
    {
        if (ready1P && ready2P)
        {
            signBoard.SetActive(true);
            signBoard.GetComponent<Animator>().SetBool("UpTitle", false);
        }

        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown(KeyCode.E))
        {
            if (ready1P && ready2P) { NextScene(); }
            icons[iconCount1P].GetComponent<Image>().color = Color.black;
            ready1P = true;
        }

        if (Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown(KeyCode.RightControl))
        {
            if (ready1P && ready2P) { NextScene(); }
            icons[iconCount2P].GetComponent<Image>().color = Color.black;
            ready2P = true;
        }

        if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Q))
        {
            if (ready1P && ready2P){ signBoard.GetComponent<Animator>().SetBool("UpTitle", true); }

            icons[iconCount1P].GetComponent<Image>().color = Color.white;
            ready1P = false;
        }

        if (Input.GetKeyDown("joystick 1 button 1") || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (ready1P && ready2P) { signBoard.GetComponent<Animator>().SetBool("UpTitle", true); }

            icons[iconCount2P].GetComponent<Image>().color = Color.white;
            ready2P = false;
        }

        var x1 = Input.GetAxis("Horizontal");
        var y1 = Input.GetAxis("Vertical");

        var x2 = Input.GetAxis("Horizontal2");
        var y2 = Input.GetAxis("Vertical2");



        if (move1P)
        {
            intervalTemp1P += Time.deltaTime;
            if (intervalTemp1P > intervalTime1P)
            {
                move1P = false;
            }
        }

        if (move2P)
        {

            intervalTemp2P += Time.deltaTime;
            if (intervalTemp2P > intervalTime2P)
            {
                move2P = false;
            }
        }


        //両方のコントローラの操作がない場合に返す
        if (x1 == 0 && y1 == 0 && x2 == 0 && y2 == 0) return;

   
        //1Pコントローラー
        if ((x1 != 0 || y1 != 0) && !ready1P && !move1P)
        {
            int temp = 0;

            intervalTemp1P = 0;

            afterChara1P = iconCount1P;

            if (x1 <= 1f && x1 > 0 && y1 < 0.6f && y1 > -0.6f)
            {
                iconCount1P++;
                temp = 1;
            }
            else if (x1 < 0 && x1 >= -1f && y1 < 0.6f && y1 > -0.6f)
            {
                iconCount1P--;
                temp = -1;
            }

            if (y1 <= 1f && y1 > 0 && x1 < 0.5f && x1 > -0.5f)
            {
                iconCount1P -= 4;
                temp++;
            }
            else if (y1 < 0 && y1 >= -1f && x1 < 0.5f && x1 > -0.5f)
            {
                iconCount1P += 4;
                temp++;
            }

            if (iconCount1P >= charaValue) { iconCount1P = iconCount1P % charaValue; }
            if (iconCount1P < 0) { iconCount1P = charaValue + iconCount1P; }

            //同じキャラ選択時移動できないようにする
            if (iconCount1P == iconCount2P)
            {
                while (iconCount1P == iconCount2P) {
                    iconCount1P = iconCount2P + temp;
                    if (iconCount1P >= charaValue) { iconCount1P = 0; }
                    if (iconCount1P < 0) { iconCount1P = charaValue - 1; }
                }
            }

            IconCursor(PlayerNum.Player1);
            VanishChara(PlayerNum.Player1);
            ChangeName(PlayerNum.Player1);
            ChangeModel(PlayerNum.Player1,iconCount1P);
            move1P = true;
        }

        //2Pコントローラー
        if ((x2 != 0 || y2 != 0) && !ready2P && !move2P)
        {
            int temp = 0;

            intervalTemp2P = 0;

            afterChara2P = iconCount2P;

            if (x2 <= 1 && x2 > 0 && y2 < 0.6f && y2 > -0.6f)
            {
                iconCount2P++;
                temp = 1;
            }

            if (x2 < 0 && x2 >= -1 && y2 < 0.6f && y2 > -0.6f)
            {
                iconCount2P--;
                temp = -1;
            }

            if (y2 <= 1 && y2 > 0 && x2 < 0.6f && x2 > -0.6f)
            {
                iconCount2P -= 4;
                temp++;
            }

            if (y2 < 0 && y2 >= -1 && x2 < 0.6f && x2 > -0.6f)
            {
                iconCount2P += 4;
                temp++;
            }

            //逆側からにいるキャラを選択
            if (iconCount2P >= charaValue) { iconCount2P = iconCount2P % charaValue; }
            if (iconCount2P < 0) { iconCount2P = charaValue + iconCount2P; }


            //同じキャラ選択時戻す
            if (iconCount1P == iconCount2P)
            {
                iconCount2P = iconCount1P + temp;
                if (iconCount2P >= charaValue) { iconCount2P = 0; }
                if (iconCount2P < 0) { iconCount2P = charaValue -1; }
            }

            IconCursor(PlayerNum.Player2);
            VanishChara(PlayerNum.Player2);
            ChangeName(PlayerNum.Player2);
            ChangeModel(PlayerNum.Player2,iconCount2P);
            move2P = true;
        }
    }

    void ChangeName(PlayerNum playerNum)
    {
        switch (playerNum)
        {
            case PlayerNum.Player1:
                name1P.text = ModelNames[iconCount1P];
                break;

            case PlayerNum.Player2:
                name2P.text = ModelNames[iconCount2P];
                break;
        }
    }

    //キャラアイコンのカーソルの位置設定
    void IconCursor(PlayerNum playerNum)
    {
        SoundManager.SoundInstance.PlaySE("SelectingButton", false,false);

        switch (playerNum)
        {
            case PlayerNum.Player1:
                cursor1P.transform.position = icons[iconCount1P].transform.position;
                break;
            case PlayerNum.Player2:
                cursor2P.transform.position = icons[iconCount2P].transform.position;
                break;
        }
    }

    //今まで選択してたキャラを非表示にする
    void VanishChara(PlayerNum playerNum)
    {
        switch(playerNum)
        {
            case PlayerNum.Player1:
                characters[afterChara1P].SetActive(false);
                break;

            case PlayerNum.Player2:
                characters[afterChara2P].SetActive(false);
                break;
        }
    }

    //選んだキャラクターを表示する
    void ChangeModel(PlayerNum playerNum,int modelNum)
    {
        //キャラクターを一度生成しているなら表示する
        if (characters[modelNum] != null)
        {
            characters[modelNum].SetActive(true);
        }
        else
        {
            GameObject tempObj = Instantiate(charaPrefabs[modelNum]);
            characters[modelNum] = tempObj;
        }


        switch (playerNum)
        {
            case PlayerNum.Player1:
                characters[modelNum].transform.position = chara1P.transform.position;
                characters[modelNum].transform.rotation = Quaternion.Euler(0, -60, 0);
                characters[modelNum].transform.parent = chara1P.transform;
                break;

            case PlayerNum.Player2:
                characters[modelNum].transform.position = chara2P.transform.position;
                characters[modelNum].transform.rotation = Quaternion.Euler(0, 60, 0);
                characters[modelNum].transform.parent = chara2P.transform;
                break;
        }
    }

    void NextScene()
    {
        TurnControl turnC = TurnControl.Instance;
        turnC.SetCharacter(PlayerNumber.P1, charaPrefabs[iconCount1P]);
        turnC.SetCharacter(PlayerNumber.P2, charaPrefabs[iconCount2P]);

        Loading.Instance.LoadingStart("TurnTest");

        ready1P = false;
        ready2P = false;
    }
}