using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // ブロック全ての格納
    [SerializeField]
    Block[] Blocks;

    //乱数でブロックを１つ選ぶ関数
    Block GetRandomBlock()
    {
        int i = Random.Range(0,Blocks.Length);

        if(Blocks[i])
        {
            return Blocks[i];
        }
        else return null;
    }


    //ブロック生成
    public Block SpawnBlock()
    {
        Block block = Instantiate(GetRandomBlock(),transform.position,Quaternion.identity);//transform.position=スポナーのポジションで生成

        if(block){
            return block;
        }
        else return null;
    }



}
