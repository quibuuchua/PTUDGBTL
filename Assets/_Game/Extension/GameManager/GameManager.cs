using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    MainMenu,
    GamePlay,
    Finish,
    Revive,
    Setting
}

public class GameManager : Singleton<GameManager>
{
    private static GameState gameState;

    public static void ChangeState(GameState state)
    {
        gameState = state;
    }

    public static bool IsState(GameState state) => gameState == state;

    private void Awake()
    {
        // tranh viec nguoi choi cham da diem vao man hinh
        Input.multiTouchEnabled = false;
        // target frame rate ve 60 fps
        Application.targetFrameRate = 60;
        // tranh viec tat man hinh
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // xu ly tai tho
        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManager.GetInstance.OpenUI<CanvasMainMenu>();
    }

    public void UpdateGameState(GameState state)
    {
        ChangeState(state);
    }

    public bool CurrentState(GameState state) => IsState(state);

}
