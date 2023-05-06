using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //　スポナーオブジェクトをスポナー変数に格納する

    Spawner spawner;
    Block activeBlock;//生成されたブロック関数

    //ブロックを落とすまでのインターバル時間
    //次にブロックが落ちるまでの時間

    [SerializeField]
    private float dropInterval = 0.25f;
    float nextdropTimer;

    //ボードスクリプトを格納
    Board board;

    //入力受付タイマー３種類
    float nextKeyDownTimer, nextKeyLeftRightTimer,nextKeyRotateTimer;

    //入力インターバル３種類
    [SerializeField]
    private float nextKeyDownInterval,nextKeyLeftRightInterval,nextKeyRotateInterval;
    
    //パネルの格納
    [SerializeField]
    private GameObject gameOverPanel;

    //ゲームオーバー判定
    bool gameOver;

    private void Start()
    {
        //Findは重いので、乱用しない　今回はStartで使うだけ
        spawner = GameObject.FindObjectOfType<Spawner>();

        //ボードを変数に格納する
        board = GameObject.FindObjectOfType<Board>();

        //ブロックとステージのズレを直す
        spawner.transform.position = Rounding.Round(spawner.transform.position);
        
        //タイマー初期設定
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

        //　スポナークラスからブロック生成関数を呼んで、変数に格納する
        if(!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }

        //ゲームオーバーパネルの非表示設定
        if(gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }

    }

    private void Update()
    {

        if(gameOver)
        {
            return;
        }
        PlayerInput();

        /*
        //時間判定をして、判定次第で落下関数を呼ぶ
        if(Time.time > nextdropTimer)
        {
            nextdropTimer = Time.time + dropInterval;

            if(activeBlock)
        {
            activeBlock.MoveDown();//移動

            //ボードクラスの関数を呼んでボードから出ていないか確認
            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveUp();

                board.SaveBlockInGrid(activeBlock);

                activeBlock = spawner.SpawnBlock();
            }
        }

        }
        */

    }

    //キー入力を検知してブロックを動かす関数
    void PlayerInput()
    {
        if(Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTimer) || Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight();
            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        else if(Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTimer) || Input.GetKeyDown(KeyCode.A))
        {
            activeBlock.MoveLeft();
            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        else if(Input.GetKey(KeyCode.E) && (Time.time > nextKeyRotateTimer))
        {
            activeBlock.RotateRight();
            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            if(!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }
         else if(Input.GetKey(KeyCode.S) && (Time.time > nextKeyDownTimer) || (Time.time > nextdropTimer))
        {
            activeBlock.MoveDown();
            nextKeyDownTimer = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;

            if(!board.CheckPosition(activeBlock))
            {
                if(board.OverLimit(activeBlock))
                {
                    GameOver();
                }
                else
                {
                //底についた時の処理
                BottomBoard();
                }
               
            }
        }
    }

    void BottomBoard()
    {
        activeBlock.MoveUp();
        board.SaveBlockInGrid(activeBlock);

        activeBlock = spawner.SpawnBlock();

        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;

        board.ClearAllRows();//埋まってれば削除

    }

    //ゲームオーバーの時パネル表示
    void GameOver()
    {
        activeBlock.MoveUp();

        //パネル非表示設定
        if(!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }
        gameOver = true;
    }

    //シーン再読み込み
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
