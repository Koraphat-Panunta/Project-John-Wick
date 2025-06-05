using UnityEngine;

public class CameraStateManagerDebugger : MonoBehaviour,IDebugger
{
    [SerializeField] private string curState;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private DebugLogScriptableObjectSave cameraDebugLog;
    [SerializeField, TextArea(10, 20)] public string debugLog;
    private CameraManagerNode cameraManagerNode => cameraController.cameraManagerNode;

    public enum CameraStateManagerDebuggerRequest
    {
        curState
    }
    private void Start()
    {
        cameraDebugLog.debugLog = "";
    }
    public CameraStateManagerDebuggerRequest request = CameraStateManagerDebuggerRequest.curState;
    private void FixedUpdate()
    {
#if UNITY_EDITOR
        DebugCurStatePath();
#endif
        request = CameraStateManagerDebuggerRequest.curState;
        curState = cameraManagerNode.Debugged<CameraNodeLeaf>(this).GetType().ToString();
    }
    private void Update()
    {
        debugLog = this.cameraDebugLog.debugLog;
    }

    private void DebugCurStatePath()
    {
        INode node = cameraManagerNode.Debugged<INode>(this);

        try
        {
            if(node.parentNode == null)
                return;

            if (node is INodeLeaf nodeLeaf)
                cameraDebugLog.debugLog += node;

            else if (node is NodeSelector nodeSelector)
                cameraDebugLog.debugLog += nodeSelector.nodeName;

            cameraDebugLog.debugLog += "\n => ";
            while (node.parentNode != null)    
            {

                if (node.parentNode is INodeLeaf parentNodeLeaf)
                    cameraDebugLog.debugLog += parentNodeLeaf + "\n";
                else if (node.parentNode is NodeSelector parenNodeSelector)
                    cameraDebugLog.debugLog += parenNodeSelector.nodeName + "\n";

                node = node.parentNode;
            }
            cameraDebugLog.debugLog += "\n";

        }
        catch{
            throw new System.Exception("DebugCurStatePath is corrupt");
        }
    }
}
