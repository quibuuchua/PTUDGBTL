using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private FloatingJoystick floatingJoystick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] Transform tf;
    [SerializeField] private float playerHeight;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private LayerMask bridgeLayer;

    private Quaternion originRotation;
    private RaycastHit slopeHit;
    private Vector3 moveDirection;
    private float inputX;
    private float inputZ;

    // Start is called before the first frame update
    void Start()
    {
        originRotation = transform.rotation;
        OnInit();
    }

    private void Update()
    {
        if (GameManager.GetInstance.CurrentState(GameState.GamePlay))
        {
            Move();
            rb.useGravity = !OnSlope();
        }
        else
        {
            ChangeAnimation(Constants.ANIMATION_IDLE);
            return;
        }
        AdvanceToNextStage();
    }

    public override void OnInit()
    {
        base.OnInit();
        ChangeAnimation(Constants.ANIMATION_IDLE);
        tf.position = Vector3.zero;
        tf.rotation = Quaternion.identity;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(tf.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 1f, bridgeLayer))
        {
            Debug.DrawLine(tf.position, tf.position + Vector3.down * playerHeight, Color.yellow);
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public void Move()
    {
        inputX = floatingJoystick.Horizontal;
        inputZ = floatingJoystick.Vertical;

        moveDirection = new Vector3(inputX * GetMoveSpeed(), 0f, inputZ * GetMoveSpeed());

        if (!rb.isKinematic)
        {
            if (OnSlope())
            {
                rb.velocity = new Vector3(GetSlopeMoveDirection().x * 5f, GetSlopeMoveDirection().y * 5f - 1f, GetSlopeMoveDirection().z * 5f);
            }
            else
            {
                rb.velocity = moveDirection;
            }
        }

        if (!floatingJoystick.IsResetJoystick())
        {
            rb.isKinematic = false;
            ChangeAnimation(Constants.ANIMATION_RUN);
        }
        else
        {
            rb.isKinematic = true;
            ChangeAnimation(Constants.ANIMATION_IDLE);
        }

        if (inputX != 0f && inputZ != 0f)
        {
            if (OnSlope())
            {
                tf.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, originRotation.y, rb.velocity.z));
            }
            else
            {
                tf.rotation = Quaternion.LookRotation(rb.velocity);
            }
        }
        
    }

    public void AdvanceToNextStage()
    {
        // TODO fix:
        List<Transform> nextStageLines = LevelManager.GetInstance.CurrentLevel().GetNextStageLines(this);
        List<Transform> finalStageLines = LevelManager.GetInstance.CurrentLevel().GetFinalLines();
        if (IsClearStage(nextStageLines) && nextStageLines.Count > 0)
        {
            for (int i = 0; i < nextStageLines.Count; i++)
            {
                nextStageLines[i].gameObject.SetActive(false);
            }
            LevelManager.GetInstance.CurrentLevel().EnablePlatformGate(this);
            ProcessToNextStage();
            LevelManager.GetInstance.CurrentLevel().LoadStage(this, GetCurrentStageIndex());
        }
        else if (IsClearStage(finalStageLines))
        {
            for (int i = 0; i < nextStageLines.Count; i++)
            {
                finalStageLines[i].gameObject.SetActive(false);
            }
            LevelManager.GetInstance.CurrentLevel().EnablePlatformGate(this);
        }
    }

    public bool IsClearStage(List<Transform> nextStageLines)
    {
        for (int i = 0; i < nextStageLines.Count; i++)
        {
            if (Vector3.Distance(tf.position, nextStageLines[i].position) < 0.8f)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckStair(Stair stair)
    {
        // check neu player dang co brick
        if (GetCurrentTotalBricks() > 0)
        {
            // neu mau stair khac voi mau player
            if (stair.GetColorType() != CurrentCharacterColor())
            {
                NormalStairChecking(stair.Bridge(), stair);
            }
            stair.StairMeshRenderer().enabled = true;

            // neu ma bridge chua du active stair va play khong con gach
            if (!stair.Bridge().IsEnoughStairForBridge() && GetCurrentTotalBricks() == 0)
            {
                stair.Bridge().EnableBarrierBox(stair.Bridge().GetStairIndex(stair));
            }
        }
        else
        {   
            // neu player het gach va mau stair dang khac voi mau player
            if (stair.GetColorType() != CurrentCharacterColor())
            {
                stair.Bridge().EnableBarrierBox(stair.Bridge().GetStairIndex(stair));
            }
        }
    }
}
