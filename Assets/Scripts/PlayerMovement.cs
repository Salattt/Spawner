using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Transform))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundChecker;

    private Rigidbody2D _rb;
    private Animator _anim;
    private Transform _transform;
    private bool _isFaceRight = true;
    private CircleCollider2D _circleCollider;
    private int _isOnGroundHash = Animator.StringToHash("isOnGround");
    private int _isAttackHash = Animator.StringToHash("isAttack");
    private int _horizontalSpeedHash = Animator.StringToHash("horizontalSpeed");

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _circleCollider = _groundChecker.GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        Walk();
        Jump();

        _anim.SetBool(_isOnGroundHash, CheckIsOnGround());
        _anim.SetBool(_isAttackHash,Input.GetKey(KeyCode.Space));
    }

    private void Walk()
    { 
        float runMultiplicator = 1.5f;

        if (Input.GetKey(KeyCode.D) && CheckIsOnGround())
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (_rb.velocity.x < _maxSpeed * runMultiplicator)
                    _rb.AddForce(Vector2.right * _speed * runMultiplicator);
                else
                    _rb.velocity = new Vector2(_maxSpeed * runMultiplicator, _rb.velocity.y);
            }
            else
            {
                if (_rb.velocity.x < _maxSpeed)
                    _rb.AddForce(Vector2.right * _speed);
                else
                    _rb.velocity = new Vector2(_maxSpeed, _rb.velocity.y);
            }

            Reflect();
        }

        if (Input.GetKey(KeyCode.A) && CheckIsOnGround())
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (-1 * _rb.velocity.x < _maxSpeed * runMultiplicator )
                    _rb.AddForce(Vector2.left * _speed * runMultiplicator);
                else
                    _rb.velocity = new Vector2(_maxSpeed * runMultiplicator * -1, _rb.velocity.y) ;
            }
            else
            {
                if ((-1 * _rb.velocity.x) < _maxSpeed)
                    _rb.AddForce(Vector2.left * _speed);
                else
                    _rb.velocity = new Vector2(-1 *_maxSpeed, _rb.velocity.y);
            }

            Reflect();
        }

        _anim.SetFloat(_horizontalSpeedHash, Mathf.Abs(_rb.velocity.x));
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && CheckIsOnGround())
            _rb.AddForce(Vector2.up * _jumpForce);
    }

    private void Reflect()
    {
        if((_rb.velocity.x > 0.2f && _isFaceRight == false) || (_rb.velocity.x < -0.2f && _isFaceRight == true))
        {
            _isFaceRight = !_isFaceRight;
            _transform.localScale *= new Vector2(-1,1);
        }
    }

    private bool CheckIsOnGround()
    {
        return Physics2D.OverlapCircle(_groundChecker.position, _circleCollider.radius, _groundLayer);
    }
}
