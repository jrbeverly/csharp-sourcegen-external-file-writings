using Sample;

var client = new ServiceClient();

client.ListThis();
client.GetThis();

if (args.Contains("--write"))
{
    client.PutThis();
}
