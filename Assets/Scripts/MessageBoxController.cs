using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxController : MonoBehaviour
{
    public AnimationPlayer ShowAnim;
    public AnimationPlayer HideAnim;

    public Text MessageText;

    public void SetMessage(string text, Color color)
    {
        MessageText.text = text;
        MessageText.color = color;
    }

    public void Show()
    {
        ShowAnim.Play();
    }

    public void Hide()
    {
        HideAnim.Play();
    }
}
