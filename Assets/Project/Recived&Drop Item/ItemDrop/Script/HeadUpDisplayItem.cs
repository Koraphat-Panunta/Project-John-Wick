using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class HeadUpDisplayItem : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI textMeshPro;
    protected abstract string textShow { get; set; }
    private void Awake()
    {
        canvas.worldCamera = Camera.main;
    }
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        canvas.transform.LookAt(canvas.worldCamera.transform.position);
        textMeshPro.text = textShow;
    }
}
