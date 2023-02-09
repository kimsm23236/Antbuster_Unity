using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private PlayerState currentPlayerState = default;
    private PlayerState PrevState = default;

    

    private GameObject BaseTurretPrefab = default;
    private GameObject BaseTurrentPSPrefab = default;
    public delegate void ChangeStateEventHandler(PlayerState value);
    public ChangeStateEventHandler SetPlayerState = default;
    public delegate void EventHandler();
    public EventHandler onCreateTurretState = default;
    public EventHandler onIdleState = default;

    // raycast
    public Camera camera = default;
    public RaycastHit2D[] hits;
    private Vector3 MousePos = default;
    //
    public enum PlayerState
    {
        NONE = -1,
        Idle,           // 기본 상태
        TurretCreate,   // 터렛 생성 버튼을 눌러 생성 준비 상태 
        SelectTurret,   // 터렛을 클릭하였을 때 터렛의 정보 출력 및 업그레이드버튼 출력을 위한 상태
        SelectAnt,      // 개미를 클릭하였을 때 개미의 정보 출력과 터렛의 타겟팅을 위한 상태
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerState = PlayerState.NONE;
        SetPlayerState = new ChangeStateEventHandler(ChangeState);
        SetPlayerState(PlayerState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickCreateTurretBtn()
    {
        SetPlayerState(PlayerState.TurretCreate);
    }
    private void ChangeState(PlayerState newState)
    {
        PrevState = currentPlayerState;
        currentPlayerState = newState;
        switch(currentPlayerState)
        {
            case PlayerState.Idle:
            onIdleState();
            break;
            case PlayerState.TurretCreate:
            onCreateTurretState();
            break;
            default:
            break;
        }
    }
    void rethrowRaycast(PointerEventData eventData, GameObject excludeGameObject)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

        pointerEventData.position = eventData.position;
        // pointerEventData.position = eventData.pressPosition;
        List<RaycastResult> raycastResult = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerEventData, raycastResult);

        for(int i = 0; i < raycastResult.Count; i++)
        {
            if(excludeGameObject != null && raycastResult[i].gameObject != excludeGameObject)
            {
                //sim
            }
        }

    }
    void simulateCallbackFunction(GameObject target)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

        RaycastResult res = new RaycastResult();
        //res.gameObject
    }
}
