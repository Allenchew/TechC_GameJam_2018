using UnityEngine;

public class ObstacleWindow : WindowBase
{
    [SerializeField]
    GameObject _switchButton;
    [SerializeField]
    GameObject _switchAction;

    public void StartWindow(float moveSpeed, bool isSwitchUsed)
    {
        _switchButton.SetActive(!isSwitchUsed);
        _switchAction.SetActive(!isSwitchUsed);
        base.StartWindow(moveSpeed);
    }
}
