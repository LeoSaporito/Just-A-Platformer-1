using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 10f;

    Rigidbody2D _rigidbody;
    
    Vector2 directionalInput;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector2(directionalInput.x * moveSpeed, _rigidbody.linearVelocity.y);
    }

    void OnMove(InputValue value)
    {
        directionalInput = value.Get<Vector2>();
        print(directionalInput);
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpForce);
        }
    }
}
