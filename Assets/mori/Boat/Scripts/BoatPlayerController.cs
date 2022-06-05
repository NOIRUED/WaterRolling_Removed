using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatPlayerController : MonoBehaviour
{
    //boat
    [SerializeField] GameObject boatObj;
    [SerializeField] BoatMoveController boatMoveController;

    //人の動き
    [SerializeField] private Animator playerAnimator;

    //フリック関連
    [SerializeField] Vector3 TouchStartPos;
    [SerializeField] Vector3 TouchEndPos;
    [SerializeField] float SwipeDistance = 1.0f;//スワイプを判定する距離


    


    private void Update()
    {
        if (rowing) return;//漕いでいる時は何もしない
        if (boatMoveController.isUpsideDown) return;//船が転覆しているときは何もしない

        //右矢印押せば右方向へ、左矢印押せば左方向へ
        if (Input.GetKey(KeyCode.RightArrow)) StartCoroutine(Row(BoatMoveController.Direction.RightForward));
        else if (Input.GetKey(KeyCode.LeftArrow)) StartCoroutine(Row(BoatMoveController.Direction.LeftForward));

        //スワイプによる船の操作
        SwipeCheck();

    }


    //船を漕ぐ
    bool rowing = false;
    private IEnumerator Row(BoatMoveController.Direction direction)
    {
        
        rowing = true;

        switch (direction)
        {
            case BoatMoveController.Direction.RightForward:
                playerAnimator.SetBool("RightForward", true);
                break;
            case BoatMoveController.Direction.LeftForward:
                playerAnimator.SetBool("LeftForward", true);
                break;
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(boatMoveController.Row(direction));

        yield return new WaitForSeconds(0.5f);

        playerAnimator.SetBool("RightForward", false);
        playerAnimator.SetBool("LeftForward", false);
        rowing = false;
        yield break;

    }



    //スワイプで船を漕ぐ関数発火
    private void SwipeCheck()
    {
        //unityエディタ上の操作
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TouchStartPos = Input.mousePosition;
                //Debug.Log((TouchStartPos.x-Screen.width/2)+","+(TouchStartPos.y-Screen.height/2));
            }
            /*
            else if (Input.GetMouseButton(0))
            {
                TouchEndPos = Input.mousePosition;
                if ((TouchStartPos - TouchEndPos).magnitude > SwipeDistance)//一定以上でスワイプ
                {
                    Swiped();
                }
            }
            */
            
            else if (Input.GetMouseButtonUp(0))
            {
                TouchEndPos = Input.mousePosition;
                if ((TouchStartPos - TouchEndPos).magnitude > SwipeDistance)//一定以上でスワイプ
                {
                    Swiped();
                }
            }
            
        }

        // 端末上での操作取得
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began)
                {
                    TouchStartPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    TouchEndPos = Input.mousePosition;
                    if ((TouchStartPos - TouchEndPos).magnitude > SwipeDistance)//一定以上でスワイプ
                    {
                        Swiped();
                    }
                }
                
            }
            
        }
    }


    //スワイプした時、船を漕ぐ
    private void Swiped()
    {
        
        //船の右側左側で判定
        float PosX = (TouchStartPos.x + TouchEndPos.x)/2 - Screen.width/2;
        float PosY = (TouchStartPos.y + TouchEndPos.y)/2 - Screen.height/2;
        TouchStartPos = Vector3.zero;
        TouchEndPos = Vector3.zero;

        //船の角度だけマイナス方向に回転した時のx座標が右の時は右のオールを漕ぐ
        float Angle = boatObj.transform.rotation.eulerAngles.y;
      
        float JudgePosX = PosX * Mathf.Cos(Mathf.PI*Angle/180) - PosY * Mathf.Sin(Mathf.PI*Angle/180);
        //Debug.Log(JudgePosX);

        if (JudgePosX >= 0) StartCoroutine( Row(BoatMoveController.Direction.RightForward) );//マウスダウンとマウスアップの中間で左右判定
        else StartCoroutine( Row(BoatMoveController.Direction.LeftForward) );
        

        /*
        //画面の右側左側で判定
        float JudgePosX = (TouchStartPos.x + TouchEndPos.x) / 2 - Screen.width / 2;
        if(JudgePosX>0) StartCoroutine(Row(BoatMoveController.Direction.RightForward));
        else StartCoroutine(Row(BoatMoveController.Direction.LeftForward));
        */
    }





}