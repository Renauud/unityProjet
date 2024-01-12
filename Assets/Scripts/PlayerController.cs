using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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

    public float delay = 0.8f;
    private bool attackBlocked; // possibilité d'utiliser une seule variable mais en utiliser 2 peut s'avérer utile pour le futur, bien que dans notre cas ce n'est pas le cas. (attackBlocked parce que nous nous sommes fait toucher par un ennemi en premier par exemple, etc..)
    public bool isAttacking;

    public Transform circleOrigin;
    public float radius;

    public Vector3 position;
    public float offset2;

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
            Debug.Log("can't attack");
            return;
        }
        Debug.Log("attacked");
        isAttacking = true;
        attackBlocked = true;
        animator.SetTrigger("AttackTrigger");
        StartCoroutine(DelayAttack());

    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
        isAttacking = false;
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

    private void OnDrawGizmosSelected() // doit ajouter une condition style isAttacking ou utiliser le attackTrigger pour que l'offset fonctionne bien
    {
        if (circleOrigin != null && isAttacking)
        {
            Gizmos.color = Color.blue;
            position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
            var offset1 = 0.13f;
            //var offset2 = 0.13f;
            // étant donné qu'il est impossible d'attaquer à gauche par manque de sprite, il n'y a que 3 possibilités puisque l'axe Y prend le dessus sur l'axe X
            //utilisation de animator GetFloat("moveX") pour offset l'épée sur l'attaque en fonction de la directions dans laquel on regarde pour améliorer la QoL
            var xDirection = animator.GetFloat("moveX");
            var yDirection = animator.GetFloat("moveY");
            if ((xDirection == 1 || xDirection == -1) && yDirection == 0)
            {
                position += new Vector3(offset1, -offset1, 0);
            }
            else
            {
                if(yDirection > 0)
                {
                    position += new Vector3(0, offset1 / 2, 0);
                }
                else
                {
                    position += new Vector3(0, -offset1 * (2-1/2), 0);
                }
            }


            Gizmos.DrawWireSphere(position, radius);
            DetectColliders(position);
        }
    }

    public void DetectColliders(Vector3 position)
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(position, radius))
        {
            Health health;
            if(health = collider.GetComponent<Health>())
            {
                health.GetHit(1);
            }
        }
    }
}