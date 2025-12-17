using UnityEngine;
using UnityEngine.UI;

public class LerpSlider : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private Slider m_lerpSlider;
    
    void Start()
    {
        m_lerpSlider = GetComponent<Slider>();
        m_lerpSlider.maxValue = healthSlider.maxValue;
        m_lerpSlider.value = healthSlider.value;
    }
    
    void Update()
    {
        m_lerpSlider.maxValue = healthSlider.maxValue;
        if (m_lerpSlider.value != healthSlider.maxValue)
        {
            m_lerpSlider.value = Mathf.Lerp(m_lerpSlider.value, healthSlider.value, 1.5f * Time.deltaTime);
        }
    }
}
