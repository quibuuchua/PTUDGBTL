using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    public void PlayButton()
    {
        Close(0);
        UIManager.GetInstance.OpenUI<CanvasGamePlay>();
        GameManager.GetInstance.UpdateGameState(GameState.GamePlay);
        LevelManager.GetInstance.OnPlay();
    }

    public void SettingsButton()
    {
        UIManager.GetInstance.OpenUI<CanvasSettings>().SetState(this);
        GameManager.GetInstance.UpdateGameState(GameState.Setting);
    }
}
