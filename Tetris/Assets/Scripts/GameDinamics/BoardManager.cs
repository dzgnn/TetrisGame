using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;
    public int height = 22;
    public int width = 10;

    public int completedLine = 0;

    private Transform[,] grid;

    public ParticleManager[] lineEffects = new ParticleManager[4];
     

    void Awake()
    {
        grid = new Transform[width,height];
    }
    void Start()
    {
        BosOlustur();
    }

    bool InBoard(int x, int y)
    {
        return (x>=0 && x<width && y>=0); 
    }

    bool IsSquareFull(int x,int y,ShapeManager shape)
    {
        return(grid[x,y] != null && grid[x,y].parent != shape.transform);
    }

    public bool RightPosition(ShapeManager shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectorToInt(child.position);
            
            if (!InBoard((int)pos.x,(int)pos.y))
            {
                return false;
            }

            if (pos.y < height)
            {
                if (IsSquareFull((int)pos.x,(int)pos.y,shape))
                {
                    return false;
                }
            }
            
        }

        return true;

        
    }

    void BosOlustur()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Transform tile = Instantiate(tilePrefab,new Vector3(x,y,0),Quaternion.identity);
                tile.name ="x   "+x.ToString()+","+"y "+y.ToString();
                tile.parent = this.transform;
            }
        }
    }

    public void ShapeToGrid(ShapeManager shape)
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectorToInt(child.position);
            grid[(int)pos.x,(int)pos.y] = child;    
        }
    }

    public bool IsLineComplete(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x,y]==null)
            {
                return false;
            }

        }
        return true;
    }

    void CleanLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x,y]!=null)
            {
                Destroy(grid[x,y].gameObject);
            }

            grid[x,y] = null;
        }
    }


    void DownOneLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x,y]!=null)
            {
                grid[x,y-1] = grid[x,y];
                grid[x,y] = null;
                grid[x,y-1].position += Vector3.down;
            }
        }
    }

    void DownAllLine(int starty)
    {
        for (int i = starty; i < height; ++i)
        {
            DownOneLine(i);
        }
    }

    public IEnumerator CleanAllLine()
    {
        completedLine = 0;

        for (int y = 0; y < height; y++)
        {
            if (IsLineComplete(y))
            {
                lineEffectCreate(completedLine,y);
                completedLine++;
            }
        }

        yield return new WaitForSeconds(0.2f);

        for (int y = 0; y < height; y++)
        {
            if (IsLineComplete(y))
            {
                CleanLine(y);
                DownAllLine(y+1);
                yield return new WaitForSeconds(0.2f);
                y--;
            }
        }
    }

    public bool IsGameOver(ShapeManager shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= height -1 )
            {
                return true;
            }
            
        }
        return false;
    }

    Vector2 VectorToInt(Vector2 vector2)
    {
        return new Vector2(Mathf.Round(vector2.x),Mathf.Round(vector2.y));
    }


    void lineEffectCreate(int whichline, int y)
    {
        //    if (lineEffects)
        //    {
        //         lineEffects.transform.position = new Vector3(0,y,0);
        //         lineEffects.EffectPlay();
        //    } 
        if (lineEffects[whichline])
        {
            lineEffects[whichline].transform.position = new Vector3(0,y,0);
            lineEffects[whichline].EffectPlay();
        }
    }
}
