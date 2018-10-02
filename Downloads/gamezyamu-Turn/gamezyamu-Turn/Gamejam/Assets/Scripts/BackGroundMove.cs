using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour {

    [SerializeField] float speed = 0.1f;

    Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().sharedMaterial;
    }

    void Update () {
        //時間に合わせて移動させる
        float value = Mathf.Repeat(Time.time * speed,1);

        Vector2 vec = new Vector2(value,value);

        material.SetTextureOffset("_MainTex",vec);
	}
}
