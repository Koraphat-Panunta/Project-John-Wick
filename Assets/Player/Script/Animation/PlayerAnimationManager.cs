using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour,IObserverPlayer
{
    public Animator animator;   
    public Player player;

    public string Sprint = "Sprint";
    public string Move_Idle = "Move/Idle";
    public string Crouch = "Crouch";
    

    public float CoverWeight;
    public float SholderSide;//-1 -> 1
    public float InputMoveMagnitude_Normalized;
    public float VelocityMoveMagnitude_Normalized;
    public float MoveInputLocalFoward_Normalized;
    public float MoveInputLocalSideWard_Normalized;
    public float MoveVelocityForward_Normalized;
    public float MoveVelocitySideward_Normalized;
    public float DotMoveInputWordl_VelocityWorld_Normalized;
    public float DotVectorLeftwardDir_MoveInputVelocity_Normallized;
    public float Rotating;
    public float AimDownSightWeight;
    public float DotVelocityWorld_Leftward_Normalized;
    public float RecoilWeight;
    public float PointRange;

    public bool isCover;

    public bool isLayer_1_Enable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        player.AddObserver(this);

        isLayer_1_Enable = true;
    }

    // Update is called once per frame
    void Update()
    {
        BackBoardUpdate();
        if (isLayer_1_Enable == true)
        {
            animator.SetLayerWeight(1, Mathf.Clamp01(animator.GetLayerWeight(1) + 10 * Time.deltaTime));
        }
        else
        {
            animator.SetLayerWeight(1,Mathf.Clamp01(animator.GetLayerWeight(1) - 10 * Time.deltaTime));
        }
    }
    private void FixedUpdate()
    {
        CalculateDeltaRotation();
    }
    private void BackBoardUpdate()
    {
       
        
        if (player.curShoulderSide == Player.ShoulderSide.Left)
            SholderSide = Mathf.Clamp(SholderSide - 100*Time.deltaTime, -1, 1);
        if (player.curShoulderSide == Player.ShoulderSide.Right)
            SholderSide = Mathf.Clamp(SholderSide + 100 * Time.deltaTime, -1, 1);

        if(player.isInCover)
            CoverWeight = Mathf.Clamp(CoverWeight + 100 * Time.deltaTime, 0, 1);
        else
            CoverWeight = Mathf.Clamp(CoverWeight - 100 * Time.deltaTime, 0, 1);

        PlayerMovement playerMovement = player.playerMovement;
        Vector3 inputVelocity_World = playerMovement.inputVelocity_World;
        Vector3 inputVelocity_Local = playerMovement.inputVelocity_Local;
        Vector3 curVelocity_Local = playerMovement.curVelocity_Local;
        Vector3 curVelocity_World = playerMovement.curVelocity_World;

        this.InputMoveMagnitude_Normalized = inputVelocity_World.normalized.magnitude;

        this.MoveInputLocalFoward_Normalized = inputVelocity_Local.normalized.z;
        this.MoveInputLocalSideWard_Normalized = inputVelocity_Local.normalized.x;

       
        this.DotMoveInputWordl_VelocityWorld_Normalized = Vector3.Dot(curVelocity_World.normalized
            , inputVelocity_World.normalized)*(curVelocity_World.magnitude/inputVelocity_World.magnitude);

        this.DotVectorLeftwardDir_MoveInputVelocity_Normallized = Mathf.Lerp(this.DotVectorLeftwardDir_MoveInputVelocity_Normallized, 
                Vector3.Dot(inputVelocity_World.normalized, 
                Vector3.Cross(player.transform.forward, Vector3.up))
            ,10*Time.deltaTime) ;


        if (player.curPlayerActionNode == player.playerSprintNode)
        {
            this.VelocityMoveMagnitude_Normalized = curVelocity_Local.magnitude / playerMovement.sprint_MaxSpeed;
            this.MoveVelocityForward_Normalized = curVelocity_Local.z / playerMovement.sprint_MaxSpeed;
            this.MoveVelocitySideward_Normalized = curVelocity_Local.x / playerMovement.sprint_MaxSpeed;

            
        }
        else
        {

            this.VelocityMoveMagnitude_Normalized = curVelocity_Local.magnitude / playerMovement.move_MaxSpeed;
            this.MoveVelocityForward_Normalized = curVelocity_Local.z / playerMovement.move_MaxSpeed;
            this.MoveVelocitySideward_Normalized = curVelocity_Local.x / playerMovement.move_MaxSpeed;

           
        }



        AimDownSightWeight = (player as IWeaponAdvanceUser).currentWeapon.aimingWeight;
        


        this.DotVelocityWorld_Leftward_Normalized = Vector3.Dot(
            Vector3.Cross(player.transform.forward, Vector3.up).normalized
            , playerMovement.curVelocity_World.normalized);

        if (RecoilWeight > 0)
            RecoilWeight = Mathf.Clamp(RecoilWeight - 3 * Time.deltaTime, 0, 1);

        if (player.currentWeapon != null){ 
            if (Vector3.Distance((player as IWeaponAdvanceUser).shootingPos
            , (player as IWeaponAdvanceUser).currentWeapon.bulletSpawnerPos.position)<2)

                PointRange = Mathf.Clamp(PointRange - 10 * Time.deltaTime, 0, 1);
            else
                PointRange = Mathf.Clamp(PointRange + 10 * Time.deltaTime, 0, 1);

        }
       

        animator.SetFloat("CoverWeight", CoverWeight);
        animator.SetFloat("SholderSide", SholderSide);
        animator.SetFloat("InputMoveMagnitude_Normalized", InputMoveMagnitude_Normalized);
        animator.SetFloat("VelocityMoveMagnitude_Normalized",VelocityMoveMagnitude_Normalized);
        animator.SetFloat("MoveInputLocalFoward_Normalized", MoveInputLocalFoward_Normalized);
        animator.SetFloat("MoveInputLocalSideWard_Normalized", MoveInputLocalSideWard_Normalized);
        animator.SetFloat("MoveVelocityForward_Normalized",MoveVelocityForward_Normalized);
        animator.SetFloat("MoveVelocitySideward_Normalized", MoveVelocitySideward_Normalized);
        animator.SetFloat("DotMoveInputWordl_VelocityWorld_Normalized",DotMoveInputWordl_VelocityWorld_Normalized);
        animator.SetFloat("Rotating",Rotating);
        animator.SetFloat("AimDownSightWeight",AimDownSightWeight);
        animator.SetFloat("DotVelocityWorld_Leftward_Normalized", DotVelocityWorld_Leftward_Normalized);
        animator.SetFloat("RecoilWeight", RecoilWeight);
        animator.SetFloat("PointRange", PointRange);
        animator.SetFloat("DotVectorLeftwardDir_MoveInputVelocity_Normallized", DotVectorLeftwardDir_MoveInputVelocity_Normallized);

        

    }

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.TakeCover)
            isCover = true;
        
        if(playerAction == SubjectPlayer.PlayerAction.GetOffCover)
            isCover = false;

        if(playerAction == SubjectPlayer.PlayerAction.Firing)
            RecoilWeight = 1;

        if(playerAction == SubjectPlayer.PlayerAction.Sprint)
        {
            Debug.Log("Recive Notify Sprint");
            animator.CrossFade(Sprint, 0.3f, 0,0);
            isLayer_1_Enable = true;
        }
        if(playerAction == SubjectPlayer.PlayerAction.Move||
            playerAction == SubjectPlayer.PlayerAction.Idle)
        {
            Debug.Log("Recive Notify Move/Idle");
            animator.CrossFade(Move_Idle, 0.3f, 0, 0);
            isLayer_1_Enable = true;
        }

        if(playerAction == SubjectPlayer.PlayerAction.GunFuEnter)
        {
            Debug.Log("Recive Notify GunFuEnter");
            isLayer_1_Enable = false;

            if(player.curPlayerActionNode == player.Hit1gunFuNode)
            animator.CrossFade("Hit", 0.05f, 0, 0);

            if (player.curPlayerActionNode == player.Hit2GunFuNode)
                animator.CrossFade("Hit2", 0.05f, 0, 0);

            if (player.curPlayerActionNode == player.knockDown_GunFuNode)
                animator.CrossFade("KnockDown", 0.05f, 0, 0);
        }

        if(playerAction == SubjectPlayer.PlayerAction.SwitchWeapon)
        {
            if (player.currentWeapon is PrimaryWeapon)
                animator.CrossFade("SwitchWeaponPrimary -> Secondary", 0.1f, 1);

            if (player.currentWeapon is SecondaryWeapon)
                animator.CrossFade("SwitchWeaponSecondary -> Primary", 0.1f,1);
        }

    }

    public void OnNotify(Player player)
    {
    }

    #region CalculateRotateRate
    private Vector3 previousDir;

    private void CalculateDeltaRotation()
    {
        Vector3 curDir = player.transform.forward;

        Rotating = Mathf.Lerp(Rotating,Vector3.SignedAngle(previousDir, curDir,Vector3.up) * Time.deltaTime/0.5f,10*Time.deltaTime);
        previousDir = curDir;
    }


    #endregion
    
}
