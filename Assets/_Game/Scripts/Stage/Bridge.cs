using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private Stair stair;
    [SerializeField] private Barrier barrier;
    [SerializeField] private int totalStairNumbers;
    [SerializeField] private Transform stairParent;
    [SerializeField] private Transform barrierParent;

    private List<Stair> stairs = new List<Stair>();
    private List<Barrier> barriers = new List<Barrier>();
    private Vector3 firstStairPos;
    private Vector3 firstBarrierPos;
    private int count = 1;
    private int totalStairsActive = 0;


    // Start is called before the first frame update
    void Start()
    {
        firstStairPos = stair.transform.position;
        firstBarrierPos = barrier.transform.position;
        stairs.Add(stair);
        barriers.Add(barrier);
        for (int i = 1; i < totalStairNumbers; i++)
        {
            Stair stair = Instantiate(this.stair, new Vector3(
                firstStairPos.x,
                firstStairPos.y + count * Constants.STAIR_DISTANCE_Y,
                firstStairPos.z + count * Constants.STAIR_DISTANCE_Z), this.stair.transform.rotation);
            stair.transform.SetParent(stairParent);
            stairs.Add(stair);


            Barrier barrier = Instantiate(this.barrier, new Vector3(
                firstBarrierPos.x,
                firstBarrierPos.y + count * Constants.STAIR_DISTANCE_Y,
                firstBarrierPos.z + count * Constants.STAIR_DISTANCE_Z), this.barrier.transform.rotation);
            barrier.transform.SetParent(barrierParent);
            barriers.Add(barrier);

            count++;    
        }
    }

    public int GetStairIndex(Stair stair)
    {
        return stairs.IndexOf(stair);
    }

    public void EnableBarrierBox(int index)
    {
        if (index + 1 < barriers.Count)
        {
            barriers[index + 1].GetObjectBoxCollider().isTrigger = false;
        }
    }

    public void IncreaseStairActive()
    {
        totalStairsActive++;
    }

    public bool IsEnoughStairForBridge()
    {
        return totalStairsActive == totalStairNumbers;
    }

    public void ResetBarrier()
    {
        for (int i = 0; i < barriers.Count; i++)
        {
            barriers[i].GetObjectBoxCollider().isTrigger = true;
        }
    }

    public void ResetCurrentBarrier(int index)
    {
        barriers[index].GetObjectBoxCollider().isTrigger = true;
    }
}
