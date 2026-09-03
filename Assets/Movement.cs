using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 6.0f;

    [Header("Turn Before Walk")]
    [Tooltip("Waktu jeda (detik) saat tombol ditekan agar karakter menoleh dulu sebelum melangkah")]
    [SerializeField] private float turnDelay = 0.12f; 

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movementInput;
    private Vector2 facingDirection = Vector2.down; // Default hadap bawah
    private float moveDelayTimer = 0f;
    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 1. Deteksi belok instan saat tombol A (kiri) atau D (kanan) baru ditekan
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetFacingDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetFacingDirection(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetFacingDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetFacingDirection(Vector2.down);
        }

        // 2. Hitung timer jeda gerak
        if (moveDelayTimer > 0f)
        {
            moveDelayTimer -= Time.deltaTime;
            movementInput = Vector2.zero; // Masih nengok di tempat, belum melangkah
        }
        else
        {
            // Jika tombol ditahan melebihi turnDelay, karakter mulai jalan
            movementInput = new Vector2(moveX, moveY).normalized;
            
            // Perbarui facing direction jika sedang jalan terus
            if (movementInput != Vector2.zero)
            {
                // Utamakan arah horizontal jika bergerak diagonal
                if (Mathf.Abs(moveX) > 0)
                    facingDirection = new Vector2(Mathf.Sign(moveX), 0);
                else
                    facingDirection = new Vector2(0, Mathf.Sign(moveY));
            }
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementInput * currentSpeed;
    }

    private void SetFacingDirection(Vector2 newDirection)
    {
        // Jika arah berubah, berikan jeda singkat agar ada efek "menoleh di tempat"
        if (facingDirection != newDirection)
        {
            facingDirection = newDirection;
            moveDelayTimer = turnDelay; 
        }
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        bool isMoving = movementInput.sqrMagnitude > 0.01f;

        animator.SetBool("IsMoving", isMoving);

        // Kirim arah hadap ke Blend Tree animator
        animator.SetFloat("MoveX", facingDirection.x);
        animator.SetFloat("MoveY", facingDirection.y);
    }

    public Vector2 FacingDirection => facingDirection;
}