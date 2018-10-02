using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Title,
    CharacterSelect,
    Racing,
    Result,
    Explode,
    FixButton,
    Icy,
    Jump,
    Kick,
    Landing,
    Laugh,
    RollFinish,
    SelectingButton,
    Switch,
    SwordIcy,
    Walking
}

public class SoundList
{
    private static Dictionary<SoundType, string> sounds = new Dictionary<SoundType, string>()
    {
        {SoundType.Title,"Title" },
        {SoundType.CharacterSelect,"CharacterSelect" },
        {SoundType.Racing,"Racing" },
        {SoundType.Explode,"Explode" },
        {SoundType.FixButton,"FixButton" },
        {SoundType.Icy,"Icy" },
        {SoundType.Jump,"Jump" },
        {SoundType.Kick,"Kick" },
        {SoundType.Landing,"Landing" },
        {SoundType.Laugh,"Laugh" },
        {SoundType.RollFinish,"RollFinish" },
        {SoundType.SelectingButton,"SelectingButton" },
        {SoundType.Switch,"Switch" },
        {SoundType.SwordIcy,"SwordIcy" },
        {SoundType.Walking,"Walking" }
    };

    public static string GetSoundString(SoundType soundType)
    {
        return sounds[soundType];
    }
}

public class StartSound : MonoBehaviour
{

    [SerializeField]
    private SoundType soundType;
    [SerializeField]
    private bool loop;


	void Start ()
    {
        SoundManager.SoundInstance.PlayBgm(SoundList.GetSoundString(soundType), false, loop);
	}
}
