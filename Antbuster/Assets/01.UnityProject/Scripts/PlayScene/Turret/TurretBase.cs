using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  TurretStatus
{
    public int price = default;
    public float frequency = default;
    public float speed = default;
    public float range = default;
    public int basicDamage = default;

    public TurretStatus(int p, float fr, float sp, float ra, int dmg)
    {
        price = p;
        frequency = fr;
        speed = sp;
        range = ra;
        basicDamage = dmg;
    }
    /*
    기본 표기 스테이터스
    Price - 생성 가격 및 업그레이드 가격
    Frequency - 총알 발사 간격? 3shot / sec
    speed - 총알 속도
    Range - 공격 범위
    Basic Damage - 총알 하나당 데미지
    */
}
public class TurretBase : MonoBehaviour
{
    // Status
    private TurretStatus status;
    public TurretStatus Stat
    {
        get
        {
            return status;
        }
    }
    public int defaultPrice = default;
    public float defaultFrequency = default;
    public float defaultSpeed = default;
    public float defaultRange = default;
    public int defaultDamage = default;
    //
    private GameObject Barrel = default;
    private GameObject curTarget = default;
    public GameObject Target
    {
        get
        {
            return curTarget;
        }
    }
    private GameObject prevTarget = default;
    private List<GameObject> targetList = default;
    private CircleCollider2D circleCollider = default;
    private bool isSelected = default;
    private GameObject bulletPrefab = default;
    public GameObject bullet
    {
        get
        {
            return bulletPrefab;
        }
    }

    public delegate void EventHandler();
    public EventHandler onChangeTarget;

    // 거리체크용 게임오브젝트
    public GameObject distchecking = default;
    public float Dist = default;
    //


    void Awake()
    {
        status = new TurretStatus(defaultPrice, defaultFrequency, defaultSpeed, defaultRange, defaultDamage);
        circleCollider = gameObject.GetComponentMust<CircleCollider2D>();
        Barrel = gameObject.FindChildObj("Barrel");
        bulletPrefab = gameObject.FindChildObj("BaseBullet");
        bulletPrefab.SetActive(false);
        targetList = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetTarget());
        // Vector3.Magnitude()
        Dist = Vector3.Distance(transform.position, distchecking.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SetTarget()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.3f);
            ChangeTarget();
        }
    }
    void ChangeTarget()
    {
        float max = float.MaxValue;
        GameObject nextTarget = default;
        foreach(GameObject tg in targetList)
        {
            Vector3 offset = tg.transform.position - transform.position;
            float dValue = offset.sqrMagnitude;
            if(max > dValue)
            {
                max = dValue;
                nextTarget = tg;
            }
        }
        prevTarget = curTarget;
        curTarget = nextTarget;
        if(curTarget != prevTarget)
        {
            onChangeTarget();
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        GFunc.LogWarning("뭔가 겹침");
        if(coll.tag == "Ant")
        {
            GFunc.LogWarning("개미가 겹침");
            targetList.Add(coll.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.tag == "Ant")
        {
            targetList.Remove(coll.gameObject);
        }
    }
}
