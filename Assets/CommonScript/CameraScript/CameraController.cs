using Cinemachine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class CameraController : MonoBehaviour,IObserverPlayer
{
    [SerializeField] public CinemachineFreeLook CinemachineFreeLook;
    [SerializeField] public PlayerController playerController;
    [SerializeField] public CinemachineCameraOffset cameraOffset;
    [SerializeField] public CinemachineImpulseSource impulseSource;
    [SerializeField] public Player Player;
    private CamerOverShoulder cameraOverShoulder;
    public CameraKickBack cameraKickBack;
    public CameraZoom cameraZoom;
    public CameraHandShake cameraHandShake;

    public enum CameraKickbackPreset
    {
        N1,
        N2, 
        N3,
        N4,
    }
    public CameraKickbackPreset cameraKickbackPreset;
    public Dictionary<CameraKickbackPreset,float> Kickback = new Dictionary<CameraKickbackPreset, float>();


    void Start()
    {
        Player = FindAnyObjectByType<Player>();
        Kickback.Add(CameraKickbackPreset.N1 , 0.02f);
        Kickback.Add(CameraKickbackPreset.N2, 0.04f);
        Kickback.Add(CameraKickbackPreset.N3, 0.06f);
        Kickback.Add(CameraKickbackPreset.N4, 0.08f);
        //Cursor.lockState = CursorLockMode.Locked;    
        Player.AddObserver(this);
        cameraOverShoulder = new CamerOverShoulder(this);
        cameraKickBack = new CameraKickBack(this);
        cameraZoom = new CameraZoom(this);
        cameraHandShake = new CameraHandShake(this);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.T)){
            Cursor.lockState = CursorLockMode.None;
            EditorApplication.isPaused = true;
            return;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            EditorApplication.isPaused = false;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.SwapShoulder)
        {
            cameraOverShoulder.Performed();
        }
        if(playerAction == SubjectPlayer.PlayerAction.Firing)
        {
            cameraKickBack.Performed(player.currentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.Aim)
        {
            cameraZoom.ZoomIn(player.currentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            cameraZoom.ZoomOut(player.currentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.GetShoot)
        {
            cameraHandShake.Performed();
        }
        if(playerAction == SubjectPlayer.PlayerAction.Sprint)
        {
            
        }
    }

    public void OnNotify(Player player)
    {
        
    }
}
