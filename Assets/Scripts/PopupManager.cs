using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;

    public TMP_Text text_title;

    public GameObject popUp;

    private void Awake()
    {
        instance = this;
    }

    public void Popup_Show(string title)
    {
        text_title.text = title;
        popUp.SetActive(true);
    }
    public void Popup_Close()
    {
        popUp.SetActive(false);
    }

}
/*public class PopupContent
{
    public string title;
    public string text;
    public Sprite icon;
    public PopupContent_Button[] buttons;

    public PopupContent(string popup_title, string popup_text, Sprite popup_icon, PopupContent_Button[] popup_buttons)
    {
        title = popup_title;
        text = popup_text;
        icon = popup_icon;
        buttons = popup_buttons;
    }
}

public class PopupContent_Button
{
    public string text = "OK";
    public string function = "";
    public MonoBehaviour function_caller;

    //shows default ok button
    public PopupContent_Button()
    {
        text = "OK";
        function = "";
        function_caller = null;
    }

    //show special button
    public PopupContent_Button(string button_text, string button_function, MonoBehaviour button_function_caller)
    {
        text = button_text;
        function = button_function;
        function_caller = button_function_caller;
    }
}*/
