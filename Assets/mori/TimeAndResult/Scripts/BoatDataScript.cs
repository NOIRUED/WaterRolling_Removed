using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatDataScript : MonoBehaviour
{
    [System.Serializable]


    public class BoatData//各船につける
    {
        public int boatID;
        public int rank;
        public string player1name;
        public int player1id;
        public string player2name;
        public int player2id;
        public int goalTimeMinutes = 0;
        public float goalTimeSeconds = 0.0f;//ゴールしていなければminites=seconds=0,していた場合ゴールまでの経過分数及び秒数
        public int racePos = 0;
    }

    [SerializeField] public BoatData boatData = new BoatData();

    
    private void Start()
    {
        //boatDataの値をなんとか決定する。photonでできる...?
        /*
        boatData.player1name
         */
    }
}
