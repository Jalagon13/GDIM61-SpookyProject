
namespace ACoolTeam
{
    public abstract class PlayerBaseState
    {
        private bool _isRootState = false;
        private PlayerStateMachine _ctx;
        private PlayerStateFactory _factory;
        private PlayerBaseState _currentSubState;
        private PlayerBaseState _currentSuperState;

        protected bool IsRootState { set { _isRootState = value; } }
        protected PlayerStateMachine Ctx { get { return _ctx; } }
        protected PlayerStateFactory Factory { get { return _factory; } }

        public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        {
            _ctx = currentContext;
            _factory = playerStateFactory;
        }

        #region Abstract Methods
        public abstract void EnterState(); // Code that plays when we enter a state
        public abstract void UpdateState(); // Code that plays during a state
        public abstract void ExitState(); // Code that plays  when we exit a state
        public abstract void CheckSwitchStates(); // Code that checks conditions to switch to a different state
        public abstract void InitializeSubState(); // Code that tells root state how to transition a substate

        #endregion

        #region Methods

        // plays every Update() call in PlayerStateMachine
        public void UpdateStates()
        {
            UpdateState();
            if (_currentSubState != null)
                _currentSubState.UpdateStates();
        }

        protected void SwitchState(PlayerBaseState newState)
        {
            // current state exits state
            ExitState();

            // new state enters state
            newState.EnterState();

            if (_isRootState)
            {
                // switch current state of context
                _ctx.CurrentState = newState;
            }
            else if (_currentSuperState != null)
            {
                // set the current super state sub state to the new state
                _currentSuperState.SetSubState(newState);
            }
        }
        protected void SetSuperState(PlayerBaseState newSuperState)
        {
            _currentSuperState = newSuperState;
        }
        protected void SetSubState(PlayerBaseState newSubState)
        {
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);
        }

        #endregion
    }
}
