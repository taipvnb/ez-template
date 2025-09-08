namespace com.ez.engine.quest.core
{
    public class EmptyConditionBehaviour : QuestConditionBehaviour
    {
        protected override void OnActivate()
        {
            Passed = true;
            OnPassed?.Invoke();
        }

        protected override void OnDeactivate()
        {
        }
    }
}