using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowShapeManager : MonoBehaviour
{
    private ShapeManager followShape = null;
    private bool isOnGround = false;

    public Color color = new Color(1f,1f,1f,.2f);

    public void CreateFollowShape(ShapeManager realShape, BoardManager board)
    {
        if (!followShape)
        {
            followShape = Instantiate(realShape,realShape.transform.position, realShape.transform.rotation) as ShapeManager;
            followShape.name = "FollowSahpe";

            SpriteRenderer[] allSprite = followShape.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer sr in allSprite)
            {
                sr.color = color;
            }
        }
        else
        {
            followShape.transform.position = realShape.transform.position;
            followShape.transform.rotation = realShape.transform.rotation;

        }

        isOnGround = false;


        while (!isOnGround)
        {
            followShape.MoveDown();

            if (!board.RightPosition(followShape))
            {
                followShape.MoveUp();
                isOnGround = true;
            }
        }
    }


    public void ResetFnc()
    {
        Destroy(followShape.gameObject);
    }
}
