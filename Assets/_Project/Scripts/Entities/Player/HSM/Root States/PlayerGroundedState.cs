using UnityEngine;

namespace ACoolTeam
{
    public class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
        {
            IsRootState = true;
            InitializeSubState();
        }

        public override void EnterState()
        {
            Debug.Log("Entered Grounded State");
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void ExitState()
        {
            
        }

        public override void CheckSwitchStates()
        {
            if (!Ctx.IsJumping && Ctx.IsJumpPressed)
                SwitchState(Factory.Jump());

        }

        public override void InitializeSubState()
        {
            if (!Ctx.IsMovementPressed)
                SetSubState(Factory.Idle());
            else
                SetSubState(Factory.Move());
        }
    }
}