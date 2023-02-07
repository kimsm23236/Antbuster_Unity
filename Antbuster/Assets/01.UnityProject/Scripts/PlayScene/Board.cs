using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width = default;
    public int height = default;
    private GameObject tilePrefab = default;

    private Tile[,] tiles;

    // 우선 네방향
    private int[] dy = new int[] { -1, 0, 1, 0,  -1, 1, 1, -1 };
    private int[] dx = new int[] { 0, -1, 0, 1, -1, -1, 1, 1 };
    private int[] cost = new int[] { 10, 10, 10, 10, 14, 14, 14, 14};


    // Start is called before the first frame update
    void Start()
    {
        tilePrefab = gameObject.FindChildObj("Tile");
        tiles = new Tile[width, height];        
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        for(int i = 0 ; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                var tileObject = Instantiate(tilePrefab, transform);
                var tile = tileObject.GetComponentMust<Tile>();
                tile.Initialize(i, j);
                tile.transform.localPosition = new Vector2(i * tile.GetComponent<RectTransform>().sizeDelta.x, j * tile.GetComponent<RectTransform>().sizeDelta.y * -1);
                tiles[i, j] = tile;
            }
            
        }
        tilePrefab.SetActive(false);
    }

    public List<Path> GetPath(Pos startPos, Pos destPos)
    {
        List<Path> resultPath = new List<Path>();

        Path startPath = new Path(startPos, default);
        Path EndPath = new Path(destPos, default);

        bool[,] closed = new bool[height, width];
        int[,] open = new int[height, width];
        PriorityQueue<Path> pq = new PriorityQueue<Path>();
        Pos[,] parent = new Pos[height, width];
        for(int i = 0 ; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                open[i,j] = int.MaxValue;
            }
        }
        open[startPos.y, startPos.x] = 10 * (Mathf.Abs(destPos.y - startPos.y) + Mathf.Abs(destPos.x - startPos.x));
        Path sNode = new Path(new Pos(startPos.x, startPos.y));
        sNode.G = 0;
        sNode.F = 10 * (Mathf.Abs(destPos.y - startPos.y) + Mathf.Abs(destPos.x - startPos.x));
        pq.Enqueue(sNode, -sNode.F);
        parent[startPos.y, startPos.x] = new Pos(startPos.y, startPos.x);

        int loopcount = 0;

        while(pq.Count > 0)
        {
            if(loopcount++ >= 10000)
            {
                throw new System.Exception("Infinite Loop");
            }

            Path curPath = pq.Dequeue();
            // GFunc.LogWarning($"curPath x : {curPath.curPos.x}, y : {curPath.curPos.y}");

            if(closed[curPath.curPos.y, curPath.curPos.x])
                continue;

            closed[curPath.curPos.y, curPath.curPos.x] = true;

            if(curPath.curPos.x == EndPath.curPos.x && curPath.curPos.y == EndPath.curPos.y)
            {
                break;
            }


            for(int i = 0; i < 8; i++)
            {
                int nextX = curPath.curPos.x + dx[i];
                int nextY = curPath.curPos.y + dy[i];

                // 범위 밖 검사
                if(nextX < 0 || nextX >= width || nextY < 0 || nextY >= height)
                    continue;
                // 닫힌 경로 배열에 있는지를 검사
                if(closed[nextY, nextX])
                    continue;
                // 타일이 잠겨있는지 검사 후 열려있다면, * 아무것도 없다면 오픈 리스트에 추가
                if(!tiles[nextX, nextY].isOpen)
                {
                    continue;
                }

                int g = curPath.G + cost[i];
                int h = 10 * (Mathf.Abs(destPos.y - startPos.y) + Mathf.Abs(destPos.x - startPos.x));

                if(open[nextY, nextX] < g + h)
                    continue;

                open[nextY, nextX] = g + h;
                Path nextPath = new Path(new Pos(nextX, nextY));
                nextPath.G = g;
                nextPath.F = g + h;
                pq.Enqueue(nextPath, -(g+h));
                parent[nextY, nextX] = new Pos(curPath.curPos.x, curPath.curPos.y);
            }
        }

        Path calPath = new Path(new Pos(destPos.x, destPos.y));
        Pos prPos = new Pos(parent[calPath.curPos.y, calPath.curPos.x].x, parent[calPath.curPos.y, calPath.curPos.x].y);
        while(startPos.x != calPath.curPos.x || startPos.y != calPath.curPos.y)
        {
            resultPath.Add(new Path(new Pos(calPath.curPos.x, calPath.curPos.y)));
            calPath.curPos = parent[calPath.curPos.y, calPath.curPos.x];
        }
        resultPath.Reverse();
        foreach(Path p in resultPath)
        {
            GFunc.LogWarning($"Setting Path : x : {p.curPos.x}, y : {p.curPos.y}");
        }
        return resultPath;
    }
    // Legacy
    public List<Path> GetOpenList(Path curPath, List<Path> closeList, PriorityQueue<Path> openList)
    {
        List<Path> childLists = new List<Path>();

        int x = curPath.curPos.x;
        int y = curPath.curPos.y;

        for(int i = 0; i < 8; i++)
        {
            int nextX = x + dx[i];
            int nextY = y + dy[i];
            Path nextPath = new Path(new Pos(nextX, nextY), default);

            // 범위 밖 검사
            if(nextX < 0 || nextX >= width || nextY < 0 || nextY >= height)
            {
                continue;
            }

            // 닫힌 경로 배열에 있는지를 검사
            if(closeList.Contains(nextPath))
                continue;

            // 타일이 닫혀있는지 검사 후 열려있다면, * 아무것도 없다면 오픈 리스트에 추가
            if(!tiles[nextX, nextY].isOpen)
            {
                continue;
            }

            childLists.Add(nextPath);
        }
        return childLists;
    }
}
