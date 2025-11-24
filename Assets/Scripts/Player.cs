using UnityEngine;
using UnityEngine.Accessibility;

public class CharacterMovement : MonoBehaviour
{
    public float maxSpeed;
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    
    [field: SerializeField]public int Coin {  get;  set; } = 0;
    [field: SerializeField]public int Stamina { get; set; } = 10;



    private Rigidbody2D rb;
    Animator anim;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float moveInput = 0f;

        // �Թ���´��� A
        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
        }
        // �Թ��Ҵ��� D
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
        }

        // ���������ǡ������͹���
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // ���ⴴ���ʹ (����Ǩ�ͺ���)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }


    public void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

        anim.SetFloat("vSpeed", rb.linearVelocity.y);

        float move = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(move));

        rb.linearVelocity = new Vector2(move * maxSpeed, rb.linearVelocity.y);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Item item = other.GetComponent<Item>();
        if(item)
        {
            item.PickUp(this);
        }
    }

    public void AddCoin(int value)
    {
        Coin += value;
        Debug.Log("Coin +1 CurrentCoin: " +  Coin);
    }

    public void Heal(int value)
    {
        Stamina += value;
        Debug.Log("Heal + 10 Health  CurrentHealth: "+ Stamina);

    }

    public void TakeDamage(int value)
    {
        Stamina -= value;
        Debug.Log("Heal - 10 Health  CurrentHealth: " + Stamina);

    }

    public void Knockback(Vector2 direction, float force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.linearVelocity = Vector2.zero; // ��૵�������������������͹
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }
}
