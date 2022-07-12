namespace MetaCereal;

public class SchemaHandler
{
    private readonly CSharpCodeGen csGen = new();
    private readonly RustCodeGen rsGen = new();

    public void Handle()
    {
        var main = IOUtils.ReadSchema("main");
        int cursor = 0;
        string? selectedContainer = null;

        while (true)
        {
            if (cursor >= main!.Length)
            {
                break;
            }

            string line = main![cursor++];

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            switch (line)
            {
                case "tcp-client-to-server" or
                    "tcp-server-to-client" or
                    "udp-client-to-server" or
                    "udp-server-to-client":
                    selectedContainer = line;
                    rsGen.Generate();
                    rsGen.SelectContainer(selectedContainer);
                    break;
                default:
                    if (selectedContainer is null)
                    {
                        Console.WriteLine("error: no selected container");
                    }
                    else
                    {
                        string schemaName = line.Trim();
                        char firstChar = schemaName[0];
                        string[] schema;
                        bool isShared = false;
                        if (firstChar == '$')
                        {
                            schemaName = schemaName[1..];
                            schema = IOUtils.ReadSchema($"data/shared/{schemaName}");
                            isShared = true;
                        }
                        else
                        {
                            schema = IOUtils.ReadSchema($"data/{selectedContainer}/{schemaName}");
                        }

                        rsGen.HandleSchema(isShared, schemaName, schema);
                    }
                    break;
            }
        }
    }

    public void Write()
    {
        rsGen.Write();
    }
}