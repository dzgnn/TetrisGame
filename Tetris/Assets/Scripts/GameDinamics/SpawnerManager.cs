using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] ShapeManager[] allShapes;
    [SerializeField] private Image[] shapeImages = new Image[2];

    private ShapeManager[] nextShapes = new ShapeManager[2];

    
    public ShapeManager CreateShape()
    {
        ShapeManager shape = null;

        shape = TakeNextShape();
        shape.gameObject.SetActive(true);
        shape.transform.position = transform.position;

        return shape;
    }

    ShapeManager RandomCreateShape()
    {
        int randomSh = Random.Range(0,allShapes.Length);

        if (allShapes[randomSh])
        {
            return allShapes[randomSh];
        }
        else
        {
            return null;
        }
    }

    public void AllNull()
    {
        for (int i = 0; i < nextShapes.Length; i++)
        {
            nextShapes[i]=null;
        }

        FillNext();
    }

    void FillNext()
    {
        for (int i = 0; i < nextShapes.Length; i++)
        {
            if (!nextShapes[i])
            {
                nextShapes[i]=Instantiate(RandomCreateShape(),transform.position,Quaternion.identity) as ShapeManager;
                nextShapes[i].gameObject.SetActive(false);
                shapeImages[i].sprite = nextShapes[i].shapeObj;
            }
        }
    }

    ShapeManager TakeNextShape()
    {
        ShapeManager otherShape = null;

        if (nextShapes[0])
        {
            otherShape = nextShapes[0];
        }

        for (int i = 1; i < nextShapes.Length; i++)
        {
            nextShapes[i-1]=nextShapes[i];
            shapeImages[i-1].sprite = nextShapes[i-1].shapeObj;
        }

        nextShapes[nextShapes.Length -1] = null;
        FillNext();
        return otherShape;
    }

    public ShapeManager HoldShapeCreate()
    {
        ShapeManager holdshape = null;
        holdshape = Instantiate(RandomCreateShape(),transform.position,Quaternion.identity) as ShapeManager;
        holdshape.transform.position = transform.position;
        return holdshape;
    }
}
