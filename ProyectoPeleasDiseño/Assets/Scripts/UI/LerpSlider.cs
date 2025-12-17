using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LerpSlider : MonoBehaviour
{
    [SerializeField] private Slider otherSlider;
    private Slider m_lerpSlider;
    
    void Start()
    {
        m_lerpSlider = GetComponent<Slider>();
        m_lerpSlider.maxValue = otherSlider.maxValue;
        m_lerpSlider.value = otherSlider.value;
    }
    
    void Update()
    {
        m_lerpSlider.maxValue = otherSlider.maxValue;
        if (m_lerpSlider.value != otherSlider.value)
        {
            m_lerpSlider.value = Mathf.Lerp(m_lerpSlider.value, otherSlider.value, 1.5f * Time.deltaTime);
        }
    }
}
