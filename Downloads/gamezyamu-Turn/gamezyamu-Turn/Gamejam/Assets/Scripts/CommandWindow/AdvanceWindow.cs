using UnityEngine;

public class AdvanceWindow : WindowBase
{
    [SerializeField]
    GameObject _passButton;
    [SerializeField]
    GameObject _passAction;
    [SerializeField]
    ActionTexts _actionTexts;

    public void StartWindow(float moveSpeed, ActionTexts.ActionType type, bool succeedPass)
    {
        bool passOn = type == ActionTexts.ActionType.Switch || !succeedPass;
        _passButton.SetActive(passOn);
        _passAction.SetActive(passOn);
        if (_actionTexts != null) _actionTexts.Initialize(type);
        base.StartWindow(moveSpeed);
    }
}
