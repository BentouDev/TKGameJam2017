using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : GameState
{
    public float EnemySpawnDelay = 5.0f;

    public bool Started;

    public override void OnStart()
    {
        StartCoroutine(Counter());
    }

    public override void OnUpdate()
    {
        Game.Player.OnUpdate();

        if(Started)
            Game.Enemy.OnUpdate();

        if(!Game.Player.Pawn.IsAlive)
            Game.EndGame();
    }

    IEnumerator Counter()
    {
        yield return new WaitForSeconds(EnemySpawnDelay);

        Started = true;
        Game.Enemy.OnStart();
    }
}
