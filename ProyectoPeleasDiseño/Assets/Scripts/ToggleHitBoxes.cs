using UnityEngine;

public class ToggleHitBoxes : MonoBehaviour
{
    private HitBox[] hitBoxes;
    private HurtBox[] hurtBoxes;
    bool isOn = true;

    private bool attackUIisOn = true;

    public GameObject attackUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitBoxes = Resources.FindObjectsOfTypeAll<HitBox>();
        hurtBoxes = Resources.FindObjectsOfTypeAll<HurtBox>();
    }

    public void Toggle()
    {
        if(isOn)
        {
            isOn = false;
            foreach(HitBox hitBox in hitBoxes)
            {
                hitBox?.MeshEnable(false);
            }
            foreach (HurtBox hurtBox in hurtBoxes)
            {
                hurtBox?.MeshEnable(false);
            }
        }
        else
        {
            isOn = true;
            foreach (HitBox hitBox in hitBoxes)
            {
                hitBox?.MeshEnable(true);
            }
            foreach (HurtBox hurtBox in hurtBoxes)
            {
                hurtBox?.MeshEnable(true);
            }
        }
    }

    public void ToggleAttackUI()
    {
        if(attackUIisOn)
        {
            attackUIisOn = false;
            attackUI.SetActive(false);
        }
        else
        {
            attackUIisOn = true;
            attackUI.SetActive(true);
        }
    }
}
