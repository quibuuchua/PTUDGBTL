using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<Character> characters = new List<Character>();
    [SerializeField] private List<Bot> bots = new List<Bot>();
    [SerializeField] private Stage[] stages;
    [SerializeField] private PoolControl PoolControl;
    [SerializeField] private List<Transform> finalLines = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        characters.Add(LevelManager.GetInstance.GetPlayer());

        for (int i = 0; i < characters.Count; i++)
        {
            LoadStage(characters[i], 0);
        }
    }

    public PoolControl GetPoolControl() => PoolControl;

    public void LoadStage(Character character, int characterStageIndex)
    {
        Platform currentPlatform = stages[characterStageIndex].GetCurrentStagePlatform();
        int brickAmount = currentPlatform.GetBrickAmount();
        PoolControl.PreLoadPool(character, brickAmount);
        currentPlatform.SpawnBrick(characterStageIndex, character);
    }

    public Platform GetCurrentStagePlatform(int characterStageIndex)
    {
        return stages[characterStageIndex].GetCurrentStagePlatform();
    }

    public List<Transform> GetFinalLines() => finalLines;

    public void SetPatrolStateBot()
    {
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].ChangeState(new PatrolState());
        }
    }

    public List<Transform> GetNextStageLines(Character character)
    {
        return GetCurrentStagePlatform(character.GetCurrentStageIndex()).GetNextStageLine();
    }

    public void EnablePlatformGate(Character character)
    {
        GetCurrentStagePlatform(character.GetCurrentStageIndex()).EnableGate();
    }

    //public void StopRespawnBrick()
    //{
    //    for (int i = 0; i < characters.Count; i++)
    //    {
    //        //StopCoroutine(characters[i].ReSpawnBrick(characters[i].CurrentCharacterColor(), BrickPool));
    //        Queue<Brick> inactiveBricks = BrickPool.GetPool(characters[i].CurrentCharacterColor()).InactiveBricks();
    //        while (inactiveBricks.Count > 0)
    //        {
    //            StopCoroutine(characters[i].ReSpawnBrick(characters[i].CurrentCharacterColor(), inactiveBricks.Dequeue().transform.position, Quaternion.identity));
    //        }
    //    }
    //}
}
