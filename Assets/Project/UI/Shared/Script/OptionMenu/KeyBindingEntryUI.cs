using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindingEntryUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI actionNameText;
    [SerializeField] public TextMeshProUGUI bindingText;
    [SerializeField] public Image bindingIconImage;
    [SerializeField] public Button rebindButton;
    [SerializeField] public TextMeshProUGUI waitingText;

    public void SetBinding(string actionDisplayName, string bindingPath, string bindingDisplayString, KeyBindingIconDataScriptableObject iconData)
    {
        actionNameText.text = actionDisplayName;

        if (iconData != null && iconData.TryGetIcon(bindingPath, out Sprite icon))
        {
            bindingIconImage.sprite = icon;
            bindingIconImage.gameObject.SetActive(true);
            bindingText.gameObject.SetActive(false);
        }
        else
        {
            bindingText.text = bindingDisplayString;
            bindingText.gameObject.SetActive(true);
            bindingIconImage.gameObject.SetActive(false);
        }
    }

    public void SetWaiting(bool isWaiting)
    {
        waitingText.gameObject.SetActive(isWaiting);
        rebindButton.interactable = !isWaiting;
    }
}
