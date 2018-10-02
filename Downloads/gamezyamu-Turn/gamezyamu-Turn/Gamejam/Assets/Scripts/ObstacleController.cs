using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {

    // 障害物オブジェクトのプレファブリスト
    [SerializeField]
    List<GameObject> _obstaclePrefab;

    public void StartAction(DisturbedCommandList command, MoveCommandList moveCommand, PlayerNumber playerNumber)
    {
        // 生成位置
        Vector3 pos = Vector3.zero;

        //ターゲットのポジションとブースト移動の取得
        TurnControl turnControl = TurnControl.Instance;
        Vector3 targetPos = turnControl.GetPlayer(PlayerNumber.P1).GetPlayerPos();
        bool isBoost = turnControl.GetPlayer(PlayerNumber.P1).IsBoost;
        if (playerNumber == PlayerNumber.P2)
        {
            isBoost = turnControl.GetPlayer(PlayerNumber.P2).IsBoost;
            targetPos = turnControl.GetPlayer(PlayerNumber.P2).GetPlayerPos();
        }


        // 仮数値
        switch (command) {
            case DisturbedCommandList.ブロック:
                pos = new Vector3(targetPos.x, targetPos.y, targetPos.z + 1f);
                if(isBoost == true)
                {
                    pos.z += 2;
                }
                break;
            case DisturbedCommandList.空中ブロック:
                pos = new Vector3(targetPos.x, targetPos.y + 2f, targetPos.z + 1f);
                if(isBoost == true)
                {
                    pos.z += 2;
                }
                break;
            case DisturbedCommandList.金タライ:
                pos = new Vector3(targetPos.x, targetPos.y + 4f, targetPos.z);
                break;
            case DisturbedCommandList.巻き戻しスイッチ:
                pos = new Vector3(targetPos.x, targetPos.y, targetPos.z);
                switch (moveCommand)
                {
                    case MoveCommandList.進む:
                        pos.z = targetPos.z + 1f;
                        if (isBoost == true)
                        {
                            pos.z += 2;
                        }
                        break;
                    case MoveCommandList.ジャンプ:
                        pos.z = targetPos.z + 1f;
                        pos.y = targetPos.y + 2f;
                        if (isBoost == true)
                        {
                            pos.z += 2;
                        }
                        break;
                }
                break;
        }

        GameObject obstacle = Instantiate(_obstaclePrefab[(int)command], transform);
        obstacle.transform.position = pos;
    }

    public void Clear()
    {
        for(int i = 0;i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject,2);
        }
    }
}
