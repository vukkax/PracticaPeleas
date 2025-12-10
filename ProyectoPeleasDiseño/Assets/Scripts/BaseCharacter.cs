using UnityEngine;
using UnityEngine.Rendering;

public class BaseCharacter : MonoBehaviour
{
    public float animSpeed = 1;
    public float moveSpeed = 5;
    private bool isAttacking = false;
    public float attackDamage = 1.15f;
    public float defense = 1.2f;
    public int maxHealth = 150;
    private int currentHealth;
    private HurtBox lastHurtBox;
    private bool canMove = true;
    private Rigidbody m_rb;
    [SerializeField] private HurtBox[] m_upHurtBoxes;
    [SerializeField] private HurtBox[] m_midHurtBoxes;
    [SerializeField] private HurtBox[] m_downHurtBoxes;
    [SerializeField] private HitBox[] m_blockHitBoxes;
    [SerializeField] private Animator m_anim;
    [SerializeField] private bool isDummy;
    private void Start()
    {
        if (isDummy) return;
        currentHealth = maxHealth;
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isDummy) return;
        if (Input.GetKey(KeyCode.D) & canMove)
        {
            Move(false);
            m_anim.SetBool("IsMoving",true);
            m_anim.SetBool("MovingBack", false);
        }
        else if (Input.GetKey(KeyCode.A) & canMove)
        {
            Move(true);
            m_anim.SetBool("IsMoving", true);
            m_anim.SetBool("MovingBack", true);
        }
        else 
        {
            m_anim.SetBool("IsMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.Q) & !isAttacking)
        {
            isAttacking = true;
            canMove = false;
            m_anim.SetTrigger("UpAttack");
        }

        if (Input.GetKeyDown(KeyCode.W) & !isAttacking)
        {
            isAttacking = true;
            canMove = false;
            m_anim.SetTrigger("MidAttack");
        }

        if (Input.GetKeyDown(KeyCode.E) & !isAttacking)
        {
            isAttacking = true;
            canMove = false;
            m_anim.SetTrigger("DownAttack");
        }
    }

    public void EndAttack()
    {
        canMove = true;
        isAttacking = false;
    }

    private void Move(bool backMovement)
    {
        float horizontalMovement = backMovement ? -1f : 1f;
        horizontalMovement = horizontalMovement * moveSpeed * 0.0012f;
        transform.position += new Vector3(horizontalMovement,0,0);
    }

    public void UpAttackEnable()
    {
        foreach (HurtBox hurtBox in m_upHurtBoxes)
        {
            hurtBox.EnableAttack();
        }
    }

    public void UpAttackDisable()
    {
        foreach (HurtBox hurtBox in m_upHurtBoxes)
        {
            hurtBox.DisableAttack();
        }
    }

    public void MidAttackEnable()
    {
        foreach (HurtBox hurtBox in m_midHurtBoxes)
        {
            hurtBox.EnableAttack();
        }
    }

    public void MidAttackDisable()
    {
        foreach (HurtBox hurtBox in m_midHurtBoxes)
        {
            hurtBox.DisableAttack();
        }
    }

    public void DownAttackEnable()
    {
        foreach (HurtBox hurtBox in m_downHurtBoxes)
        {
            hurtBox.EnableAttack();
        }
    }

    public void DownAttackDisable()
    {
        foreach (HurtBox hurtBox in m_downHurtBoxes)
        {
            hurtBox.DisableAttack();
        }
    }

    public void BlockEnable()
    {
        foreach(HitBox hitBox in m_blockHitBoxes)
        {
            hitBox.EnableBlock();
        }
    }

    public void BlockDisable()
    {
        foreach (HitBox hitBox in m_blockHitBoxes)
        {
            hitBox.DisableBlock();
        }
    }

    public void DealDamage(BaseCharacter attackingCharacter, HurtBox hurtBox, HitBox hitBox)
    {
        if(lastHurtBox == hurtBox) return;

        lastHurtBox = hurtBox;
        hitBox.lastHurtBox = hurtBox;

        hurtBox.onDisableAttack += hitBox.DisableHurt;
        hurtBox.onDisableAttack += ForgetLastHurtBox;

        hitBox.EnableHurt();

        if (hitBox.isBlocking)
        {
            Debug.Log("Blocked on height " + hitBox.mHeight);
        }
        else 
        {
            Debug.Log("Hit on height " + hitBox.mHeight);
        }  
    }

    public void ForgetLastHurtBox()
    {
        lastHurtBox.onDisableAttack -= ForgetLastHurtBox;
        lastHurtBox = null;
    }
}
