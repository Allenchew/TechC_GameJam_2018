using UnityEngine;
using UnityEngine.UI;

public class ActionTexts : MonoBehaviour{
    [SerializeField]
    Image title;
    [SerializeField]
    Sprite[] titleImage;
    [SerializeField]
    Sprite[] textImage;
    [SerializeField]
    Image[] textsImage;
    public enum ActionType
    {
        Onstacle,
        Advance,
        Switch,
    }

    public void Initialize(ActionType type)
    {
        int num = type == ActionType.Switch ? textsImage.Length : 0;
        for (int i = 0;i < textsImage.Length;i++)
        {
            textsImage[i].sprite = textImage[i + num];
        }

        title.sprite = titleImage[(int)type - 1];
    }
}
