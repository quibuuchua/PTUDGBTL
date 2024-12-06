using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] private DataSO data;
    [SerializeField] private Animator animator;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float firstBrickY = -0.5f;
    [SerializeField] private float firstBrickZ = -0.6f;
    [SerializeField] private CharacterBrick characterBrickPrefab;

    private List<CharacterBrick> bricks = new List<CharacterBrick>();
    private IState currentState;
    private CommonEnum.ColorType currentColorType;
    private string currentAnimationName = Constants.ANIMATION_IDLE;
    private int currentStageIndex = 0;

    private void Awake()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        currentStageIndex = 0;
        GetData();
        bricks = new List<CharacterBrick>();
    }

    public virtual void OnDespawn()
    {
        ClearBrick();

    }

    public float GetMoveSpeed() => moveSpeed;

    public void GetData()
    {
        currentColorType = data.color;
        skinnedMeshRenderer.material = data.GetMaterial(currentColorType);
    }

    public CommonEnum.ColorType CurrentCharacterColor() => currentColorType;

    public Material GetCurrentMeshMaterial() => skinnedMeshRenderer.material;

    public int GetCurrentStageIndex() => currentStageIndex; 

    public void ProcessToNextStage()
    {
        currentStageIndex++;
    }

    public void ChangeAnimation(string animationName)
    {
        if (currentAnimationName != animationName)
        {
            animator.ResetTrigger(currentAnimationName);
            currentAnimationName = animationName;
            animator.SetTrigger(currentAnimationName);
        }
    }

    public void AddBrick()
    {
        CharacterBrick characterBrick = Instantiate(characterBrickPrefab);
        characterBrick.ChangeColor(currentColorType);
        characterBrick.ChangeMaterial(GetCurrentMeshMaterial());
        bricks.Add(characterBrick);
        StackBrick();
    }

    public void RemoveBrick(CharacterBrick brick)
    {
        if (bricks.Count > 0)
        {
            bricks.Remove(brick);
            Destroy(brick.gameObject);
        }
    }

    public void ClearBrick()
    {
        if (bricks.Count > 0)
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                Destroy(bricks[i].gameObject);
            }
        }
        bricks.Clear();
    }

    public CharacterBrick GetLastBrick() => bricks.Count > 0 ? bricks[bricks.Count - 1] : null;

    public int GetCurrentTotalBricks() => bricks.Count;


    public void StackBrick()
    {
        Transform brickTransform = bricks[bricks.Count - 1].transform;
        brickTransform.SetParent(transform);
        brickTransform.rotation = transform.rotation;
        if (bricks.Count == 1)
        {
            brickTransform.localPosition = new Vector3(0f, firstBrickY, firstBrickZ);
        }
        else
        {
            brickTransform.localPosition = new Vector3(0f, firstBrickY + (bricks.Count - 1) * 0.3f, firstBrickZ);
        }
    }


    public Vector3 GetRandomResetPointPos()
    {
        Transform[] resetPoints = LevelManager.GetInstance.CurrentLevel().GetCurrentStagePlatform(currentStageIndex).GetResetPointPos();
        return resetPoints[Random.Range(0, resetPoints.Length)].position;
    }


    public void NormalStairChecking(Bridge bridge, Stair stair)
    {
        CharacterBrick brick = GetLastBrick();
        stair.ChangeColor(CurrentCharacterColor());
        stair.ChangeMaterial(GetCurrentMeshMaterial());
        RemoveBrick(brick);
        bridge.IncreaseStairActive();
    }

    public void NormalBrickChecking(Brick brick)
    {
        if (currentColorType == brick.GetColorType() && GameManager.GetInstance.CurrentState(GameState.GamePlay))
        {
            AddBrick();
            BrickPool.Despawn(brick);
            StartCoroutine(ReSpawnBrick(CurrentCharacterColor(), brick.TF.position, brick.TF.rotation));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_BRICK))
        {
            Brick brick = Cache.GenBrick(other);
            NormalBrickChecking(brick);
        }
    }

    public IEnumerator ReSpawnBrick(CommonEnum.ColorType colorType, Vector3 pos, Quaternion ros)
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        Brick brick = BrickPool.Spawn<Brick>(colorType, pos, ros);
    }
}
