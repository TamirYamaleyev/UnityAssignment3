using System;
using System.Collections;
using UnityEngine;

public class JumpAction : MonoBehaviour
{
    public Transform ShadowTransform;
    public Transform groundRaycastStart;
    public LayerMask groundLayer;
    public float RaycastDistance = 0.001f;
    public float JumpForce = 70.0f;
    
    private Rigidbody2D rb;
    private Vector3 shadowLocalpos;
    private float gravityScale;
    private float horizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        // save relative position of the shadow
        shadowLocalpos = ShadowTransform.localPosition;
        gravityScale = rb.gravityScale;
    }
    
    /*
    IEnumerator StopPhysics()
    {
        yield return new WaitForSeconds(JumpTime);
        
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    public void Jump(float horizontal)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 force = new Vector2(horizontal, JumpForce);
            
        rb.AddForce(force);
            
        StartCoroutine(StopPhysics());
    }*/


    void LinkShadow()
    {
        ShadowTransform.SetParent(transform);
        ShadowTransform.localPosition = shadowLocalpos;
    }
    
    void UnlinkShadow()
    {
        ShadowTransform.SetParent(null);
    }

    public void JumpStart(float horizontal)
    {
        this.horizontal = horizontal;
        ChangePhysics(true);
            
        UnlinkShadow();
    }

    void JumpEnd()
    {
        ChangePhysics(false);

        LinkShadow();
        
        GetComponent<Player>().JumpEnd();
    }

    void ShadowFollowJumpingPlayer()
    {
        Vector3 pos = ShadowTransform.position;
        pos.x = transform.position.x; // Follow player's x position
        ShadowTransform.position = pos;
    }

    bool ShouldEndJump()
    {
        ShadowFollowJumpingPlayer();
        
        RaycastHit2D hit = Physics2D.Raycast(
            /*transform.position*/ groundRaycastStart.position,
            Vector2.down,
            RaycastDistance, 
            groundLayer
            );
        
        return hit && hit.collider;
    }

    void ChangePhysics(bool bUsePhysics)
    {
        if (bUsePhysics)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = gravityScale;
            
            Vector2 direction = new Vector2(horizontal, 1);
            rb.AddForce(direction * JumpForce);
        }
        else
        {
            // zero velocity
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void LateUpdate()
    {
        if (ShouldEndJump())
            JumpEnd();
    }
}
