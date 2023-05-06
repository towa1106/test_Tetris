using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //二次元配列の作成
    private Transform[,] grid;
    
    
    //ボード基盤用の格納用作成
    [SerializeField]
    private Transform emptySprite;

    [SerializeField]//Boardの高さ、幅、調整用Header
    private int height = 30,width = 10, header = 8;

    private void Awake()
    {
        grid = new Transform[width,height];
    }

    private void Start(){
        CreateBoard();
    }

    void CreateBoard()
    {
        if(emptySprite)
        {
            for(int y = 0; y < height - header; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    Transform clone = Instantiate(emptySprite,
                        new Vector3(x,y,0),Quaternion.identity);

                    clone.transform.parent = transform;

                }
            }
        }
    }

    //ブロックが枠内にあるか判定する関数を呼ぶ関数
    public bool CheckPosition(Block block)
    {
        //子ブロックの数分ループを回す(テトリミノの構成ブロック数=4)
        foreach(Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            if(!BoardOutCheck((int)pos.x,(int)pos.y))
            {
                return false;
            }

            //移動先にブロックがあるか判定する
            if(BlockCheck((int)pos.x,(int)pos.y,block))
            {
                return false;
            }


        }
        return true;
    }

    //枠内にいるのか判定する関数
    bool BoardOutCheck(int x, int y)
    {
       return (x >= 0 && x < width && y >= 0);
    }

    bool BlockCheck(int x, int y,Block block)
    {
        //二次元配列が空ではないのは他のブロックがある時
        //親が違うのは他のブロックがある時
        return(grid[x,y] != null && grid[x,y].parent != block.transform);
    }

    //ブロックが落ちた場所を記録する関数
    public void SaveBlockInGrid(Block block)
    {
        foreach(Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            grid[(int)pos.x, (int)pos.y] = item;
        }
    }

    //全部の行チェックし、埋まってれば削除
    public void ClearAllRows()
    {
        for(int y = 0; y < height; y++){
            if(IsComplete(y))
            {
                //削除
                ClearRow(y);

                //行下げる
                ShiftRowsDown(y + 1);

                y--;
            }
        }
    }

    //全ての行チェック
    bool IsComplete(int y)
    {
        for(int x = 0; x < width; x++)
        {
            if(grid[x,y] == null) return false;
        }
        return true;
    }

    //削除する関数
    void ClearRow(int y)
    {
         for(int x = 0; x < width; x++)
        {
            if(grid[x,y] != null)
            {
                Destroy(grid[x,y].gameObject);
            }
            grid[x,y] = null;
        }
    }
    //上にあるブロックを１団下げる関数
    void ShiftRowsDown(int startY)
    {
        for(int y = startY; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if(grid[x,y] != null)
                {
                    grid[x,y-1] = grid[x,y];
                    grid[x,y] = null;
                    grid[x,y-1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    public bool OverLimit(Block block)
    {
        foreach(Transform item in block.transform)
        {
            if(item.transform.position.y >= height - header)
            {
                return true;
            }
        }
        return false;
    }





}
