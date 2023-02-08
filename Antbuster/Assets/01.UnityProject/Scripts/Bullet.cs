using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int basicDamage = default;
    public float moveDistance = default;
    private Vector3 prevPos = default;

    private Rigidbody2D bulletRigid = default;
    // Start is called before the first frame update
    void Awake()
    {
        bulletRigid = gameObject.GetComponentMust<Rigidbody2D>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseDist();
        GFunc.LogWarning($"moveDistance : {moveDistance}");
    }
    public void SetVelo(Vector2 newVelo)
    {
        moveDistance = 0f;
        prevPos = transform.position;
        bulletRigid.velocity = newVelo;
    }
    void IncreaseDist()
    {
        float frameMove = Vector3.Distance(transform.position, prevPos);
        prevPos = transform.position;
        // Vector3 frameMove =(transform.position - prevPos);
        // prevPos = transform.position;
        moveDistance += frameMove;
    }
}
