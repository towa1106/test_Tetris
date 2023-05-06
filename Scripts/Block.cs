using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //回転していいブロックか
    [SerializeField]
    private bool canRotate = true;

    //移動
    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }
    //移動関数を呼ぶ関数4種
    public void MoveLeft()
    {
        Move(new Vector3(-1,0,0));
    }
    public void MoveRight()
    {
        Move(new Vector3(1,0,0));
    }
    public void MoveUp()
    {
        Move(new Vector3(0,1,0));
    }
    public void MoveDown()
    {
        Move(new Vector3(0,-1,0));
    }

    //回転用2種
    public void RotateRight()
    {
        if(canRotate)
        {
            transform.Rotate(0,0,-90);
        }
    }
    public void RotateLeft()
    {
        if(canRotate)
        {
            transform.Rotate(0,0,90);
        }
    }


}
