using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWon : GameState
{
    public string WinMessage;
    public Color WinColor;

    public float MessageDelay = 2.0f;

    public override void OnStart()
    {
        MainGame.Instance.MessageController.SetMessage(WinMessage, WinColor);
        MainGame.Instance.MessageController.Show();

        StartCoroutine(HideMessage());
    }
    
    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(MessageDelay);
        MainGame.Instance.MessageController.Hide();

        if (Game.CurrentLevel)
        {
            Game.CurrentLevel.OpenGates();
        }
    }

    public override void OnUpdate()
    {
        if (Game.CurrentLevel && !Game.CurrentLevel.Entered())
        {
            Game.Player.OnUpdate();
        }
    }
}
