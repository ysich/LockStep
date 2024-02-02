namespace LockStep
{
    public abstract class LockStepUpdateSystemBase
    {
        protected LockStepSystem m_system;
        public LockStepUpdateSystemBase(LockStepSystem lockStepSystem)
        {
            m_system = lockStepSystem;
        }
        public abstract void Update();
    }
}