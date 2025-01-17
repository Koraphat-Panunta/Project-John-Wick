using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimationManager : MonoBehaviour,IObserverPlayer,IPlayerAnimationNode
{
    public Animator animator;   
    public Player player;

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

    public bool isSprint;
    public bool isAiming;
    public bool isTriggerDodge;
    public bool isTriggerMantle;
    public bool isTriggerGunFu;
    public Player.PlayerStance playerStance;
    public Player.ShoulderSide shoulderSide;
    public bool isGround;

    public bool isCover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        player.AddObserver(this);
        InitailizedPlayerAnimationNode();
    }

    // Update is called once per frame
    void Update()
    {
        BackBoardUpdate();
        UpdateNode();
    }
    private void FixedUpdate()
    {
        FixedUpdateNode();
        CalculateDeltaRotation();
    }
    private void BackBoardUpdate()
    {
        this.playerStance = player.playerStance;

        this.shoulderSide = player.curShoulderSide;

        this.isGround = player.playerMovement.isGround;
        
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

            isSprint = true;
        }
        else
        {

            this.VelocityMoveMagnitude_Normalized = curVelocity_Local.magnitude / playerMovement.move_MaxSpeed;
            this.MoveVelocityForward_Normalized = curVelocity_Local.z / playerMovement.move_MaxSpeed;
            this.MoveVelocitySideward_Normalized = curVelocity_Local.x / playerMovement.move_MaxSpeed;

            isSprint = false;
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

        if(playerAction == SubjectPlayer.PlayerAction.LowReady)
            isAiming = false;
        if(playerAction == SubjectPlayer.PlayerAction.Aim)
            isAiming = true;

       

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
    public void InitailizedPlayerAnimationNode()
    {
        playerAnimationManager = this;

        startSelectorAnimationNode = new PlayerAnimationNodeSelector(playerAnimationManager, animator, () => true);
        

        move_Idle_StandPlayerAnimationNodeLeaf = new Move_Idle_StandPlayerAnimationNodeLeaf(playerAnimationManager, animator,
            () => true);

        sprint_PlayerAnimationNodeLeaf = new Sprint_PlayerAnimationNodeLeaf(playerAnimationManager,animator,
            ()=> isSprint);

        startSelectorAnimationNode.AddChildNode(sprint_PlayerAnimationNodeLeaf);
        startSelectorAnimationNode.AddChildNode(move_Idle_StandPlayerAnimationNodeLeaf);

        startSelectorAnimationNode.Transition(out PlayerAnimationNodeLeaf playerAnimationNodeLeaf);
        curAnimationNodeLeaf = playerAnimationNodeLeaf;

        curAnimationNodeLeaf.Enter();
    }

    public void UpdateNode()
    {
        if (curAnimationNodeLeaf.IsReset())
        {
            curAnimationNodeLeaf.Exit();
            curAnimationNodeLeaf = null;
            startSelectorAnimationNode.Transition(out PlayerAnimationNodeLeaf playerAnimationNodeLeaf);
            curAnimationNodeLeaf = playerAnimationNodeLeaf;
            curAnimationNodeLeaf.Enter();
        }

        if (curAnimationNodeLeaf != null)
            curAnimationNodeLeaf.Update();
    }

    public void FixedUpdateNode()
    {
        if (curAnimationNodeLeaf != null)
            curAnimationNodeLeaf.FixedUpdate();
    }

    Animator IPlayerAnimationNode.animator { get => animator; set => animator = value; }
    public PlayerAnimationManager playerAnimationManager { get; set ; }
    public PlayerAnimationNodeLeaf curAnimationNodeLeaf { get ; set ; }
    public PlayerAnimationNodeSelector startSelectorAnimationNode { get; set; }

    public Move_Idle_StandPlayerAnimationNodeLeaf move_Idle_StandPlayerAnimationNodeLeaf { get; set; }
    public Sprint_PlayerAnimationNodeLeaf sprint_PlayerAnimationNodeLeaf { get; set; }
    
}
