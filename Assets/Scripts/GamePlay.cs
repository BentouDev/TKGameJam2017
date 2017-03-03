using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : GameState
{
    public override void OnUpdate()
    {
        Game.Player.OnUpdate();

        if(Game.Player.Pawn.IsAlive)
            Game.EndGame();
    }
}
