using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
