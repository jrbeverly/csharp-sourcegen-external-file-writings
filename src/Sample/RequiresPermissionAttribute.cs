namespace Sample;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RequiresPermissionAttribute(string permission) : Attribute
{
    public string Permission { get; } = permission;
}
