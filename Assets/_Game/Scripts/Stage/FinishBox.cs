using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
        {
            LevelManager.GetInstance.OnReset();
            UIManager.GetInstance.CloseUI<CanvasGamePlay>(0);
            UIManager.GetInstance.OpenUI<CanvasVictory>();
            GameManager.GetInstance.UpdateGameState(GameState.Finish);
        }
        else if (other.CompareTag(Constants.TAG_BOT))
        {
            LevelManager.GetInstance.OnReset();
            UIManager.GetInstance.CloseUI<CanvasGamePlay>(0);
            UIManager.GetInstance.OpenUI<CanvasFail>();
            GameManager.GetInstance.UpdateGameState(GameState.Finish);
        }
    }
}
