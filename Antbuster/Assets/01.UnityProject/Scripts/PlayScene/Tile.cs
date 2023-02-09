using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int x, y;
    private int value_; 

    private bool isLocked = default; 
    // 플레이어 상태가 포탑을 설치하기 위한 상태인지 확인하기 위한 변수
    private bool isReadyTurretCreate = default;
    // 이 타일이 포탑을 설치할 수 있는 칸인지 확인하기 위한 변수 * 아래 세 bool 변수에 의해 결정됨
    private bool isCantCreateTurretThisTile = default;
    // 따로 지정된 포탑 설치 불가능 지역 * 개미둥지 주변, 케익 주변 * true면 불가능한 지역
    private bool isConstanceUnable = default;
    // 주위 9칸에 개미가 있는지 * true면 개미 있음
    private bool isInNearAnt = default;
    // 주위 9칸에 포탑이 있는지 * true면 포탑 있음
    private bool isInNearTurret = default;
    private List<GameObject> CollisionObjs = default;
    
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

    private Image image = default;
    private Color currentColor = default;
    private Color defaultColor = default;
    private Color turretCreateAbleColor = default;
    private Color turretCreateUnableColor = default;
    private bool isCoroutineBreaked = false;

    public delegate void EventHandler();
    public EventHandler onReachAnt;
    public EventHandler onOutAnt;
    public EventHandler onReachTurret;
    public EventHandler onOutTurret;
    // A*
    // private 
    
    //
    void Awake()
    {
        image = GetComponent<Image>();
        CollisionObjs = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // 이미지
        turretCreateAbleColor = Color.green;
        turretCreateAbleColor.a = 128;
        turretCreateUnableColor = Color.red;
        turretCreateUnableColor.a = 128;
        defaultColor = Color.white;
        defaultColor.a = 0;
        image.color = defaultColor;
        //
        isConstanceUnable = false;
        isInNearAnt = false;
        isInNearTurret = false;
        isCantCreateTurretThisTile = true;
        isLocked = false;
        //
        onLockHandle = () => isLocked = true;
        onReleaseHandle = () => isLocked = false;
        onReachAnt += () => isInNearAnt = true;
        onOutAnt += () => isInNearAnt = false;
        onReachTurret += () => isInNearTurret = true;
        onOutTurret += () => isInNearTurret = false;
        //
        GameObject gameObjs = GFunc.GetRootObj("GameObjs");
        PlayerController pc = gameObjs.FindChildObj("Player").GetComponentMust<PlayerController>();
        pc.onCreateTurretState += () => isReadyTurretCreate = true;
        pc.onIdleState += () => isReadyTurretCreate = false; 
        //

    }
    public void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;
        // A* Test 
        /*
        if((x == 1 && y == 1) || (x == 23 && y == 18))
            return;
        int rdNum = Random.Range(1, 5+1);
        if(rdNum <= 2)
        {
            isLocked = true;
            image.color = Color.black;
        }
        */
        // A* Test
    }

    // Update is called once per frame
    void Update()
    {
        CCTCheck();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isReadyTurretCreate = !isReadyTurretCreate;
        }
    }
    private void CCTCheck()
    {
        isCantCreateTurretThisTile = isInNearAnt || isInNearTurret || isConstanceUnable;
    }
    private void ChangeColor()
    {
        if(isCantCreateTurretThisTile)
            currentColor = turretCreateUnableColor;
        else
            currentColor = turretCreateAbleColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isReadyTurretCreate == false)
            return;
        ChangeColor();
        isCoroutineBreaked = false;
        StartCoroutine(OnPointerColorChange());
        image.color = currentColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isReadyTurretCreate == false)
            return;

        image.color = defaultColor;
        isCoroutineBreaked = true;
    }
    IEnumerator OnPointerColorChange()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.3f);
            ChangeColor();
            image.color = currentColor;
            if(isCoroutineBreaked)
            {
                image.color = defaultColor;
                yield break;
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Ant" || collider.tag == "Turret")
        {
            CollisionObjs.Add(collider.gameObject);
        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        CollisionObjs.Remove(collider.gameObject);
    }
}
