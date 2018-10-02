using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMainGame : MonoBehaviour {

    TurnControl turnControl;

    public GameObject player1De;
    public GameObject player2De;

    public bool debug;
    [SerializeField]
    private int goalDistance = 20;
    [SerializeField]
    private float startLimitTime = 10;
    [SerializeField]
    private int timeCount = 3;
    [SerializeField]
    private float downLimitTime = 1;
    [SerializeField]
    private float minLimitTime = 3;

    private bool isAnimationing;

    void Start ()
    {
        turnControl = TurnControl.Instance;
        turnControl.Init(goalDistance,startLimitTime,timeCount,downLimitTime,minLimitTime);
        CommandController commandController1 = GameObject.Find("CommandController1").GetComponent<CommandController>();
        CommandController commandController2 = GameObject.Find("CommandController2").GetComponent<CommandController>();
        turnControl.SetCommnadController(commandController1, commandController2);
        ObstacleController obstacleController = FindObjectOfType<ObstacleController>();
        turnControl.SetObstacleController(obstacleController);

        Player player1 = GameObject.Find("Player1Box").GetComponent<Player>();
        Player player2 = GameObject.Find("Player2Box").GetComponent<Player>();
        turnControl.SetPlayer(player1, player2);
        turnControl.TurnInit(true);

        if (debug)
        {
            player1.CreatePlayer(player1De);
            player2.CreatePlayer(player2De);
        }
        else
        {
            GameObject player1Prefb = turnControl.GetCharacter(PlayerNumber.P1);
            GameObject player2Prefb = turnControl.GetCharacter(PlayerNumber.P2);
            player1.CreatePlayer(player1Prefb);
            player2.CreatePlayer(player2Prefb);
        }
	}

    private void Update()
    {
        //アニメーション中
        if(isAnimationing == true)
        {

            //お互いのアクションが終わったら
            bool player1end = turnControl.GetPlayerActionEnd(PlayerNumber.P1);
            bool player2end = turnControl.GetPlayerActionEnd(PlayerNumber.P2);
            if(player1end == false || player2end == false) { return; }
            turnControl.NextTurn();
            isAnimationing = false;
            return;
        }

        //入力待ち
        bool[] isMoveCommandEnds = { false, false };

        isMoveCommandEnds[0] = turnControl.GetMoveCommandEnd(PlayerNumber.P1);
        isMoveCommandEnds[1] = turnControl.GetMoveCommandEnd(PlayerNumber.P2);
        
        for(int i = 0; i < 2; i++)
        {
            if(isMoveCommandEnds[i] == false)
            {
                return;
            }
        }

        turnControl.ObstacleActionStart();
        turnControl.PlayerActionStart();
        isAnimationing = true;
    }
}
