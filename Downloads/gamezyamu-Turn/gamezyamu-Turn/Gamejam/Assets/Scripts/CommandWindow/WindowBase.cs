using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WindowBase : MonoBehaviour {
    [SerializeField]
    RectTransform _rectTransform;
    Vector3 _startPos;

    [SerializeField]
    Sprite[] _buttonsImage;

    private void Start()
    {
        _startPos = _rectTransform.localPosition;
    }

    public void Initialize(bool isUseController, PlayerNumber playerNum)
    {
        Transform parent = transform.GetChild(0);
        Image[] buttons = new Image[parent.childCount];
        for (int i = 0;i < parent.childCount; i++)
        {
            buttons[i] = parent.GetChild(i).GetComponent<Image>();
        }
        int num = isUseController ? 0 : 4;
        if (playerNum == PlayerNumber.P2) num = 8;
        // ボタンの画像差し替え
        for (int i = 0;i < buttons.Length; i++)
        {
            buttons[i].sprite = _buttonsImage[i + num];
        }
    }

    public void StartWindow(float moveSpeed)
    {
        gameObject.SetActive(true);
        _rectTransform.DOLocalMoveX(0, moveSpeed);
    }

    public void SelectCommand(float moveSpeed)
    {
        _rectTransform.DOLocalMoveX(-1000f, moveSpeed);
        DOVirtual.DelayedCall(moveSpeed, () => _rectTransform.localPosition = _startPos);
    }
}
