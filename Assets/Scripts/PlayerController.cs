using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    private Vector2 PlayerInput;
    private bool isMoving;
    private Animator animator;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableObjects;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        Vector2 moveForce = PlayerInput * moveSpeed;

        rb.velocity = moveForce;
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        facingDir *= 0.1f; // réduit la taille du "laser" tiré pour intéragir avec les objets qui peuvent être intéragis
        var interactPos = transform.position + facingDir;
         
        //Debug.DrawLine(transform.position, interactPos , Color.red, 1f);

        var collider = Physics2D.OverlapCircle(interactPos, .2f, interactableObjects);

        if (collider != null) 
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

}