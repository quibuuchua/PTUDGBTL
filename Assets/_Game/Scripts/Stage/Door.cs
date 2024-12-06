using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool isNextStageDoor;
    [SerializeField] private BoxCollider BoxCollider;

    public bool IsNextStageDoor() => isNextStageDoor;

}
