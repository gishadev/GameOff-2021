using System;
using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 10f;

        private float _hInput;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _hInput = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
        }

        private void FixedUpdate()
        {
            _rb.velocity = new Vector2(_hInput * moveSpeed * Time.deltaTime, _rb.velocity.y);
        }

        private void Jump()
        {
            _rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}