using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResultCanvasController : MonoBehaviour
{
    [SerializeField] RaceInfScript raceInfScript;

    [SerializeField] int NextRank = 1;
    [SerializeField] GameObject ResultPanel;
    [SerializeField] GameObject ResultButtonPanel;
    [SerializeField] GameObject RankRowPref;
    [SerializeField] GameObject GoalText;
    [SerializeField] GameObject GoalRankText;
    public GameObject GameManager;
    Ueda_GameManager gamemanager;


    [SerializeField] Toggle ToggleForResultPanel;

    /*
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
    }
    */

    private enum RankRowElement
    {
        RankText,
        NameText,
        TimeText
    }

    private void Start()
    {
        /*
        BoalGoalDetectorから実行する
        */
        gamemanager = GameManager.GetComponent<Ueda_GameManager>();

    }


    public IEnumerator ShowResult()//結果を表示する。一定時間ごボタンを表示する。
    {
        yield return new WaitForSeconds(5.0f);
        ResultPanel.SetActive(true);
        ToggleForResultPanel.gameObject.SetActive(true);
        yield break;
    }

    public IEnumerator ShowButton()
    {
        yield return new WaitForSeconds(3.0f);
        ResultButtonPanel.SetActive(true);
        yield break;
    }


    public void createResultRow(BoatDataScript.BoatData boat, int timeMinutes, float timeSeconds)//ゴールする度に結果を見えない場所で作っておく
    {
        
        //結果をboatのクラスオブジェクトに反映
        boat.rank = NextRank;
        NextRank++;
        boat.goalTimeMinutes = timeMinutes;
        boat.goalTimeSeconds = timeSeconds;

        //表に結果の列追加
        GameObject newRankRow = Instantiate(RankRowPref);
        newRankRow.transform.SetParent( ResultPanel.transform,false);
        newRankRow.transform.Find(RankRowElement.RankText.ToString() ).GetComponent<Text>().text =boat.rank.ToString();
        newRankRow.transform.Find(RankRowElement.NameText.ToString() ).GetComponent<Text>().text = boat.player1name + "\n" + boat.player2name;
        newRankRow.transform.Find(RankRowElement.TimeText.ToString() ).GetComponent<Text>().text = String.Format("{0:00}", timeMinutes) + ":" + String.Format("{0:00.00}", timeSeconds);

        if (boat.player1id == gamemanager.actorNumber || boat.player2id == gamemanager.actorNumber)//自分の順位を灰色に。ゴール時に表示する順位を設定
        {
            newRankRow.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
            GoalRankText.GetComponent<Text>().text = "第" + boat.rank.ToString() + "位";
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(ResultPanel.transform as RectTransform);//UI再構築
    }



    public IEnumerator ShowGoalText()
    {    
        GoalText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GoalText.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        GoalRankText.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        GoalRankText.SetActive(false);
        yield break;
    }


    
    public void ToggleForResultFunc()
    {
        if (ToggleForResultPanel.isOn)
        {
            ResultPanel.SetActive(true);
        }
        else
        {
            ResultPanel.SetActive(false);
        }
        
    }

    




}
