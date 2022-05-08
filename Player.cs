using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 startPos;
    public Rigidbody2D rigidBody;

    private Animator animator;
    private SpriteRenderer sprite;

    private AudioManager audioManager;

    private bool isGrounded;
    public bool gameOver;

    public float tapForce;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        audioManager = FindObjectOfType<AudioManager>();

        isGrounded = true;
        gameOver = true;
    }

    public bool changeDirection(string direction) {
        if (isGrounded) {
            jump();

            if (direction.Equals("right")) {
                transform.localPosition = new Vector3(-startPos.x, transform.position.y, transform.position.z);
                sprite.flipX = true;
            }
            else {
                transform.localPosition = new Vector3(startPos.x, transform.position.y, transform.position.z);
                sprite.flipX = false;
            }

            return true;
        } 
        return false; 
    }

    private void jump() {
        isGrounded = false;
        animator.Play("jump", 0, 0);
        audioManager.Play("jump");

        Vector2 velocity = rigidBody.velocity;
        velocity.y = tapForce;
        rigidBody.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.relativeVelocity.y > 0f) 
        {
            if (col.gameObject.tag == "left" || col.gameObject.tag == "right") {
                isGrounded = true;
            } 
        }
    }

    public void reset() {
        transform.localPosition = startPos;
        sprite.flipX = false;

        isGrounded = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }

}
