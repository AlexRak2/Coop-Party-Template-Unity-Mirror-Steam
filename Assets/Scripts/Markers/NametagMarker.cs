using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NametagMarker : Marker
{
    public TMP_Text username_text, ready_text;
    public Image ready_image, pfp_Image;

    public void UpdateTag(string username, bool isReady) 
    {
        username_text.text = username;
        ready_image.color = isReady ? MainMenu.instance.readyColor : MainMenu.instance.notReadyColor;
        ready_text.text = isReady ? "Ready" : "Not Ready";
        ready_text.color = isReady ? MainMenu.instance.readyColor : MainMenu.instance.notReadyColor;
    }

    public void UpdatePFP(Sprite icon)
    {
        pfp_Image.sprite = icon;
    }
}
