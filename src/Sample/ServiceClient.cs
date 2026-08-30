namespace Sample;

public sealed class ServiceClient
{
    [RequiresPermission("s3:ListBucket")]
    public void ListThis()
    {
        Console.WriteLine("ServiceClient.ListThis");
    }

    [RequiresPermission("s3:GetObject")]
    public void GetThis()
    {
        Console.WriteLine("ServiceClient.GetThis");
    }

    [RequiresPermission("s3:PutObject")]
    public void PutThis()
    {
        Console.WriteLine("ServiceClient.PutThis");
    }
}
