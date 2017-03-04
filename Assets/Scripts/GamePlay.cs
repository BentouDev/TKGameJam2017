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
        MainGame.Instance.GUI.Fade.UnfadePlayer.Play();
    }

    public override void OnUpdate()
    {
        Game.Player.OnUpdate();

        if(Started)
            Game.Enemy.OnUpdate();

        if(!Game.Player.Pawn.IsAlive || !Game.Enemy.IsAlive())
            Game.EndGame();
    }

    IEnumerator Counter()
    {
        yield return new WaitForSeconds(EnemySpawnDelay);

        Started = true;
        Game.Enemy.OnStart();
    }
}
