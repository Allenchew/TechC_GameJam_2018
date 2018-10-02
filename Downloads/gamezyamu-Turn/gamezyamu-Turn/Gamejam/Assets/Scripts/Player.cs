using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveCommandList
{
    何もしない = -1,
    進む = 0,
    ジャンプ,
    待つ
}

public enum DisturbedCommandList
{
    ブロック = 0,
    空中ブロック,
    金タライ,
    巻き戻しスイッチ
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector3 playerStartPosition;
    [SerializeField]
    private RuntimeAnimatorController runtimeAnimator;
    [SerializeField]
    private MainCamera playerCamera;
    [SerializeField]
    private PlayerNumber playerNum;
    private Vector3 nextPlayePos;
    private Vector3 goalPos;
    private Vector3 movePlayer;
    private Animator playerAnimator;
    private Transform playerTransform;
    private Rigidbody playerRb;

    private float speed = 2;
    private bool isAnimationEnd;
    private bool waitFlag;
    private bool isStartAnima;

    //行動関係変数
    private float moveDistance;
    private float sumMoveDistance;

    //待つ関連変数
    private bool isBoost;
    private const int boostValue = 3;

    //ジャンプ移動関連変数
    private float       startDirection;
    private Vector3     startPosition;

    //ダメージ関連変数
    private bool        isDamage;
    private float       damageMoveTimer;
    private Vector3     damagePosition;

    private void Start()
    {
        moveDistance = 0;
        sumMoveDistance = 0;
        isDamage = false;
        isBoost = false;
    }

    public void GetAnimatior()
    {
        playerAnimator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        playerAnimator.runtimeAnimatorController = runtimeAnimator;
    }

    public void CreatePlayer(GameObject prefab)
    {
        Instantiate(prefab,playerStartPosition,Quaternion.identity, transform);
        GetAnimatior();
        playerRb = transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
        CapsuleCollider capsuleCollider = transform.GetChild(0).gameObject.AddComponent<CapsuleCollider>();
        capsuleCollider.center = new Vector3(0, 0.4228645f, 0);
        capsuleCollider.radius = 0.1817598f;
        capsuleCollider.height = 0.8542711f;
        
        playerRb.isKinematic = false;
        playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        playerTransform = transform.GetChild(0);
        playerTransform.gameObject.layer = 9;
        playerCamera.SetPlayer(playerTransform);
    }


    private void Update()
    {
        if(isStartAnima == false) { return; }
        if(isDamage == true)
        {
            damageMoveTimer += Time.deltaTime;
            Vector3 finalPostion;
            if (damageMoveTimer >= 1.0f)
            {
                damageMoveTimer = 1;
                finalPostion = Vector3.Slerp(damagePosition, startPosition, damageMoveTimer);
                playerRb.MovePosition(finalPostion);
                damageMoveTimer = 0;
                playerAnimator.SetTrigger("End");
                isDamage = false;
                isStartAnima = false;
                isAnimationEnd = true;
                return;
            }
            finalPostion = Vector3.Slerp(damagePosition, startPosition, damageMoveTimer);
            playerRb.MovePosition(finalPostion);
            return;
        }
        switch(selectMoveCommand)
        {
            case MoveCommandList.進む:
                PlayerMove();
                return;
            case MoveCommandList.ジャンプ:
                PlayerJump();
                return;
            case MoveCommandList.待つ:
                PlayerWait();
                return;
        }
    }

    private void PlayerMove()
    {
        movePlayer = new Vector3(0, 0, speed * Time.deltaTime);
        Vector3 finalPos = playerTransform.position + movePlayer;
        if(finalPos.z >= nextPlayePos.z)
        {
            finalPos = nextPlayePos;
            playerRb.MovePosition(finalPos);
            sumMoveDistance += moveDistance;
            playerAnimator.SetTrigger("End");
            isStartAnima = false;
            isAnimationEnd = true;
        }
        playerRb.MovePosition(finalPos);
    }


    /// <summary>
    /// ジャンプアクション
    /// </summary>
    private void PlayerJump()
    {
        movePlayer = new Vector3(0, 0, speed * Time.deltaTime);
        Vector3 finalPos = playerTransform.position + movePlayer;
        if (finalPos.z >= nextPlayePos.z)
        {

            finalPos = nextPlayePos;
            playerRb.MovePosition(finalPos);
            playerAnimator.SetTrigger("End");
            sumMoveDistance += moveDistance;
            isStartAnima = false;
            isAnimationEnd = true;
        }
        else
        {
            float deltaDistance = Mathf.Abs(nextPlayePos.z - finalPos.z);
            float saDistance = startDirection - deltaDistance;
            float nomalDistance = saDistance / startDirection;
            float correction = 1.0f;
            if(moveDistance == 6) { correction = 1; }
            if(nomalDistance < 0.5f)
            {
                float g = (2 * correction) * nomalDistance / 0.5f;
                finalPos.y = startPosition.y + g;
            }
            else
            {
                float g2 = (2 * correction) * (1 - (nomalDistance - 0.5f) / 0.5f);
                finalPos.y = startPosition.y + g2;
            }
            playerRb.MovePosition(finalPos);
        }
    }

    private float timer;

    private void PlayerWait()
    {
        timer += Time.deltaTime;
        if (timer >= 2)
        {
            waitFlag = true;
            isStartAnima = false;
            isAnimationEnd = true;
            timer = 0;
        }
    }
    private MoveCommandList selectMoveCommand;

    public bool IsAnimationEnd
    {
        get
        {
            return isAnimationEnd;
        }
    }

    public bool IsBoost
    {
        get
        {
            return isBoost;
        }
    }

    /// <summary>
    /// プレイヤーの移動した合計値
    /// </summary>
    public float SumMoveDistance
    {
        get
        {
            return sumMoveDistance;
        }
    }

    public void Damage(ObstacleType obstacleType)
    {
        playerAnimator.SetTrigger("Damage");
        CapsuleCollider capsuleCollider = transform.GetChild(0).GetComponent<CapsuleCollider>();
        capsuleCollider.center = new Vector3(0, 0.4228645f, 0);
        capsuleCollider.radius = 0.1817598f;
        capsuleCollider.height = 0.8542711f;
        damagePosition = playerTransform.position;
        moveDistance = 0;
        isDamage = true;
        isStartAnima = true;
        isAnimationEnd = false;
        switch (obstacleType)
        {
            case ObstacleType.Tarai:
                isBoost = false;
                break;
            case ObstacleType.Switch:
                if(playerNum == PlayerNumber.P1)
                {
                    startPosition = playerTransform.position;
                    startPosition.z = TurnControl.Instance.GetPlayer(PlayerNumber.P2).playerTransform.position.z;
                    sumMoveDistance = TurnControl.Instance.GetPlayer(PlayerNumber.P2).sumMoveDistance;
                }
                else
                {
                    startPosition = playerTransform.position;
                    startPosition.z = TurnControl.Instance.GetPlayer(PlayerNumber.P1).playerTransform.position.z;
                    sumMoveDistance = TurnControl.Instance.GetPlayer(PlayerNumber.P1).sumMoveDistance;
                }
                
                break;
        }
    }

    public void StartAnimation(MoveCommandList moveCommandList)
    {
        //デバッグ用
        //moveCommandList = MoveCommandList.待つ;
        //
        selectMoveCommand = moveCommandList;
        
        startPosition = playerTransform.position;
        CapsuleCollider capsuleCollider = transform.GetChild(0).GetComponent<CapsuleCollider>();
        switch (moveCommandList)
        {
            case MoveCommandList.進む:
                playerAnimator.SetTrigger("Run");
                capsuleCollider.center = new Vector3(0, 0.4228645f, 0);
                capsuleCollider.radius = 0.1817598f;
                capsuleCollider.height = 0.8542711f;
                isStartAnima = true;
                if (isBoost == true)
                {
                    isBoost = false;
                    nextPlayePos = playerTransform.position + playerTransform.forward * 2 * boostValue;
                    moveDistance = 6;
                }
                else
                {
                    nextPlayePos = playerTransform.position + playerTransform.forward * 2;
                    moveDistance = 2;
                }
                
                return;
            case MoveCommandList.ジャンプ:
                playerAnimator.SetTrigger("Jump");
                capsuleCollider.center = new Vector3(0, 0.4228645f, 0);
                capsuleCollider.radius = 0.1817598f;
                capsuleCollider.height = 0.8542711f;
                isStartAnima = true;
                if (isBoost == true)
                {
                    isBoost = false;
                    nextPlayePos = playerTransform.position + playerTransform.forward * 2 * boostValue;
                    moveDistance = 6;
                }
                else
                {
                    nextPlayePos = playerTransform.position + playerTransform.forward * 2;
                    moveDistance = 2;
                }
                startDirection = Vector3.Distance(playerTransform.position, nextPlayePos);
                
                return;
            case MoveCommandList.待つ:
                timer = 0;
                playerAnimator.SetTrigger("Wait");
                capsuleCollider.center = new Vector3(0, 0.56403f, 0);
                capsuleCollider.radius = 0.1817598f;
                capsuleCollider.height = 0.6910505f;
                moveDistance = 0;
                isStartAnima = true;
                isAnimationEnd = true;
                isBoost = true;
                return;
            case MoveCommandList.何もしない:
                isAnimationEnd = true;
                return;
        }
    }

    public Vector3 GetPlayerPos()
    {
        return playerTransform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerStartPosition, 1);
    }
}
