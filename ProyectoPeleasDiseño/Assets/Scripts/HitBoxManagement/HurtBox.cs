using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField] private BaseCharacter mCharacter;
    [SerializeField] private Material normalMat;
    [SerializeField] private Material attackMat;
    private Collider mCollider;
    private MeshRenderer mMeshRenderer;

    public delegate void DisabledAttack();
    public DisabledAttack onDisableAttack;

    [HideInInspector] public int mHeight;
    [HideInInspector] public bool isAttacking;

    private void Start()
    {
        mCollider = GetComponent<Collider>();
        mMeshRenderer = GetComponent<MeshRenderer>();
        if(isAttacking) EnableAttack();
        else DisableAttack();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isAttacking) return;

        HitBox otherHitbox = other.GetComponent<HitBox>();
        if (otherHitbox == null) return;
        if (otherHitbox.mCharacter == mCharacter) return;
        otherHitbox.mCharacter.DealDamage(mCharacter, this, otherHitbox);
    }

    public void EnableAttack()
    {
        mMeshRenderer.material = attackMat;
        isAttacking = true;
    }

    public void DisableAttack()
    {
        mMeshRenderer.material = normalMat;
        isAttacking = false;
        onDisableAttack?.Invoke();
    }
}
