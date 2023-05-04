using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _characterCollider;
    private LayerMask _groundLayerMask;
    private Animator _animator;

    private const float MoveSpeed = 7.0f;
    private const float JumpForce = 14f;
    private bool _isDoubleJumpEnable;

    private static readonly Dictionary<int, string> States = new() {
        {1, "Player_Idle"},
        {2, "Player_Running"},
        {3, "Player_Jump"},
        {4, "Player_Falling"}
    };

    private bool _isGrounded;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _characterCollider = GetComponent<BoxCollider2D>();
        _groundLayerMask = LayerMask.GetMask("Ground");
        Debug.Log(LayerMask.NameToLayer("Terrain"));
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isDoubleJumpEnable = false;

    }

    // Update is called once per frame
    private void Update()
    {

        if (_isGrounded)
        {
            _isDoubleJumpEnable = false;
        }
        var dirX = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(dirX * MoveSpeed, _rb.velocity.y);
        if ((_isGrounded || _isDoubleJumpEnable) && Input.GetButtonDown("Jump"))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 
                _isDoubleJumpEnable ? JumpForce*.5f : JumpForce);
            
            _isDoubleJumpEnable = !_isDoubleJumpEnable;
        }
        UpdateAnimation(dirX);
    }

    private void UpdateAnimation(float dirX)
    {
        string movementState;
        if (dirX > 0)
        {
            movementState = States[2];
            _spriteRenderer.flipX = false;
        }
        else if (dirX < 0)
        {
            movementState = States[2];
            _spriteRenderer.flipX = true;
        }
        else
        {
            movementState = States[1];
        }

        if (_rb.velocity.y > 0.1f) {
            movementState = States[3];
        }
        else if (_rb.velocity.y < -0.1f)
        {
            movementState = States[4];
        }

        _animator.Play(movementState);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("Collided with " + collision.gameObject.name);
    //}

    private void FixedUpdate()
    {
        
        _isGrounded = IsHabilToJump();
    }

    private bool IsHabilToJump()
    {
       return Physics2D.BoxCast(_characterCollider.bounds.center, 
            _characterCollider.bounds.size, 
            0f, 
            Vector2.down, 
            0.1f, _groundLayerMask);
    }
}
