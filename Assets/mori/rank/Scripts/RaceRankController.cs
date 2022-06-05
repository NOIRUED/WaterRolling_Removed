using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class RaceRankController : MonoBehaviour
{
    
    List<BoatDataScript.BoatData> boatDataList=new List<BoatDataScript.BoatData>();

    [Header("BoatPosition")]
    [SerializeField] GameObject BoatPosPanel;//BoatPosMarkの親
    [SerializeField] GameObject BoatPosMarkPref;//boatの表示。①,②など
    RectTransform[] BoatPosMarkRectArray;//BoatPosMarkのRectTransform
    [SerializeField] int RaceWay_v = 500;//表示上のレースの長さ
    [SerializeField] int MaxRacePosDitectorNum = 33;//チェックポイントの合計数
    int boatNum = 0;//存在する船の数
    private enum BoatPosMarkChild//BoatPosMarkの子要素
    {
        Text
    }

    [Header("MyRankRow")]
    [SerializeField] RaceInfScript raceInfScript;
    [SerializeField] Text MyRankText;
    BoatDataScript.BoatData MyBoatData=new BoatDataScript.BoatData();


    [Header("RealTimeRankShow")]
    [SerializeField] GameObject RealTimeRankPanel;//ReaiTimeRankを表示するpanel
    [SerializeField] GameObject RealTimeRankRowPref;//RealTimeRankRowのPreffab
    private Text[] RealTimeRankRows_Name1;
    private Text[] RealTimeRankRows_Name2;
    bool RealTimeRankChanged = false;
    bool RealTimeRankUpdating = false;
    private enum RealTimeRankRowChild
    {
        RankText,
        Name1Text,
        Name2Text
    }



    private void Start()
    {
        BoatPosMarkRectArray = new RectTransform[3];
        RealTimeRankRows_Name1 = new Text[3];
        RealTimeRankRows_Name2 = new Text[3];
    }

    public void JudgeRank(BoatDataScript.BoatData boatData, int racePos)
    {
        if (!boatDataList.Contains(boatData))//船がまだ一度もチェックポイント(race pos ditector)を通ってない時。可能であればこれだけをスタート位置のトリガーに設定したい(?)
        {
            
            boatNum++; //boatの数を１つ増やす
            boatDataList.Add(boatData);//船のデータを確保

            //BoatPosMarkを生成する。及びそのRectTransformを操作するための準備
            GameObject BoatPosMark = Instantiate(BoatPosMarkPref,Vector3.zero,Quaternion.identity, BoatPosPanel.transform);
            BoatPosMark.transform.Find(BoatPosMarkChild.Text.ToString()).GetComponent<Text>().text = (boatNum).ToString();
            BoatPosMarkRectArray[boatNum-1] = BoatPosMark.GetComponent<RectTransform>();
            for(int i = boatNum-1; i > -1; i--) BoatPosMarkRectArray[i].SetAsLastSibling();//最前面を変更。1位を最前面に

            //自分の船のrankを取得。(MyRankを表示する準備。)
            if (boatData.player1id == raceInfScript.MyID || boatData.player2id == raceInfScript.MyID) MyBoatData = boatData;

            

            //RealTimeRankRowを生成する。及びそのTextを操作するための準備
            GameObject RealTimeRankRow = Instantiate(RealTimeRankRowPref, Vector3.zero, Quaternion.identity, RealTimeRankPanel.transform);
            RealTimeRankRow.transform.Find(RealTimeRankRowChild.RankText.ToString()).GetComponent<Text>().text = (boatNum).ToString()+"位";
            RealTimeRankRows_Name1[boatNum - 1] = RealTimeRankRow.transform.Find(RealTimeRankRowChild.Name1Text.ToString()).GetComponent<Text>();
            RealTimeRankRows_Name1[boatNum-1].text = boatDataList[boatNum - 1].player1name;
            RealTimeRankRows_Name2[boatNum - 1] = RealTimeRankRow.transform.Find(RealTimeRankRowChild.Name2Text.ToString()).GetComponent<Text>();
            RealTimeRankRows_Name2[boatNum-1].text = boatDataList[boatNum - 1].player2name;
        }

        //racePosで並び替え
        boatDataList = boatDataList.OrderByDescending(a => a.racePos).ToList();

        //rankを設定。boatの位置を直線状の場所で表示
        for (int i = 0; i < boatNum; i++)
        {
            if (boatDataList[i].rank != i + 1) RealTimeRankChanged = true;//順位に変更があった

            boatDataList[i].rank = i + 1;//boatにrankをつける。
            BoatPosMarkRectArray[i].anchoredPosition = new Vector2(0, RaceWay_v * boatDataList[i].racePos/ MaxRacePosDitectorNum);//boatのpositionをUIに表示(直線上)
        }

        //順位に変更があった時、RealTimeRankPanelの内容を変更する
        if (RealTimeRankChanged)
        {
            if (RealTimeRankUpdating) return;//rankをupdate中なら変更をそのまま任せる
            StartCoroutine(UpdateRealTimeRank());
            RealTimeRankChanged = false;
        }
    }


    private IEnumerator UpdateRealTimeRank()
    {
        RealTimeRankUpdating = true;
        MyRankText.text = "  " + "位";
        for (int i = 0; i < boatNum; i++)
        {
            RealTimeRankRows_Name1[i].text ="";
            RealTimeRankRows_Name2[i].text = "";
        }
        yield return new WaitForSeconds(0.5f);
        MyRankText.text = MyBoatData.rank.ToString() + "位";

        for (int i = 0; i < boatNum; i++)
        {
            RealTimeRankRows_Name1[i].text = boatDataList[i].player1name;
            RealTimeRankRows_Name2[i].text = boatDataList[i].player2name;
        }
        yield return new WaitForSeconds(0.5f);
        RealTimeRankUpdating = false;
        yield break;
    }
}

