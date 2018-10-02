using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerNumber
{
    P1,
    P2
}

public class TurnControl
{

    private static TurnControl instance;

    public  static TurnControl Instance
    {
        get
        {
            if(instance != null) { return instance; }
            instance = new TurnControl();
            return instance;
        }
    }

    private const int PlayerNum = 2;
    
    private bool[] isDisturderEnds = new bool[PlayerNum];
    private bool[] isMoveCommandEnds = new bool[PlayerNum];
    private bool isActioning;
    private MoveCommandList[] selectMoveCommand;
    private DisturbedCommandList[] selectDisturbedCommand;

    //プレイヤーのプレハブ
    private GameObject player1Prefab;
    private GameObject player2Prefab;

    //プレイヤーのクラス保存
    private Player player1;
    private Player player2;

    // 各コマンドコントローラーの保存
    private CommandController commandController1;
    private CommandController commandController2;

    // 障害物を管理するクラスの保存
    private ObstacleController obstacleController;

    // 現在地を示すUIを管理するクラス
    GuidePositionController guide;

    //ステージ関係の変数
    private int goalDistance;
    private int turnNum = 0;
    private float timeLimit;
    private int limitTimeDownCount;
    private float downLimitTime;
    private float minLimitTime;

    //ゲーム情報
    private bool isPlayer1Win;
    private bool isPlayer2Win;

    public int TurnNum
    {
        get
        {
            return turnNum;
        }
    }

    public bool IsPlayer1Win
    {
        get
        {
            return isPlayer1Win;
        }
    }

    public bool IsPlayer2Win
    {
        get
        {
            return isPlayer2Win;
        }
    }

    public bool IsGameEnd()
    {
        if(player1.SumMoveDistance >= goalDistance)
        {
            isPlayer1Win = true;
        }
        if(player2.SumMoveDistance >= goalDistance)
        {
            isPlayer2Win = true;
        }
        if(isPlayer2Win == true || isPlayer1Win == true) { return true; }
        return false;
    }

    //初期化
    public void Init(int goal, float startLimitTime, int downCount, float downTime, float minTime)
    {
        turnNum = 0;
        timeLimit = startLimitTime;
        downLimitTime = downTime;
        limitTimeDownCount = downCount;
        isPlayer1Win = false;
        isPlayer2Win = false;
        minLimitTime = minTime;
        goalDistance = goal;
        isActioning = false;
        selectDisturbedCommand = new DisturbedCommandList[PlayerNum];
        selectMoveCommand = new MoveCommandList[PlayerNum];
        for(int i= 0; i < PlayerNum; i++)
        {
            isDisturderEnds[i] = false;
            isMoveCommandEnds[i] = false;
        }
        ObstacleController controller = GameObject.Find("ObstacleController").GetComponent<ObstacleController>();
        guide = GameObject.Find("GuidePositionController").GetComponent<GuidePositionController>();
        guide.Initialize(goal);
    }

    public bool GetPlayerActionEnd(PlayerNumber playerNumber)
    {
        switch (playerNumber)
        {
            case PlayerNumber.P1:
                return player1.IsAnimationEnd;
            case PlayerNumber.P2:
                return player2.IsAnimationEnd;
        }
        return false;
    }

    //妨害選択したかの取得
    public bool GetDisturderEnd(PlayerNumber player)
    {
        return isDisturderEnds[(int)player];
    }

    public bool GetMoveCommandEnd(PlayerNumber player)
    {
        return isMoveCommandEnds[(int)player];
    }

    //妨害選択終了
    public void DisturbedEnd(PlayerNumber player)
    {
        switch (player)
        {
            case PlayerNumber.P1:
                isDisturderEnds[(int)PlayerNumber.P1] = true;
                return;
            case PlayerNumber.P2:
                isDisturderEnds[(int)PlayerNumber.P2] = true;
                return;
        }
    }

    //進行選択終了
    public void MoveCommandEnd(PlayerNumber player)
    {
        switch (player)
        {
            case PlayerNumber.P1:
                isMoveCommandEnds[(int)PlayerNumber.P1] = true;
                return;
            case PlayerNumber.P2:
                isMoveCommandEnds[(int)PlayerNumber.P2] = true;
                return;
        }
    }

    /// <summary>
    /// プレイヤーのアクションを開始
    /// </summary>
    public void PlayerActionStart()
    {
        bool isMove1 = (selectDisturbedCommand[1] == DisturbedCommandList.巻き戻しスイッチ && (int)selectMoveCommand[1] != (int)selectMoveCommand[0]) || (selectDisturbedCommand[1] != DisturbedCommandList.巻き戻しスイッチ && (int)selectDisturbedCommand[1] != (int)selectMoveCommand[0]);
        bool isMove2 = (selectDisturbedCommand[0] == DisturbedCommandList.巻き戻しスイッチ && (int)selectMoveCommand[0] != (int)selectMoveCommand[1]) || (selectDisturbedCommand[0] != DisturbedCommandList.巻き戻しスイッチ && (int)selectDisturbedCommand[0] != (int)selectMoveCommand[1]);

        if (selectDisturbedCommand[(int)PlayerNumber.P1] == DisturbedCommandList.巻き戻しスイッチ)
        {
            selectMoveCommand[(int)PlayerNumber.P1] = MoveCommandList.何もしない;
        }
        if(selectDisturbedCommand[(int)PlayerNumber.P2] == DisturbedCommandList.巻き戻しスイッチ)
        {
            selectMoveCommand[(int)PlayerNumber.P2] = MoveCommandList.何もしない;
        }
        player1.StartAnimation(selectMoveCommand[(int)PlayerNumber.P1]);
        player2.StartAnimation(selectMoveCommand[(int)PlayerNumber.P2]);

        commandController1.OpenResult(isMove1);
        commandController2.OpenResult(isMove2);
    }

    /// <summary>
    /// 障害物アクションを開始
    /// </summary>
    public void ObstacleActionStart()
    {
        obstacleController.StartAction(selectDisturbedCommand[(int)PlayerNumber.P1],selectMoveCommand[0], PlayerNumber.P2);
        obstacleController.StartAction(selectDisturbedCommand[(int)PlayerNumber.P2], selectMoveCommand[1], PlayerNumber.P1);
    }

    /// <summary>
    /// プレイヤーがアクションを終了したかを取得
    /// </summary>
    /// <param name="playerNumber">プレイヤーの番号</param>
    /// <returns></returns>
    public bool IsPlayerActionEnd(PlayerNumber playerNumber)
    {
        return false;
    }

    /// <summary>
    /// ターンの初期化
    /// </summary>
    public void TurnInit(bool isStart = false)
    {
        if (IsGameEnd() == true && !isStart)
        {
            //ゲーム終了処理
            Loading.Instance.LoadingStart("Result");
        }
        for (int i = 0; i < 2; i++)
        {
            isMoveCommandEnds[i] = false;
            isDisturderEnds[i] = false;
        }
        //ターンの追加
        turnNum++;
        //制限時間の更新
        if(turnNum % limitTimeDownCount == 0)
        {
            timeLimit -= downLimitTime;
            if(timeLimit < minLimitTime)
            {
                timeLimit = minLimitTime;
            }
        }
        commandController1.StartCommandSelect(timeLimit);
        commandController2.StartCommandSelect(timeLimit);
        isActioning = false;
        obstacleController.Clear();
    }

    /// <summary>
    /// 次のターン
    /// </summary>
    public void NextTurn()
    {
        TurnInit();
    }

    /// <summary>
    /// プレイヤーの登録(選択画面)
    /// </summary>
    /// <param name="player">プレイヤーナンバー</param>
    /// <param name="prefab">プレハブ</param>
    public void SetCharacter(PlayerNumber player,GameObject prefab)
    {
        switch (player)
        {
            case PlayerNumber.P1:
                player1Prefab = prefab;
                return;
            case PlayerNumber.P2:
                player2Prefab = prefab;
                return;
        }
    }

    /// <summary>
    /// 登録したプレイヤーの取得
    /// </summary>
    /// <param name="player">プレイヤーのナンバー</param>
    /// <returns></returns>
    public GameObject GetCharacter(PlayerNumber player)
    {
        switch (player)
        {
            case PlayerNumber.P1:
                return player1Prefab;
            case PlayerNumber.P2:
                return player2Prefab;
        }
        return null;
    }

    /// <summary>
    /// コマンドの登録
    /// </summary>
    /// <param name="playerNumber">プレイヤーナンバー</param>
    /// <param name="comands">コマンド配列</param>
    public void SetComand(PlayerNumber playerNumber, int[] comands)
    {
        switch (playerNumber)
        {
            case PlayerNumber.P1:
                selectDisturbedCommand[0] = (DisturbedCommandList)comands[0];
                selectMoveCommand[0] = (MoveCommandList)comands[1];
                DisturbedEnd(PlayerNumber.P1);
                MoveCommandEnd(PlayerNumber.P1);
                return;
            case PlayerNumber.P2:
                selectDisturbedCommand[1] = (DisturbedCommandList)comands[0];
                selectMoveCommand[1] = (MoveCommandList)comands[1];
                DisturbedEnd(PlayerNumber.P2);
                MoveCommandEnd(PlayerNumber.P2);
                return;
        }
    }


    /// <summary>
    /// プレイヤークラスの登録
    /// </summary>
    /// <param name="player1">プレイヤー1</param>
    /// <param name="player2">プレイヤー2</param>
    public void SetPlayer(Player player1, Player player2)
    {
        this.player1 = player1;
        this.player2 = player2;
    }

    public Player GetPlayer(PlayerNumber playerNumber)
    {
        if(playerNumber == PlayerNumber.P1)
        {
            return player1;
        }
        return player2;
    }

    /// <summary>
    /// コマンドコントローラーを登録
    /// </summary>
    /// <param name="controller1">コントローラー1</param>
    /// <param name="controller2">コントローラー2</param>
    public void SetCommnadController(CommandController controller1, CommandController controller2)
    {
        this.commandController1 = controller1;
        this.commandController1.SetPlayer(PlayerNumber.P1, Controller.ControllerInstance.IsUseController());
        this.commandController2 = controller2;
        this.commandController2.SetPlayer(PlayerNumber.P2, Controller.ControllerInstance.IsUseController());
    }

    /// <summary>
    /// 障害物を管理するクラスの登録
    /// </summary>
    /// <param name="controller">コントローラー</param>
    public void SetObstacleController(ObstacleController controller)
    {
        this.obstacleController = controller;
    }
}
