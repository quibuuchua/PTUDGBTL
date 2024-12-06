using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] public Transform finishBox;
    [SerializeField] private int minCollectedBricks;
    [SerializeField] private int maxCollectedBricks;

    private int collectedBrick;
    private IState currentState;
    private Vector3 currentTargetPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null && GameManager.GetInstance.CurrentState(GameState.GamePlay))
        {
            currentState.OnExecute(this);
        }

        if (!GameManager.GetInstance.CurrentState(GameState.GamePlay))
        {
            SetDestination(transform.position);
            ChangeState(new IdleState());
            return;
        }
    }

    public override void OnInit()
    {
        base.OnInit();

        collectedBrick = Random.Range(GetMinCollectedBrick(), GetMaxCollectedBrick());

        ChangeState(new IdleState());
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public void Moving()
    {
        if (IsReachTarget())
        {
            SetDestination(RandomBrickPos());
        }
    }
   

    public int GetMinCollectedBrick()
    {
        return minCollectedBricks;
    }

    public int GetMaxCollectedBrick()
    {
        return maxCollectedBricks;
    }
    
    public bool BuildBridge()
    {
        return GetCurrentTotalBricks() >= collectedBrick;
    }

    public bool IsReachTarget()
    {
        return Vector3.Distance(transform.position, currentTargetPosition) < 0.8f;
    }

    public void SetDestination(Vector3 pos)
    {
        currentTargetPosition = pos;
        navMeshAgent.destination = pos;
    }

    public void CheckStair(Stair stair)
    {
        // check neu bot dang co brick
        if (GetCurrentTotalBricks() > 0)
        {
            // neu mau stair khac mau bot
            if (stair.GetColorType() != CurrentCharacterColor())
            {
                NormalStairChecking(stair.Bridge(), stair);
            }
            stair.Bridge().ResetCurrentBarrier(stair.Bridge().GetStairIndex(stair));
            stair.StairMeshRenderer().enabled = true;

            // neu bridge chua du active stair va bot het brick
            if (!stair.Bridge().IsEnoughStairForBridge() && GetCurrentTotalBricks() == 0)
            {
                ChangeState(new PatrolState());
            }
        }
        else
        {
            // check neu bot het gach va mau stair dang khac mau bot
            if (stair.GetColorType() != CurrentCharacterColor())
            {
                ChangeState(new PatrolState());
            }
        }
    }

    public Vector3 RandomBrickPos()
    {
        List<Vector3> bricksPos = LevelManager.GetInstance.CurrentLevel().GetCurrentStagePlatform(GetCurrentStageIndex()).GetPlatformBrickPos()[this];
        return bricksPos[Random.Range(0, bricksPos.Count)];
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.TAG_DOOR))
        {
            Door door = Cache.GetDoor(other);
            if (door.IsNextStageDoor())
            {
                ProcessToNextStage();
                LevelManager.GetInstance.CurrentLevel().LoadStage(this, GetCurrentStageIndex());
                SetDestination(RandomBrickPos());
            }
        }
    }

}
