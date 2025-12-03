using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    private HurtBox lastHurtBox;
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
