using Services.Condition.Data;

namespace Services.Condition
{
    public interface IConditionService
    {
        bool IsMet(IConditionData conditionData);
        IConditionData ConvertToConditionData(string value);
    }
}