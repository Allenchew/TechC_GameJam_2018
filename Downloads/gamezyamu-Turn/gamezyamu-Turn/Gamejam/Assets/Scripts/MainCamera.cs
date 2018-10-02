using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    [SerializeField]
    private Transform player;
    private Vector3 offset;

    public void SetPlayer(Transform playerT)
    {
        player = playerT;
        offset = transform.position - player.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(player == null) { return; }
        transform.position = player.position + offset;	
	}
}
