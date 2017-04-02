﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private float chkTimer;
    public static bool showChk = false;

    private float uiBaseScreenHeight = 700f;

    private static Font starFont;

    public static GameObject player;

    public static Vector3 savedPosition;

    private static float time = 5f;
    private static bool counter = false;
    private static string content = "";

    // Use this for initialization
    void Start ()
    {
        player = this.GetComponent<ResizingObjects>().Player;
        starFont = player.GetComponent<PlayerStatus>().starFont;
	}

    private void Update()
    {
        if (showChk)
        {
            chkTimer += Time.deltaTime;
        }

        if (!PlayerStatus.isAlive)
        {
            time -= Time.deltaTime;
        }
    }

    public static void Revive()
    {
        if (counter)
        {
            PlayerStatus.reviveProtection = true;
            PlayerStatus.heatAmount = 100;
            PlayerStatus.blueWarning = false;
            PlayerStatus.redWarning = false;
            PlayerStatus.orangeWarning = false;
            PlayerStatus.yellowWarning = false;
            PlayerStatus.isAlive = true;
            PlayerStatus.cameraFollow = true;
            Controller.ignoreKey = false;
            player.transform.position = savedPosition;
            player.GetComponent<PlayerStatus>().ResetLife();
            time = 5f;
            counter = false;
        }
	}

    private void OnGUI()
    {
        GUIStyle starStyle = new GUIStyle();
        starStyle.fontSize = GetScaledFontSize(35);
        starStyle.normal.textColor = new Color(0.01569f, 0.81569f, 0.86275f);
        //starStyle.alignment = TextAnchor.MiddleCenter;
        GUI.skin.font = starFont;

        if (showChk)
        {
            if ((int)chkTimer <= 2)
            {
                GUI.Label(ResizeGUI(new Rect(800, 60, 100, 100)), "Checkpoint reached", starStyle);
            }
            else
            {
                showChk = false;
            }
        }

        if (!PlayerStatus.isAlive)
        {
            int t = Mathf.FloorToInt(time);

            GUI.Label(ResizeGUI(new Rect(900, 120, 100, 100)), "Reviving in", starStyle);

            switch (t)
            {
                case 4:
                    content = "3";
                    break;
                case 3:
                    content = "2";
                    break;
                case 2:
                    content = "1";
                    break;
                case 1:
                    content = "0";
                    break;
                case 0:
                    counter = true;
                    break;
            }

            GUI.Label(ResizeGUI(new Rect(970, 160, 100, 100)), content, starStyle);
        }
    }

    Rect ResizeGUI(Rect _rect)
    {
        float FilScreenWidth = _rect.width / 1920;
        float rectWidth = FilScreenWidth * Screen.width;
        float FilScreenHeight = _rect.height / 1080;
        float rectHeight = FilScreenHeight * Screen.height;
        float rectX = (_rect.x / 1920) * Screen.width;
        float rectY = (_rect.y / 1080) * Screen.height;

        return new Rect(rectX, rectY, rectWidth, rectHeight);
    }

    private int GetScaledFontSize(int baseFontSize)
    {
        float uiScale = Screen.height / uiBaseScreenHeight;
        int scaledFontSize = Mathf.RoundToInt(baseFontSize * uiScale);
        return scaledFontSize;
    }

}