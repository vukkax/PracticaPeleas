
using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNum : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpProComponent;
    [SerializeField] private float timeUntilFade;
    [SerializeField] private float verticalSpeed;
    [Header("AnimatorCurve")]
    [SerializeField] AnimationCurve animCurve;

    [SerializeField] private float curveTimerValue;
    private float curveTimer;
    private Animator anim;
    RectTransform rectTransform;
    

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
        tmpProComponent = GetComponentInChildren<TextMeshProUGUI>();

        if (!anim || !tmpProComponent)
        {
            print("No hay");
            
        }
        else
        {
            StartCoroutine(FadeText());
        }
    }

    private void Update()
    {
        curveTimer += Time.deltaTime;
        if (curveTimer >= curveTimerValue)
        {
            curveTimer = 0;
        }
        var normalizedValue = curveTimer / curveTimerValue;
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x + animCurve.Evaluate(normalizedValue), (rectTransform.localPosition.y + verticalSpeed*Time.deltaTime) ,0);;
    }

    public void SetText(string text)
    {
        tmpProComponent.text = text;
    }

    public void DestroyText()
    {
        Destroy(transform.parent.gameObject);
    }

    IEnumerator FadeText()
    {
        yield return new WaitForSeconds(timeUntilFade);
        anim.SetTrigger($"FadeOut");
    }
}
