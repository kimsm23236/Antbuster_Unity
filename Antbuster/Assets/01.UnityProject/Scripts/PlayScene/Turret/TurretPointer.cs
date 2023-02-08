using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretPointer : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler
{
    public bool isSetReady = default;
    // Start is called before the first frame update
    void Start()
    {
        isSetReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnPointerMove(PointerEventData eventData)
    {
        if(!isSetReady)
            return;
        gameObject.AddAnchoredPos(eventData.delta);
    }
}
