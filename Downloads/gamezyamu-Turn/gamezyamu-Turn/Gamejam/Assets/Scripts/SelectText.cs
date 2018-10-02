using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectText : MonoBehaviour {

    Text keyText;

    [SerializeField] string[] changeWords = new string[3];

    [SerializeField] Color[] colors;

    [SerializeField] float speed;
    Color deffaultColor;
    Color color1P;
    Color color2P;

    int count;

    float alpha = 255;
    float timer;
    float alphaTimer;

    bool fadeIn;
    bool fadeOut;

	// Use this for initialization
	void Start () {
        //deffaultColor = new Color(50,50,50);
        //color1P = new Color(176,184,255);
        //color2P = new Color(255,175, 215);
        keyText = GetComponent<Text>();
        keyText.text = changeWords[count];
        fadeIn = true;
    }

    // Update is called once per frame
    void Update () {
        //timer += Time.deltaTime;

        if (fadeIn)
        {
            timer -= Time.deltaTime * speed;
            alpha = keyText.color.a + timer;
            alpha = Mathf.Clamp(alpha, 0, 1);
            colors[count].a = alpha;
            keyText.color = colors[count];

            if (alpha <= 0)
            {
                count++;
                if (count >= colors.Length)
                {
                    count = 0;
                }
                keyText.text = changeWords[count];

                fadeOut = true;
                fadeIn = false;
            }


        }

        if (fadeOut)
        {
            timer += Time.deltaTime * speed;
            alpha = keyText.color.a + timer;
            alpha = Mathf.Clamp(alpha, 0, 1);
            colors[count].a = alpha;
            keyText.color = colors[count];

            if (alpha >= 1)
            {
                fadeIn = true;
                fadeOut = false;
            }
        }
    }
}
