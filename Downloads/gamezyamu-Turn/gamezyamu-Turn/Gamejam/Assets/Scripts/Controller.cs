using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class Controller : MonoBehaviour {
    public static Controller ControllerInstance;


    [System.NonSerialized]
    public List<KeyCode[]> ControllerSet = new List<KeyCode[]>();
    [System.NonSerialized]
    public List<KeyCode[]> KeyBoardSet = new List<KeyCode[]>();
    [System.NonSerialized]
    public bool[] PresetPlayer = { false, false };


    private int SetupPlayerNum = 0;
    private string[] ButtonToNum = new string[]{"A","B","X","Y","LeftTriggerButton","RightTriggerButton","Back","Start"};
    private string[] AxisToNum = new string[] { "Up", "Down", "Left", "Right", "LeftTrigger", "RightTrigger" };
    private string[][] KeyToNum = new string[][] { new string[] { "S", "D", "A", "W" }, new string[] { "Down", "Right", "Left", "Up" } };
    
    private int[] PresetNum = new int[2];

    private void Awake()
    {
        if (ControllerInstance == null)
        {
            ControllerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if( ControllerInstance != this)
        {
            Destroy(gameObject);
        }
    }
	// Use this for initialization
	void Start () {
        KeyBoardSet.Add(new KeyCode[] { KeyCode.S, KeyCode.D, KeyCode.A, KeyCode.W });
        KeyBoardSet.Add(new KeyCode[] { KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.UpArrow });
    }
	void Update () {
        Scene AScene = SceneManager.GetActiveScene();
        if (AScene.name == "Title")
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(kcode))
                    {
                        String OutName = kcode.ToString();
                        if (OutName.Length > 8 && OutName.Substring(0, 8) == "Joystick" && (!PresetPlayer[0] || !PresetPlayer[1]))
                        {
                            if (!PresetPlayer[SetupPlayerNum])
                            {
                                switch (OutName[8])
                                {
                                    case '1':
                                        if (PresetNum.Length > 0 && Array.IndexOf(PresetNum, 1) != -1)
                                        {

                                        }
                                        else
                                        {
                                            PresetPlayer[SetupPlayerNum] = true;
                                            PresetNum[SetupPlayerNum] = 1;
                                            SetupPlayerNum++;
                                            Debug.Log(OutName[8]);
                                            SetupControllerNum("Joystick" + OutName[8]);
                                        }
                                        break;
                                    case '2':
                                        if (PresetNum.Length > 0 && Array.IndexOf(PresetNum, 2) != -1)
                                        {

                                        }
                                        else
                                        {
                                            PresetPlayer[SetupPlayerNum] = true;
                                            PresetNum[SetupPlayerNum] = 2;
                                            SetupPlayerNum++;
                                            Debug.Log(OutName[8]);
                                            SetupControllerNum("Joystick" + OutName[8]);
                                        }
                                        break;
                                    case '3':
                                        if (PresetNum.Length > 0 && Array.IndexOf(PresetNum, 3) != -1)
                                        {

                                        }
                                        else
                                        {
                                            PresetPlayer[SetupPlayerNum] = true;
                                            PresetNum[SetupPlayerNum] = 3;
                                            SetupPlayerNum++;
                                            Debug.Log(OutName[8]);
                                            SetupControllerNum("Joystick" + OutName[8]);
                                        }
                                        break;
                                    case '4':
                                        if (PresetNum.Length > 0 && Array.IndexOf(PresetNum, 4) != -1)
                                        {

                                        }
                                        else
                                        {
                                            PresetPlayer[SetupPlayerNum] = true;
                                            PresetNum[SetupPlayerNum] = 4;
                                            SetupPlayerNum++;
                                            Debug.Log(OutName[8]);
                                            SetupControllerNum("Joystick" + OutName[8]);
                                        }
                                        break;
                                    default:
                                        break;

                                }
                            }
                        }

                    }

                }
            }
        }
        
    }
    private void SetupControllerNum(string JoyName)
    {
        KeyCode[] kc = new KeyCode[8];
        for (int i = 0; i < 8; i++)
        {
            kc[i] = (KeyCode)System.Enum.Parse(typeof(KeyCode), JoyName + "Button" + i);
        }
        ControllerSet.Add(kc);
        Debug.Log("done");
    }
    public KeyCode ButtonPressed(int PlayerNum, string ButtonName)
    {
        int IndexKey = Array.IndexOf(KeyToNum[PlayerNum], ButtonName);
        if (PresetPlayer[0] && PresetPlayer[1])
        {
            int IndexPressed = Array.IndexOf(ButtonToNum, ButtonName);
            Debug.Log(ButtonName);
            return ControllerSet[PlayerNum][IndexPressed];
        }else if(IndexKey!= -1) 
        {
            return KeyBoardSet[PlayerNum][IndexKey];
        }
        return KeyCode.None;
    }
    public bool AxisPressed(int PlayerNum, string AxisName)
    {
        switch (Array.IndexOf(AxisToNum, AxisName))
        {
            case 0:
                if (Input.GetAxis("Joystick" + PresetNum[PlayerNum] + "Axis7") > 0.5f)
                    return true;
                break;
            case 1:
                if (Input.GetAxis("Joystick" + PresetNum[PlayerNum] + "Axis7") < -0.5f)
                    return true;
                break;
            case 2:
                if (Input.GetAxis("Joystick" + PresetNum[PlayerNum] + "Axis6") < -0.5f)
                    return true;
                break;
            case 3:
                if (Input.GetAxis("Joystick" + PresetNum[PlayerNum] + "Axis6") > 0.5f)
                    return true;
                break;
            case 4:
                if (Input.GetAxis("Joystick" + PresetNum[PlayerNum] + "Axis3") > 0.5f)
                    return true;
                break;
            case 5:
                if (Input.GetAxis("Joystick" + PresetNum[PlayerNum] + "Axis3") < -0.5f)
                    return true;
                break;
        }
        return false;
    }

    public bool IsUseController()
    {
        return (PresetPlayer[0] && PresetPlayer[1]);
    }
}
