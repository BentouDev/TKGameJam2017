using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : GameState
{
    public override void OnStart()
    {
        MainGame.Instance.GUI.Fade.FadePlayer.Play();
    }
}
