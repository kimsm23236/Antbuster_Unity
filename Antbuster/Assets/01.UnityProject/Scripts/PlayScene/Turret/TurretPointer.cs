using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretPointer : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler
{
    public bool isSetReady = default;
    private PlayerController pc = default;
    public Canvas parentCanvas = default;
    public Camera camera = default;
    Vector2 mousePos = default;
    // Start is called before the first frame update
    void Start()
    {
        isSetReady = false;

        // player controller
        pc = transform.parent.gameObject.GetComponentMust<PlayerController>();
        pc.onCreateTurretState += () => 
        {
            isSetReady = true;
            mousePos = Input.mousePosition;
            mousePos = camera.ScreenToWorldPoint(mousePos);
            gameObject.GetRect().position = mousePos;
            gameObject.SetActive(true);
            //StartCoroutine(SetReady());
        };
        gameObject.SetActive(false);
        pc.onIdleState += () => 
        {
            isSetReady = false;
            gameObject.SetActive(false);
        };
        GFunc.Assert(parentCanvas != null || parentCanvas != default);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos = camera.ScreenToWorldPoint(mousePos);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(isSetReady == false)
            return;

        pc.SetPlayerState(PlayerController.PlayerState.Idle);
        gameObject.SetActive(false);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        if(!isSetReady)
            return;

        gameObject.GetRect().position = mousePos;
    }
}
