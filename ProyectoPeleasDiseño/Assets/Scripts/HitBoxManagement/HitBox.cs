using System.ComponentModel;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] public BaseCharacter mCharacter;
    private Collider mCollider;
    private MeshRenderer mRenderer;

    [SerializeField] Material normalMat;
    [SerializeField] Material blockMat;
    [SerializeField] Material hurtMat;
    [SerializeField] Material blockedMat;
    public int mHeight;

    [HideInInspector] public bool isHurt;
    [HideInInspector] public bool isBlocking;
    [HideInInspector] public HurtBox lastHurtBox;

    private void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
        mCollider = GetComponent<Collider>();
        ManageMaterial();
    }

    public void EnableHurt()
    {
        isHurt = true;
        ManageMaterial();
    }
    public void DisableHurt()
    {
        isHurt = false;
        lastHurtBox.onDisableAttack-= DisableHurt;
        lastHurtBox = null;
        ManageMaterial();
    }
    public void EnableBlock()
    {
        isBlocking = true;
        ManageMaterial();
    }
    public void DisableBlock()
    {
        isBlocking = false;
        ManageMaterial();
    }

    private void ManageMaterial()
    {
        if (isHurt) 
        {
            if (isBlocking)
            {
                mRenderer.material = blockedMat;
            }
            else
            {
                mRenderer.material = hurtMat;
            }
        } 
        else
        {
            if (isBlocking)
            {
                mRenderer.material = blockMat;
            }
            else
            {
                mRenderer.material = normalMat;
            }
        }
    }
}
