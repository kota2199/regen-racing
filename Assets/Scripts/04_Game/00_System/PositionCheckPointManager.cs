using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCheckPointManager : MonoBehaviour
{
    public int checkpointIndex; // チェックポイントの番号

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            PositionCounter positionCounter = other.GetComponent<PositionCounter>();
            if (positionCounter != null)
            {
                positionCounter.UpdateCheckpoint(checkpointIndex);
            }
        }
    }
}
