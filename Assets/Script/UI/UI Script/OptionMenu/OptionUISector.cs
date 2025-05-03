using UnityEngine;
using UnityEngine.UI;

public abstract class OptionUISector 
{
    public OptionUICanvas optionUICanvas { get; protected set; }
    public Button applyButton { get; protected set; }
    public GameObject canvasSector { get; protected set; }
    public OptionUISector(OptionUICanvas optionUICanvas, Button applyButton, GameObject canvasSector)
    {
        this.optionUICanvas = optionUICanvas;
        this.applyButton = applyButton;
        this.canvasSector = canvasSector;

        this.applyButton.onClick.AddListener(Apply);
    }
    public abstract void Show();
    public abstract void Hide();
    public abstract void Apply();
    public abstract void Load();
}
