using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int x, y;
    private int value_; 

    private bool isLocked = default; 
    public bool isOpen
    {
        get
        {
            return !isLocked;
        }
    }

    public delegate void OnLockingHandle();
    public OnLockingHandle onLockHandle;
    public OnLockingHandle onReleaseHandle;

    // A*
    // private 
    
    //

    // Start is called before the first frame update
    void Start()
    {
        isLocked = false;
        onLockHandle = () => isLocked = true;
        onReleaseHandle = () => isLocked = false;
    }
    public void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;
        // A* Test 
        if((x == 1 && y == 1) || (x == 23 && y == 18))
            return;
        int rdNum = Random.Range(1, 5+1);
        if(rdNum <= 2)
        {
            isLocked = true;
            Image image = GetComponent<Image>();
            image.color = Color.black;
        }
        // A* Test
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
