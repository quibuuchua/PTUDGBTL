using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Platform platform;

    public Platform GetCurrentStagePlatform() => platform;
}
