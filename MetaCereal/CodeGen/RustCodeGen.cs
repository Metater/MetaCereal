namespace MetaCereal.CodeGen;

public class RustCodeGen : AbstractCodeGen
{
    private string? container;

    private int dataCount = 0;
    private CodeBlock dataEnum = new();
    private CodeBlock dataEnumImpl = new();
    private CodeBlock dataDecodeImpl = new();


    private readonly List<CodeBlock> tcpCSDataTypes = new();

    private readonly CodeBlock code = new();


    public override void SelectContainer(string container)
    {
        this.container = container;

        switch (container)
        {
            case "tcp-client-to-server":
                CommonInit("TcpCSData");
                break;
            case "tcp-server-to-client":
                break;
            case "udp-client-to-server":
                break;
            case "udp-server-to-client":
                break;
        }
    }

    private void CommonInit(string containerType)
    {
        dataEnum = new(
            "#[derive(Debug)]",
            $"pub enum {containerType} {{"
        );
        dataEnumImpl = new(
            $"impl {containerType} {{"
        );
        dataDecodeImpl = new(
            $"impl bincode::Decode for {containerType} {{",
            "\tfn decode<D: bincode::de::Decoder>(decoder: &mut D) -> Result<Self, bincode::error::DecodeError> {",
            "\t\tlet variant_index = <u8 as bincode::Decode>::decode(decoder)?;",
            "\t\tmatch variant_index {"
        );
    }

    public override void HandleSchema(bool isShared, string schemaName, string[] schema)
    {
        switch (container)
        {
            case "tcp-client-to-server":
                #region TcpCS
                #endregion TcpCS
                break;
            case "tcp-server-to-client":
                #region TcpSC
                #endregion TcpSC
                break;
            case "udp-client-to-server":
                #region UdpCS
                #endregion UdpCS
                break;
            case "udp-server-to-client":
                #region UdpSC
                #endregion UdpSC
                break;
        }

        string pascalSchemaName = TextUtils.ToPascal(schemaName);
        string upperSchemaName = TextUtils.ToUpper(schemaName);

        dataEnum.L($"\t{pascalSchemaName}({pascalSchemaName}Data),");

        dataEnumImpl.L($"\tpub const {upperSchemaName}: u8 = {dataCount};");

        dataDecodeImpl.L($"\t\t\tSelf::{upperSchemaName} => Ok(TcpCSData::{pascalSchemaName}(<{pascalSchemaName}Data as bincode::Decode>::decode(decoder)?)),");

        HandleTcpCtoSTopLevelSchema(schemaName, schema);

        dataCount++;
    }

    private void HandleTcpCtoSTopLevelSchema(string schemaName, string[] schema)
    {
        CodeBlock code = new(
            "#[derive(bincode::Encode, bincode::Decode, Debug)]"
        );
        code.L($"pub struct {TextUtils.ToPascal(schemaName)}Data {{");

        List<string> block = new();
        foreach (string line in schema)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            int leadingTabs = TextUtils.CountLeadingTabs(line);
            block.Add(line);
            if (leadingTabs == 0)
            {
                if (block.Count != 0)
                {
                    HandleTcpCtoSBlock(code, schemaName, block);
                    block.Clear();
                }
            }
        }
        if (block.Count != 0)
        {
            HandleTcpCtoSBlock(code, schemaName, block);
            block.Clear();
        }

        tcpCSDataTypes.Add(code.L(
            "}"
        ));
    }

    private void HandleTcpCtoSBlock(CodeBlock code, string schemaName, List<string> block)
    {
        foreach (string line in block)
        {
            Console.WriteLine(line);
            string[] split = line.Split(' ');
            code.L($"\tpub {TextUtils.ToSnake(split[0])}: {split[1]},");
        }
        Console.WriteLine();
    }

    public override void Generate()
    {
        if (container is null)
        {
            return;
        }

        switch (container)
        {
            case "tcp-client-to-server":
                CommonGenerate("TcpCSData");
                GenerateDataDecodeImpl("TcpCSData");
                break;
            case "tcp-server-to-client":

                break;
            case "udp-client-to-server":

                break;
            case "udp-server-to-client":

                break;
        }
    }

    private void CommonGenerate(string containerType)
    {
        code.Append(dataEnum.L(
            "}"
        ));
        code.Append(dataEnumImpl.L(
            "}"
        ));
    }

    private void GenerateDataDecodeImpl(string containerType)
    {
        code.Append(dataDecodeImpl.L(
            "\t\t\tvariant => Err(bincode::error::DecodeError::UnexpectedVariant {",
            "\t\t\t\tfound: variant as u32,",
            $"\t\t\t\ttype_name: \"{containerType}\",",
            "\t\t\t\tallowed: bincode::error::AllowedEnumVariants::Allowed(&[]),",
            "\t\t\t})",
            "\t\t}",
            "\t}",
            "}"
        ));
    }

    public override void Write()
    {
        CodeBlock dataTypes = new CodeBlock().Append(tcpCSDataTypes.ToArray());
        code.Append(dataTypes);
        Console.WriteLine(code.Compile());
    }
}
