using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public Pos position;
    public Path parent;

    public int F = 0;
    public int G = 0;
    public int H = 0;
    public Path(Pos position, Path parent)
    {
        this.position = position;
        this.parent = parent;
    }
    public Path(Pos position)
    {
        this.position = position;
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
    private Pos destPos = default;
    private Board board = default;
    private GameObject target = default;
    public float speed = default;
    public float rotateSpeed = default;

    private List<Path> currentPath = default;
    private int pathIdx = default;
    private GameObject[,] moveTargetArr = default;
    
    private AntState state = default;

    // Start is called before the first frame update
    void Start()
    {
        currentPath = new List<Path>();
        currentPos = new Pos(debugStartX, debugStartY);
        destPos = new Pos(debugDestX, debugDestY);
        GameObject gObjs = GFunc.GetRootObj("GameObjs");
        board = gObjs.GetComponentMust<Board>("Board");
        moveTargetArr = new GameObject[board.height, board.width];
        for(int i = 0; i < board.height; i++)
        {
            for(int j = 0; j < board.width; j++)
            {
                moveTargetArr[i,j] = board.Tiles[j, i].gameObject.FindChildObj("MoveTarget");
            }
        }
        // speed = 1f;
        // rotateSpeed = 1f;
        pathIdx = 0;
        PathFind();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        RotateToTarget();
    }
    void Move()
    {
        if(target == null || target == default)
            return;
        RectTransform rect = gameObject.GetRect();
        // transform.LookAt(target.transform);
        rect.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
        //transform.LookAt(target.transform);
        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
        ChangeTarget();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Tile")
        {
            currentPos = nextPos;
            // 다음 경로 설정해서 reserveNextPos에 넣어주는 함수 만들어서 넣기
        }
    }

    void PathFind()
    {
        currentPath = board.GetPath(currentPos, destPos);
        // 경로 지정 후 타겟 설정
        target = moveTargetArr[currentPath[pathIdx].position.y, currentPath[pathIdx].position.x];        
    }
    void ChangeTarget()
    {
        float dist = Mathf.Abs(target.transform.position.x - transform.position.x) +
                        Mathf.Abs(target.transform.position.y - transform.position.y);
        if(dist < 0.04f)
        {
            pathIdx = Mathf.Clamp(pathIdx + 1, 0, currentPath.Count - 1);
            target = moveTargetArr[currentPath[pathIdx].position.y, currentPath[pathIdx].position.x];
        }
    }
    void RotateToTarget()
    {
        if(target == null || target == default)
            return;
        Vector2 direction = new Vector2(transform.position.x - target.transform.position.x, transform.position.y - target.transform.position.y);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }
}
