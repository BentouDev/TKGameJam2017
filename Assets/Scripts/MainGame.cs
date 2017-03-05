using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    public static MainGame Instance;

    public SceneLoader Loader;
    public PlayerController Player;
    public AIController Enemy;
    public GUIController GUI;
    public MessageBoxController MessageController;

    public LevelController CurrentLevel { get; set; }

    public List<GameState> States { get; private set; }

    public GameState CurrentState { get; private set; }

    [Header("Next Level")]
    public float DissolveAnimTime = 4;
    public float PostFadeDelay = 2;

    void Start()
    {
        Instance = this;

        States = FindObjectsOfType<GameState>().ToList();

        foreach (GameState state in States)
        {
            state.Init(this);
        }

        Loader.OnSceneLoad += StartGame;
    }

    public void EndGame()
    {
        if(!(CurrentState is GameEnd))
            ChangeState<GameEnd>();
    }

    public void LoadLevel(int index)
    {
        Loader.StartLoadScene(index);
    }

    public void StartGame()
    {
        CurrentLevel = FindObjectOfType<LevelController>();

        if (!(CurrentState is GameStart))
            ChangeState<GameStart>();
    }

    public void ExitGame()
    {
        foreach (var p in FindObjectsOfType<Persistent>())
        {
            p.DestroyOnExit();
        }
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

    public void NextLevel()
    {
        StartCoroutine(NextLevelLoadTime());
    }

    IEnumerator NextLevelLoadTime()
    {
        CurrentLevel.LevelEnd.Play();

        yield return new WaitForSeconds(DissolveAnimTime);

        GUI.Fade.FadePlayer.Play();

        yield return new WaitForSeconds(PostFadeDelay);

        if (Loader.CurrentScene + 1 == SceneManager.sceneCount)
        {
            Loader.StartLoadScene(Loader.CurrentScene + 1);
        }
        else
        {
            Restart();
        }
    }

    public void Restart()
    {
        ExitGame();
        
        SceneManager.LoadScene(0);
    }
}
