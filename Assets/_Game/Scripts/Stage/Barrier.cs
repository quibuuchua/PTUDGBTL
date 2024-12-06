using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private BoxCollider BoxCollider;

    public BoxCollider GetObjectBoxCollider()
    {
        return BoxCollider;
    }
}
