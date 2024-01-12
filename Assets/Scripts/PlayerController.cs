using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    private Vector2 PlayerInput;
    public bool isMoving;
    private Animator animator;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableObjects;

    public DialogueManager dialogueManager;

    public float delay = 0.3f;
    private bool attackBlocked;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        dialogueManager = DialogueManager.Instance;
    }
    public void HandleUpdate()
    {
        isMoving = false;
        if (!isMoving)
        {
            PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            if (PlayerInput != Vector2.zero)
            {
                animator.SetFloat("moveX", PlayerInput.x);
                animator.SetFloat("moveY", PlayerInput.y);

                var targetPos = transform.position;
                targetPos.x += PlayerInput.x;
                targetPos.y += PlayerInput.y;

                isMoving = true;
                
            }
        }

        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        if (dialogueManager.HasSword)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
        }

        Vector2 moveForce = PlayerInput * moveSpeed;

        rb.velocity = moveForce;
    }

    void Attack()
    {
        if (attackBlocked)
        {
            return;
            Debug.Log("can't attack");
        }
        Debug.Log("attacked");
        animator.SetTrigger("AttackTrigger");
        StartCoroutine(DelayAttack());

    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        facingDir *= 0.001f; // réduit la taille du "laser" tiré pour intéragir avec les objets qui peuvent être intéragis
        var interactPos = transform.position + facingDir;
         
        Debug.DrawLine(transform.position, interactPos , Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, .09f, interactableObjects);

        if (collider != null) 
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

}