using System;
using UnityEngine;

namespace Gisha.GameOff_2021
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;

        private float _hInput;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _hInput = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            _rb.velocity = transform.right * _hInput * moveSpeed;
        }
    }
}