using System.Collections;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class BaseCharacter : MonoBehaviour
{
    public float animSpeed = 1;
    public float moveSpeed = 5;
    private bool isAttacking = false;
    public float attackDamage = 1.15f;
    [Range(0,1)]public float defense;
    public float maxHealth = 150;
    [SerializeField] private float currentHealth;
    private bool isDead;
    private HurtBox lastHurtBox;
    private bool canMove = true;
    private Rigidbody m_rb;
    public float topAttackDamage = 10f;
    public float midAttackDamage = 10f;
    public float downAttackDamage = 10f;
    [SerializeField] private int topBlockStopFrames;
    [SerializeField] private int midBlockStopFrames;
    [SerializeField] private int downBlockStopFrames;
    [SerializeField] private HurtBox[] m_upHurtBoxes;
    [SerializeField] private HurtBox[] m_midHurtBoxes;
    [SerializeField] private HurtBox[] m_downHurtBoxes;
    [SerializeField] private HitBox[] m_blockHitBoxes;
    public Animator m_anim;
    [SerializeField] private bool isDummy;
    private bool isBlocking;
    private int blockHeight;

    [Header("UI")] [SerializeField] 
    private Slider healthBar;
    public float healthBarSpeed;
    public TextMeshProUGUI attackNameText;


    [Header("Damage Numbers")] 
    [SerializeField] private GameObject dmgNum;
    private Camera mainCam;
    private void Start()
    {
        mainCam = Camera.main;
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        m_rb = GetComponent<Rigidbody>();
        m_anim.SetFloat($"AnimSpeed", animSpeed);
        if (isDummy)
        {
            //StartCoroutine(DummieRotation());
        }
    }

    IEnumerator DummieRotation()
    {
        Block(true, 2);
        yield return new WaitForSeconds(5);
        Block(true, 0);
        yield return new WaitForSeconds(5);
        Block(false, 0);
        isAttacking = true;
        canMove = false;
        m_anim.SetTrigger($"MidAttack");
        yield return new WaitForSeconds(5);
        StartCoroutine(DummieRotation());
    }

    public void MidAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            canMove = false;
            m_anim.SetTrigger($"MidAttack");
        }
    }
    private void Update()
    {
        healthBar.value = Mathf.Lerp(healthBar.value, currentHealth, healthBarSpeed * Time.deltaTime);
        if (isDummy) return;
        if (isDead) return;
        if (Input.GetKey(KeyCode.D) & canMove)
        {
            Move(false);
            m_anim.SetBool($"IsMoving",true);
            m_anim.SetBool($"MovingBack", false);
            
        }
        else if (Input.GetKey(KeyCode.A) & canMove)
        {
            Move(true);
            m_anim.SetBool($"IsMoving", true);
            m_anim.SetBool($"MovingBack", true);
        }
        else 
        {
            m_anim.SetBool($"IsMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.Q) & !isAttacking)
        {
            isAttacking = true;
            canMove = false;
            m_anim.SetTrigger($"UpAttack");
            if(attackNameText) attackNameText.text = "ATTACK_UP";
        }

        if (Input.GetKeyDown(KeyCode.W) & !isAttacking)
        {
            isAttacking = true;
            canMove = false;
            m_anim.SetTrigger($"MidAttack");
            if(attackNameText) attackNameText.text = "ATTACK_MID";
        }

        if (Input.GetKeyDown(KeyCode.E) & !isAttacking)
        {
            isAttacking = true;
            canMove = false;
            m_anim.SetTrigger($"DownAttack");
            if(attackNameText) attackNameText.text = "ATTACK_DOWN";
        }
    }

    public void Block(bool isOn, int height)
    {
        if (isOn)
        {
            m_anim.SetBool($"IsBlocking", true);
            isBlocking = true;
            blockHeight = height;
            if(height == 2)
            {
                m_anim.SetBool($"BlockUp", true );
            }
            else if (height == 0)
            {
                m_anim.SetBool($"BlockUp", false);
            }
        }
        else
        {
            m_anim.SetBool($"IsBlocking", false);
            isBlocking = false;
        }
    }

    public void YouGotBlocked(int height)
    {
        Debug.Log("Stop Block");
        if(height == 0)
        {
            StartCoroutine(BlockStop(downBlockStopFrames));
        }
        else if (height == 1)
        {
            StartCoroutine(BlockStop(midBlockStopFrames));
        }
        else
        {
            StartCoroutine(BlockStop(topBlockStopFrames));
        }
    }

    IEnumerator BlockStop(int frames)
    {
        Debug.Log("Frames " + frames);
        float seconds = frames * 0.034f;
        Debug.Log("Seconds "+ seconds);
        m_anim.SetFloat($"AnimSpeed", 0);
        yield return new WaitForSecondsRealtime(seconds);
        m_anim.SetFloat($"AnimSpeed", animSpeed);
    }

    public void EndAttack()
    {
        canMove = true;
        isAttacking = false;
       
        if(attackNameText) attackNameText.text = "";
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
            Block(false, 0);
            hurtBox.EnableAttack();
            hurtBox.mHeight = 2;
            hurtBox.mDamage = topAttackDamage;
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
            Block(false,0);
            hurtBox.EnableAttack();
            hurtBox.mHeight = 1;
            hurtBox.mDamage = midAttackDamage;
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
            Block(false, 0);
            hurtBox.EnableAttack();
            hurtBox.mHeight = 0;
            hurtBox.mDamage = downAttackDamage;
        }
    }

    public void DownAttackDisable()
    {
        foreach (HurtBox hurtBox in m_downHurtBoxes)
        {
            hurtBox.DisableAttack();
        }
    }


    public void BlockUp()
    {
        Block(true, 2);
    }

    public void BlockDown()
    {
        Block(true, 0);
    }

    public void Unblock()
    {
        Block(false, 0);
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
        if (isDead) return;
        if(lastHurtBox == hurtBox) return;

        lastHurtBox = hurtBox;
        hitBox.lastHurtBox = hurtBox;

        hurtBox.onDisableAttack += hitBox.DisableHurt;
        hurtBox.onDisableAttack += ForgetLastHurtBox;

        hitBox.EnableHurt();

        var num = Instantiate(dmgNum, mainCam.WorldToScreenPoint(hurtBox.transform.position), dmgNum.transform.rotation).GetComponentInChildren<DamageNum>();
        num.gameObject.transform.GetChild(0).transform.position =
            mainCam.WorldToScreenPoint(hurtBox.transform.position);
        if ((isBlocking & blockHeight==hurtBox.mHeight) || (isBlocking & hurtBox.mHeight == 1))
        {
            Debug.Log("Blocked on height " + hurtBox.mHeight);
            attackingCharacter.YouGotBlocked(hurtBox.mHeight);
            if(num) num.SetText($"Blocked");
        }
        else
        {
            m_anim.SetTrigger($"Hit");
            Debug.Log("Hit on height " + hurtBox.mHeight); 
            currentHealth -= (hurtBox.mDamage - (hurtBox.mDamage * defense));
            if(num) num.SetText($"-{hurtBox.mDamage}");
            if (currentHealth <= 0) Death();
            EndAttack();
        }  
    }

    private void Death()
    {
        currentHealth = 0;
        isDead = true;
        m_anim.SetTrigger($"Die");
    }
    
    public void ForgetLastHurtBox()
    {
        lastHurtBox.onDisableAttack -= ForgetLastHurtBox;
        lastHurtBox = null;
    }
}
