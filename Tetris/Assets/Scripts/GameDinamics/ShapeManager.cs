using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    [SerializeField] bool canTurn=true;

    public Sprite shapeObj;

    void Move(Vector3 dir)
    {
        transform.Translate(dir,Space.World);
    }
    public void MoveLeft()
    {
        Move(Vector3.left);
    }

   public void MoveRight()
    {
        Move(Vector3.right);
    }

    public void MoveUp()
    {
        Move(Vector3.up);
    }

    public void MoveDown()
    {
        transform.Translate(Vector3.down,Space.World);
    }

    public void TurnRight()
    {
        if (canTurn)
        {
           transform.Rotate(0,0,-90);
        }
        
    }

    public void TurnLeft()
    {
        if (canTurn)
        {
           transform.Rotate(0,0,90);
        }
        
    }


    IEnumerator MoveRoutine()
    {
        while (true)
        {
            MoveDown();
            yield return new WaitForSeconds(0.3f);  
        }
        
    }

    public void IsClockDirection(bool isdir)
    {
        if (isdir)
        {
            TurnRight();
        }
        else
        {
            TurnLeft();
        }
    }
}
