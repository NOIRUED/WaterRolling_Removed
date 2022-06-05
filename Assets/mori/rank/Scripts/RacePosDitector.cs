using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacePosDitector : MonoBehaviour
{
    [SerializeField] RaceRankController raceRankController;
    [SerializeField] int RacePos = 0;

    private void OnTriggerEnter(Collider other)
    {
        BoatDataScript.BoatData boatData = other.gameObject.GetComponent<BoatDataScript>().boatData;
        if (boatData.racePos > RacePos) return;
        boatData.racePos = RacePos;
        raceRankController.JudgeRank( boatData , RacePos);
    }
}
