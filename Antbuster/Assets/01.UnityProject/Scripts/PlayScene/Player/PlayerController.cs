using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerState currentPlayerState = default;

    private GameObject BaseTurretPrefab = default;
    private GameObject BaseTurrentPSPrefab = default;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
