using EmployeeManagementSystem.Domain.Base;

namespace EmployeeManagementSystem.Domain.Entities;

public class Position : BaseEntity
{
    public string                Name      { get; private set; } = string.Empty;
    public int?                  ParentId  { get; private set; }
    public Position?             Parent    { get; private set; }
    public ICollection<Position> Children  { get; private set; } = new List<Position>();
    public ICollection<Employee> Employees { get; private set; } = new List<Employee>();

    private Position() { }

    public static Position Create(string name, int? parentId = null)
    {
        ValidateName(name);

        var position = new Position
        {
            Name = name,
            ParentId = parentId,
            CreatedAt = DateTime.UtcNow
        };

        return position;
    }

    public void UpdateName(string name)
    {
        ValidateName(name);
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeParent(int? parentId)
    {
        if (parentId.HasValue && parentId.Value == Id)
            throw new InvalidOperationException("A position cannot be its own parent.");

        ParentId = parentId;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool HasChildren()
        => Children.Any();

    public bool HasEmployees()
        => Employees.Any();

    public bool CanBeDeleted()
        => !HasChildren() && !HasEmployees();

    public void AddChild(Position child)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child));

        if (child.Id == Id)
            throw new InvalidOperationException("A position cannot be its own child.");

        if (Children == null)
            Children = new List<Position>();

        child.ChangeParent(Id);
        Children.Add(child);
    }

    public void RemoveChild(Position child)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child));

        if (Children == null || !Children.Contains(child))
            throw new InvalidOperationException("The specified position is not a child of this position.");

        Children.Remove(child);
        child.ChangeParent(null);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Position name is required.", nameof(name));

        if (name.Length > 200)
            throw new ArgumentException("Position name cannot exceed 200 characters.", nameof(name));
    }
}
