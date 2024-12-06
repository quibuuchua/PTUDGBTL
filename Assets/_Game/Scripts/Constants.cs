using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public const string ANIMATION_IDLE = "idle";
    public const string ANIMATION_RUN = "run";
    public const string ANIMATION_VICTORY = "victory";

    public const string TAG_PLAYER = "Player";
    public const string TAG_BOT = "Bot";
    public const string TAG_STAIR = "Stair";
    public const string TAG_BRICK = "Brick";
    public const string TAG_BRIDGE = "Bridge";
    public const string TAG_DOOR = "Door";
    public const string TAG_FINISH_BOX = "FinishBox";

    public const float STAIR_DISTANCE_Y = 0.2f;
    public const float STAIR_DISTANCE_Z = 0.3f;
}
