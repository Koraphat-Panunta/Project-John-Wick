using UnityEngine;

public abstract class OptionUIDisplayer 
{
    public GameObject optionCanvasSector { get; protected set; }
    public OptionUIDisplayer(GameObject canvasSector)
    {
        this.optionCanvasSector = canvasSector;
    }
    public void Show(DataBased dataBased)
    {
        this.optionCanvasSector.SetActive(true);
        this.Load(dataBased);
    }
    public void Hide()
    {
        this.optionCanvasSector?.SetActive(false);
    }
    protected abstract void Load(DataBased dataBased);
}
