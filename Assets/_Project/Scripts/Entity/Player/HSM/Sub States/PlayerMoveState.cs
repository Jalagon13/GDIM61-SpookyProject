
namespace ACoolTeam
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

        public override void EnterState()
        {

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

        }

        public override void InitializeSubState()
        {

        }
    }
}
