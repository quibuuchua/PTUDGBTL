using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolControl : MonoBehaviour
{
    [SerializeField] private Brick brick;

    public void PreLoadPool(Character character, int platformBrickAmount)
    {
        brick.ChangeColor(character.CurrentCharacterColor());
        brick.ChangeMaterial(character.GetCurrentMeshMaterial());
        GameObject pool = new GameObject(LevelManager.GetInstance.CurrentLevel().name + "_" + character.CurrentCharacterColor().ToString() + "_" + (character.GetCurrentStageIndex() + 1));
        BrickPool.PreLoad(brick,
                          platformBrickAmount,
                          pool.transform);
    }

}




