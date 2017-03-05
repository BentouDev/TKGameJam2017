using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : GameState
{
    public float StartDelay;
    public float MessageDelay;
    public AnimationPlayer StartGUIAnim;

    public string StartMessage;
    public Color StartColor;

    public override void OnStart()
    {
        if (!Game.Player)
            Game.Player = FindObjectOfType<PlayerController>();

        if (!Game.Enemy)
            Game.Enemy = FindObjectOfType<AIController>();

        var levelController = FindObjectOfType<LevelController>();
        if (levelController)
        {
            levelController.LevelStart.Play();
        }

        MainGame.Instance.GUI.Fade.UnfadePlayer.Play();

        Game.Player.OnStart();

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(StartDelay);

        MainGame.Instance.MessageController.SetMessage(StartMessage, StartColor);
        MainGame.Instance.MessageController.ShowAnim.Play();

        StartCoroutine(HideMessage());
    }

    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(MessageDelay);

        PlayGame();
    }

    public void PlayGame()
    {
        MainGame.Instance.MessageController.HideAnim.Play();
        Game.ChangeState<GamePlay>();
    }
}
