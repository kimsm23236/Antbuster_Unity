using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public Pos curPos;
    public Path parent;

    public int F = 0;
    public int G = 0;
    public int H = 0;
    public Path(Pos position, Path parent)
    {
        this.curPos = position;
        this.parent = parent;
    }
    public Path(Pos position)
    {
        this.curPos = position;
        this.parent = default;

    }

    // public static bool operator ==(Path p1, Path p2)
    // {
    //     return (p1.curPos.x == p2.curPos.x) && (p1.curPos.y == p2.curPos.y);
    // }
    // public static bool operator !=(Path p1, Path p2)
    // {
    //     return (p1.curPos.x != p2.curPos.x) || (p1.curPos.y != p2.curPos.y);
    // }
    // public override bool Equals(object obj)
    // {
    //     Path p1 = obj as Path;
    //     return (curPos.x == p1.curPos.x) && (curPos.y == p1.curPos.y);
    // }
}
public class Pos
{
    public int x;
    public int y;
    public Pos(int xValue, int yValue)
    {
        x = xValue;
        y = yValue;
    }

    public void SetPos(int newX, int newY)
    {
        this.x = newX;
        this.y = newY;
    }
    public void SetPos(Pos newPos)
    {
        this.x = newPos.x;
        this.y = newPos.y;
    }
}
public enum AntState
{
    NONE = -1,
    TraceCake, TraceNest
};

public class AntPathfinder : MonoBehaviour
{
    // debuging
    public int debugStartX = default;
    public int debugStartY = default;
    public int debugDestX = default;
    public int debugDestY = default;
    // debuging
    public Pos currentPos = default;
    public Pos nextPos = default;
    private Pos reserveNextPos = default;
    private Pos destPos = default;
    private Board board = default;

    private List<Path> currentPath = default;
    
    private AntState state = default;

    // Start is called before the first frame update
    void Start()
    {
        currentPath = new List<Path>();
        currentPos = new Pos(debugStartX, debugStartY);
        destPos = new Pos(debugDestX, debugDestY);
        GameObject gObjs = GFunc.GetRootObj("GameObjs");
        board = gObjs.GetComponentMust<Board>("Board");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            PathFind();
        }
    }

    void SetReserveNextPos()
    {

    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Tile")
        {
            currentPos = nextPos;
            nextPos = reserveNextPos;
            // 다음 경로 설정해서 reserveNextPos에 넣어주는 함수 만들어서 넣기
        }
    }

    void PathFind()
    {
        currentPath = board.GetPath(currentPos, destPos);
        /*
        Path startPath = new Path(currentPos, default);
        Path EndPath = new Path(destPos, default);

        PriorityQueue<Path> openList = new PriorityQueue<Path>();
        List<Path> closeList = new List<Path>();

        openList.Enqueue(startPath, 0);

        int loopcount = 0;

        while(openList.Count > 0)
        {
            if(loopcount++ >= 10000)
            {
                throw new System.Exception("Infinite Loop");
            }
            Path curPath = openList.Dequeue();
            GFunc.LogWarning($"curPath x : {curPath.curPos.x}, y : {curPath.curPos.y}");
            closeList.Add(curPath);

            if(curPath.curPos.x == EndPath.curPos.x && curPath.curPos.y == EndPath.curPos.y)
            {

                List<Path> paths = new List<Path>();
                Path current = curPath;
                while(current != default)
                {
                    paths.Add(current);
                    current = current.parent;
                }
                paths.Reverse();
                currentPath = paths;
                // 디버깅용
                for(int i = 0 ; i < currentPath.Count; i++)
                {
                    GFunc.LogWarning($"Path : ({currentPath[i].curPos.x}, {currentPath[i].curPos.y})");
                }

                //
                break;
            }

            List<Path> childList = board.GetOpenList(curPath, closeList);
            foreach(Path child in childList)
            {
                child.parent = curPath;
                child.G = curPath.G + 1;
                child.H = (int)Mathf.Pow((float)(child.curPos.x - EndPath.curPos.x), 2f);
                child.F = child.G + child.H;
                // 자식이 오픈리스트에 존재할경우
                // g값 비교하여 갱신
                bool isNotUpdate = false;
                for(int i = 0 ; i < openList.Count; i++)
                {
                    Path openNode = openList[i] as Path;
                    if((child.curPos.x == openNode.curPos.x) && (child.curPos.y == openNode.curPos.y) && child.G > openNode.G)
                        isNotUpdate = true;
                }
                if(isNotUpdate)
                    continue;
                
                openList.Enqueue(child, -child.F);
            }
        }
        */
    }
}
