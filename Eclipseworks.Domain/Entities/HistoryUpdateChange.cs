namespace Eclipseworks.Domain.Entities;

public class HistoryUpdateChange : EntityBase
{
    public string Property { get; private set; }
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }

    public HistoryUpdateChange(
        string property, 
        string? oldValue = null, 
        string? newValue = null
    )
    {
        Property = property;
        OldValue = oldValue;
        NewValue = newValue;
    }
}