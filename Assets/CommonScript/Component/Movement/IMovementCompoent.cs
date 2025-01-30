using UnityEngine;

public interface IMovementCompoent
{
    public MonoBehaviour userMovement { get; set; }
    public Vector3 moveInputVelocity_World { get; set; }
    public Vector3 curMoveVelocity_World { get; set; }
    public Vector3 moveInputVelocity_Local { get; set; }
    public Vector3 curMoveVelocity_Local { get; set; }
    public Vector3 forwardDir { get; set; }
    //public Vector3 lookRotationCommand { get; set; }

    //public float _moveAccelerate { get; set; } // 1
    //public float _moveMaxSpeed { get; set; }//3

    //public float _sprintAccelerate { get; set; }//2.5
    //public float _sprintMaxSpeed { get; set; }//5.5
    //public float _rotateSpeed { get; set; }

    public void MovementUpdate();
    public void MovementFixedUpdate();
    public void MoveToDirWorld(Vector3 dirWorldNormalized,float speed,float maxSpeed);
    public void MoveToDirLocal(Vector3 dirLocalNormalized,float speed,float maxSpeed);
    public void RotateToDirWorld(Vector3 lookDirWorldNomalized,float rotateSpeed);
    public void GravityUpdate();

    public enum Stance
    {
        Stand,
        Crouch,
        Prone
    }
    public Stance curStance { get; set; }
    public bool isGround { get; set; }
    //public bool isSprint { get; set; }

}
