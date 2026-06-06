using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    Rigidbody2D _rigidbody;
    Animator _animator;
    BoxCollider2D _boxCollider;
    CapsuleCollider2D _capsuleCollider;

    Vector2 _directionalInput;

    float gravityScaleAtStart = 6f;
    float gravityScaleOnClimb = 0f;

    bool isAlive;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();

        _rigidbody.gravityScale = gravityScaleAtStart;
        isAlive = true;
    }

    void FixedUpdate()
    {
        if (!isAlive) { return; }

        Run();
        FlipDirection();
        Climb();
        Die();
    }    

    void OnMove(InputValue value)
    {
        _directionalInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!_boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpForce);
        }
    }
    void Run()
    {
        Vector2 _playerVelocity = new Vector2(_directionalInput.x * moveSpeed, _rigidbody.linearVelocity.y);
        _rigidbody.linearVelocity = _playerVelocity;

        bool horizontalMovement = Mathf.Abs(_playerVelocity.x) > Mathf.Epsilon;
        _animator.SetBool("isWalking", horizontalMovement);
    }
    void FlipDirection()
    {
        bool hasHorizontalMovement = Mathf.Abs(_rigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (hasHorizontalMovement)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody.linearVelocity.x), 1f);
        }
    }
    void Climb()
    {
        if (!_capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            _rigidbody.gravityScale = gravityScaleAtStart;
            _animator.SetBool("isClimbing", false);

            return;
        }
        else
        {
            _rigidbody.gravityScale = gravityScaleOnClimb;
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _directionalInput.y * moveSpeed);

            bool isClimbing = Mathf.Abs(_directionalInput.y) > Mathf.Epsilon;

            _animator.SetBool("isClimbing", isClimbing);
        }
    }
    private void Die()
    {
        bool isTouchingHazard = _boxCollider.IsTouchingLayers(LayerMask.GetMask("Hazard"));

        if (isTouchingHazard)
        {
            print("ded");

            isAlive = false;

            _rigidbody.linearVelocity = new Vector2(0f, jumpForce);
            _animator.SetBool("isDead", true);

        }
    }
}
