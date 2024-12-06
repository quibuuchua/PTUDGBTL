using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSettings : UICanvas
{
    //[SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject closeButton;

    public void SetState(UICanvas canvas)
    {
        mainMenuButton.SetActive(false);
        continueButton.SetActive(false);
        closeButton.SetActive(false);

        if (canvas is CanvasMainMenu)
        {
            closeButton.SetActive(true);
        }
        else if (canvas is CanvasGamePlay)
        {
            mainMenuButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(true);
        }
    }

    public void MainMenuButton()
    {
        Close(0);
        UIManager.GetInstance.CloseAll();
        UIManager.GetInstance.OpenUI<CanvasMainMenu>();
        GameManager.GetInstance.UpdateGameState(GameState.MainMenu);
    }

    public void ContinueButton()
    {
        Close(0);
        UIManager.GetInstance.OpenUI<CanvasGamePlay>();
        GameManager.GetInstance.UpdateGameState(GameState.GamePlay);
        LevelManager.GetInstance.OnPlay();
    }

    public void CloseButton()
    {
        Close(0);
        GameManager.GetInstance.UpdateGameState(GameState.MainMenu);
    }
}
