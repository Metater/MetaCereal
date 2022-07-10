namespace MetaCereal;

public class SchemaScanner
{
    private readonly CSharpCodeGen csGen = new();
    private readonly RustCodeGen rsGen = new();

    public void Scan()
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
                    //Console.WriteLine(selectedContainer);
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
                        if (firstChar == '$')
                        {
                            schemaName = schemaName[1..];
                            schema = IOUtils.ReadSchema($"data/shared/{schemaName}");
                        }
                        else
                        {
                            schema = IOUtils.ReadSchema($"data/{selectedContainer}/{schemaName}");
                        }

                        //Console.WriteLine($"\t{schemaName}");
                        HandleSchema(selectedContainer, schemaName, schema);
                    }
                    break;
            }
        }
    }
    
    private void HandleSchema(string selectedContainer, string schemaName, string[] schema)
    {
        foreach (var item in schema)
        {
            //Console.WriteLine($"\t\t{item}");
        }

        switch (selectedContainer)
        {
            case "tcp-client-to-server":
                rsGen.HandleTcpCtoSSchema(schemaName, schema);
                break;
            case "tcp-server-to-client":
                break;
            case "udp-client-to-server":
                break;
            case "udp-server-to-client":
                break;
        }
    }

    public void Generate()
    {
        rsGen.Generate();
    }
}