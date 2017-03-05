using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : GameState
{
    public float FadeDelay;

    public string LostMessage;
    public Color LostColor;

    public float PostDelayFade = 2;

    public override void OnStart()
    {
        MainGame.Instance.MessageController.SetMessage(LostMessage, LostColor);

        MainGame.Instance.MessageController.ShowAnim.Play();
        
        StartCoroutine(DelayFadeOnMessage());
    }

    IEnumerator DelayFadeOnMessage()
    {
        yield return new WaitForSeconds(FadeDelay);
        MainGame.Instance.MessageController.HideAnim.Play();
        MainGame.Instance.GUI.Fade.FadePlayer.Play();

        StartCoroutine(PostFade());
    }

    IEnumerator PostFade()
    {
        yield return new WaitForSeconds(PostDelayFade);

        Game.Restart();
    }
}
