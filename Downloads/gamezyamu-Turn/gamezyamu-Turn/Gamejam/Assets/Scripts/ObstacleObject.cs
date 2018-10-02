using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Ice,
    Tree,
    Switch,
    Tarai
}

public class ObstacleObject : MonoBehaviour {

    [SerializeField]
    private ObstacleType obstacleType;

    private void OnTriggerEnter(Collider hit)
    {
        if(hit.transform.parent.GetComponent<Player>() == null) { return; }
        Player player = hit.transform.parent.GetComponent<Player>();
        player.Damage(obstacleType);
        SoundManager.SoundInstance.PlaySE(SoundList.GetSoundString(SoundType.Explode), false, false);
    }
}
