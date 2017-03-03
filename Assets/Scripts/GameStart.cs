using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : GameState
{
    public override void OnStart()
    {
        if (!Game.Player)
            Game.Player = FindObjectOfType<PlayerController>();

        Game.Player.OnStart();

        Game.ChangeState<GamePlay>();
    }
}
