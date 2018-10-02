using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CommandController : MonoBehaviour {

    [SerializeField]
    ObstacleWindow _obstacleWindow;
    [SerializeField]
    AdvanceWindow _advanceWindow;
    [SerializeField]
    Image _resultImage;
    [SerializeField]
    Sprite[] _resultImages;
    [SerializeField]
    Text _timerText;
    [SerializeField]
    GameObject _timerGO;
    [SerializeField]
    float _moveSpeed = 2.0f;
    // 制限時間
    float _finishTime;
    // 残り時間
    float _timer;
    int _selectCount;
    // 選択したボタンに対応する番号を保存
    int[] _selectCommand = new int[2];
    // 1つ前に選択したボタンに対応する番号
    int[] _beforeSelectCommand = new int[2];
    // スイッチコマンド使ったか
    bool _isSwitchUsed = false;
    bool _isUseController;
    PlayerNumber _player;

    void Update()
    {
        if (_timerGO.activeSelf)
        {
            if (_timer < 0)
            {
                // ランダム決定(巻き戻しスイッチは対象外)
                int randomNum = Random.Range(0, 3);
                SelectCommand(randomNum);
            }

            int pushButtonNum = CheckButtonPush();
            if(pushButtonNum >= 0)
            {
                SelectCommand(pushButtonNum);
            }

            _timer -= Time.deltaTime;
            _timerText.text = _timer.ToString("F1");
        }
    }

    private int CheckButtonPush()
    {
        // 妨害：木　前進：前進　スイッチ：前方
        if (Input.GetKey(Controller.ControllerInstance.ButtonPressed((int)_player, (_isUseController ? "B" : (_player == PlayerNumber.P1) ? "D" : "Right"))))
        {
            SoundManager.SoundInstance.PlaySE(SoundList.GetSoundString(SoundType.FixButton), false, false);
            return 0;
        }
        // 妨害：雪の結晶　前進：ジャンプ　スイッチ：上空
        if (Input.GetKey(Controller.ControllerInstance.ButtonPressed((int)_player, (_isUseController ? "A" : (_player == PlayerNumber.P1) ? "S" : "Down"))))
        {
            SoundManager.SoundInstance.PlaySE(SoundList.GetSoundString(SoundType.FixButton), false, false);
            return 1;
        }
        // 妨害：たらい　前進：パス　スイッチ：その場
        if (Input.GetKey(Controller.ControllerInstance.ButtonPressed((int)_player, (_isUseController ? "X" : (_player == PlayerNumber.P1) ? "A" : "Left"))))
        {
            if (_selectCount == 0)
            {
                SoundManager.SoundInstance.PlaySE(SoundList.GetSoundString(SoundType.FixButton), false, false);
                return 2;
            }
            // スイッチコマンド選択もしくは前回パスが押されていない
            if (_selectCommand[0] == 3 || _beforeSelectCommand[0] == 3)
            {
                SoundManager.SoundInstance.PlaySE(SoundList.GetSoundString(SoundType.FixButton), false, false);
                return 2;
            }
            if (_beforeSelectCommand[1] != 2)
            {
                SoundManager.SoundInstance.PlaySE(SoundList.GetSoundString(SoundType.FixButton), false, false);
                return 2;
            }
        }
        // 妨害：スイッチ
        if (Input.GetKey(Controller.ControllerInstance.ButtonPressed((int)_player, (_isUseController ? "Y" : (_player == PlayerNumber.P1) ? "W" : "Up"))))
        {
            // 一度もスイッチが使われていない
            if (_selectCount == 0 && !_isSwitchUsed)
            {
                _isSwitchUsed = true;
                SoundManager.SoundInstance.PlaySE(SoundList.GetSoundString(SoundType.FixButton), false, false);
                return 3;
            }
        }
        return -1;
    }

    /// <summary>
    /// コマンド選択スタート
    /// </summary>
    /// /// <param name="time">制限時間</param>
    public void StartCommandSelect(float time)
    {
        // gameObject.SetActive(true);
        _finishTime = time;
        _timer = time;
        _selectCount = 0;
        _obstacleWindow.StartWindow(_moveSpeed, _isSwitchUsed);
        DOVirtual.DelayedCall(_moveSpeed, () => _timerGO.SetActive(true));
    }

    /// <summary>
    /// コマンド選択
    /// </summary>
    /// <param name="num">選択した番号</param>
    private void SelectCommand(int num)
    {
        if (_selectCount >= _selectCommand.Length) return;

        //　障害コマンド
        if (_selectCount == 0)
        {
            SelectObstacleCommand(num);
        }

        _timerGO.SetActive(false);
        // 選択した番号保存
        _selectCommand[_selectCount] = num;
        _selectCount++;

        // 前進コマンド
        if (_selectCount == 2)
        {
            SelectAdvanceCommand();
        }
    }

    private void SelectObstacleCommand(int num)
    {
        _advanceWindow.gameObject.SetActive(true);
        _obstacleWindow.SelectCommand(_moveSpeed);
        // デバッグ用-----
        if(!_isSwitchUsed) _isSwitchUsed = num == 3;
        //----------------
        // パスボタンはとりあえす2連続では使えないように
        //（TurnControl.csでStartCommandSelect呼ぶとき前回選択したパスが成功しているかのboolを送ってもらえれば失敗時の次も表示が可能）
        ActionTexts.ActionType type = num == 3 ? ActionTexts.ActionType.Switch : ActionTexts.ActionType.Advance;
        bool succeedPass = _selectCommand[0] != 3 && _selectCommand[1] == 2;
        _advanceWindow.StartWindow(_moveSpeed, type, succeedPass);
        DOVirtual.DelayedCall(_moveSpeed, () => {
            _obstacleWindow.gameObject.SetActive(false);
            _timerGO.SetActive(true);
        });
        _timer = _finishTime;
    }

    private void SelectAdvanceCommand()
    {
        _advanceWindow.SelectCommand(_moveSpeed);
        DOVirtual.DelayedCall(_moveSpeed, () => {
            _advanceWindow.gameObject.SetActive(false);
            // gameObject.SetActive(false);

            // 選択結果を送る
            TurnControl.Instance.SetComand(_player, _selectCommand);
        });
        _timerGO.SetActive(false);
        _selectCommand.CopyTo(_beforeSelectCommand, 0);
    }

    public void SetPlayer(PlayerNumber player, bool isUseController)
    {
        this._player = player;
        _isUseController = isUseController;
        _obstacleWindow.Initialize(isUseController, _player);
        _advanceWindow.Initialize(isUseController, _player);
    }

    public void OpenResult(bool isMove)
    {
        _resultImage.sprite = isMove ? _resultImages[0] : _resultImages[1];
        _resultImage.gameObject.SetActive(true);
        DOVirtual.DelayedCall(1f, () => _resultImage.gameObject.SetActive(false));
    }
}
