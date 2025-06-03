using UnityEngine;

public class CameraStateManagerDebugger : MonoBehaviour,IDebugger
{
    [SerializeField] private string curState;
    [SerializeField] private CameraController cameraController;
    private CameraManagerNode cameraManagerNode => cameraController.cameraManagerNode;

    public enum CameraStateManagerDebuggerRequest
    {
        curState
    }
    public CameraStateManagerDebuggerRequest request = CameraStateManagerDebuggerRequest.curState;
    private void FixedUpdate()
    {
        request = CameraStateManagerDebuggerRequest.curState;
        curState = cameraManagerNode.Debugged<CameraNodeLeaf>(this).GetType().ToString();
    }
}
