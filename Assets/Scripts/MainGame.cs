using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public PlayerController Player;

    public List<GameState> States { get; private set; }

    public GameState CurrentState { get; private set; }

    void Start()
    {
        States = FindObjectsOfType<GameState>().ToList();

        foreach (GameState state in States)
        {
            state.Init(this);
        }

        StartGame();
    }

    public void EndGame()
    {
        if(!(CurrentState is GameEnd))
            ChangeState<GameEnd>();
    }

    public void StartGame()
    {
        if (!(CurrentState is GameStart))
            ChangeState<GameStart>();
    }

    public void ChangeState<T>() where T : GameState
    {
        if (CurrentState != null) CurrentState.OnEnd();

        CurrentState = States.FirstOrDefault(t => t is T);

        if(CurrentState != null) CurrentState.OnStart();
    }

    private void Update()
    {
        if(CurrentState) CurrentState.OnUpdate();
    }
}
