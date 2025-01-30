using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamerOverShoulder:ICameraAction
{
    CameraController _camController;
    CinemachineCameraOffset _cameraOffset;
    Vector3 originalOffset;
    public CamerOverShoulder(CameraController camController)
    {
        _camController = camController;
        _cameraOffset = _camController.cameraOffset;
        originalOffset = _cameraOffset.m_Offset;
        curSide = Side.right;
    }
    public enum Side
    {
        left, right
    }
    public Side curSide { get; private set; }
    
    private IEnumerator ChangeCameraLeftSide()
    {
        while(_cameraOffset.m_Offset.x > -originalOffset.x )
        {
            if(curSide != Side.left)
            {
                yield break;
            }
            _cameraOffset.m_Offset.x = Mathf.Lerp(_cameraOffset.m_Offset.x, -originalOffset.x, 4 * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator ChangeCameraRightSide()
    {
        while (_cameraOffset.m_Offset.x < originalOffset.x )
        {
            if (curSide != Side.right)
            {
                yield break;
            }
            _cameraOffset.m_Offset.x = Mathf.Lerp(_cameraOffset.m_Offset.x, originalOffset.x, 4 * Time.deltaTime);
            yield return null;
        }
    }
    public void Performed()
    {
        if(curSide == Side.right)
        {
            curSide = Side.left;
            _camController.StartCoroutine(ChangeCameraLeftSide());
        }
        else if(curSide == Side.left)
        {
            curSide = Side.right;
            _camController.StartCoroutine(ChangeCameraRightSide());
        }
    }
}
