namespace EmployeeManagementSystem.Domain.Base;

public abstract class BaseEntity
{
    public int       Id        { get; protected set; }
    public DateTime  CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    protected void SetId(int id)
    {
        if (Id != 0)
            throw new InvalidOperationException("Entity ID cannot be changed once set.");
        Id = id;
    }

    protected void SetCreatedAt(DateTime createdAt)
        => CreatedAt = createdAt;

    protected void SetUpdatedAt(DateTime? updatedAt)
        => UpdatedAt = updatedAt;
}
