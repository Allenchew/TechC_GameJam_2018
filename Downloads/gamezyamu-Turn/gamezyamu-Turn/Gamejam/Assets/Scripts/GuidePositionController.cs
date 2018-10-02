using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GuidePositionController : MonoBehaviour {

    [SerializeField]
    RectTransform player1;
    [SerializeField]
    RectTransform player2;

    bool isMove;
    // 上から下までの幅
    private const int Height = 1000;
    // 距離1で進む値
    float moveNum;
    float goal;

    public void Initialize(float goal)
    {
        moveNum = Height / goal;
        this.goal = goal;
    }

    private void Update()
    {
        //if (isMove)
        {
            float posY1 = (TurnControl.Instance.GetPlayer(PlayerNumber.P1).GetPlayerPos().z - goal / 2) * moveNum;
            float posY2 = (TurnControl.Instance.GetPlayer(PlayerNumber.P2).GetPlayerPos().z - goal / 2) * moveNum;
            Vector3 player1Pos = new Vector3(player1.localPosition.x, posY1, 0);
            Vector3 player2Pos = new Vector3(player2.localPosition.x, posY2, 0);
            player1.localPosition = player1Pos;
            player2.localPosition = player2Pos;
        }
    }
}
