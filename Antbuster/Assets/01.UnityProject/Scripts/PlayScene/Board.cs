using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width = default;
    public int height = default;
    private GameObject tilePrefab = default;

    private Tile[,] tiles;


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

            }
            
        }
        tilePrefab.SetActive(false);
    }
}
