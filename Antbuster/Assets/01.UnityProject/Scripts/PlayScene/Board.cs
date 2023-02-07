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
    private int[] dx = {-1, 0, 1, 1, 1, 0, -1, -1};
    private int[] dy = {-1, -1, -1, 0, 1, 1, 1, 0};


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

    public List<Path> GetOpenList(Path curPath, List<Path> closeList)
    {
        List<Path> OpenLists = new List<Path>();

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
            if(tiles[nextX, nextY].isOpen)
            {
                OpenLists.Add(new Path(new Pos(nextX, nextY), default));
            }
        }
        return OpenLists;
    }
}
