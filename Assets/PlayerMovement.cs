using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //I recommend 7 for the move speed, and 1.2 for the force damping
    public Rigidbody2D rb;
    public float moveSpeed;
    public Vector2 forceToApply; //force du knockback, la valeure se disssipe
    public Vector2 PlayerInput;
    public float forceDamping; //affecte à la vitesse à la quelle forceToApply est réduite, ne pas toucher, vaut mieux jouer avec la force du kb
    void Update()
    {
        PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    void FixedUpdate()
    {
        Vector2 moveForce = PlayerInput * moveSpeed;
        moveForce += forceToApply;
        forceToApply /= forceDamping;
        if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.y) <= 0.01f) // vérifie si on a besoin d'infliger un kb au joueur
        {
            forceToApply = Vector2.zero;
        }
        rb.velocity = moveForce;
    }

    private void OnCollisionEnter2D(Collision2D collision) // knockback on collision
    {
        if (collision.collider.CompareTag("INSERTTAGHERE"))
        {
            forceToApply += new Vector2(-20, 0);
            Destroy(collision.gameObject);
        }
    }
}