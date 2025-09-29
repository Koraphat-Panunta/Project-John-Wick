using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SliderValueToStringText : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI textValue;

    private void Awake()
    {
        slider.onValueChanged.AddListener(OnValueChange);
    }
    private void OnValueChange(float value)
    {
        textValue.text = Mathf.RoundToInt(value).ToString();
    }
    private void OnEnable()
    {
        if (slider != null) 
        {
            textValue.text = Mathf.RoundToInt(slider.value).ToString();
        }
    }
}
