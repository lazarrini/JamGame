using System;
using UnityEngine;



public class Popochka : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private float speed = 3f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        
        
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        if (hit)
        {
            
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        }

    }
}



