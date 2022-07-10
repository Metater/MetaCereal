namespace MetaCereal.CodeGen;

public class RustCodeGen
{
    #region TcpClientToServer
    private int tcpCSDataCount = 0;
    private readonly CodeBlock tcpCSDataEnum = new(
        "#[derive(Debug)]",
        "pub enum TcpCSData {"
    );
    private readonly CodeBlock tcpCSDataEnumImpl = new(
        "impl TcpCSData {"
    );
    private readonly CodeBlock tcpCSDataDecodeImpl = new(
        "impl bincode::Decode for TcpCSData {",
        "\tfn decode<D: bincode::de::Decoder>(decoder: &mut D) -> Result<Self, bincode::error::DecodeError> {",
        "\t\tlet variant_index = <u8 as bincode::Decode>::decode(decoder)?;",
        "\t\tmatch variant_index {"
    );

    public void HandleTcpCtoSSchema(string schemaName, string[] schema)
    {
        string pascalSchemaName = TextUtils.ToPascal(schemaName);
        string upperSchemaName = TextUtils.ToUpper(schemaName);

        tcpCSDataEnum.L($"\t{pascalSchemaName}({pascalSchemaName}Data),");

        tcpCSDataEnumImpl.L($"\tpub const {upperSchemaName}: u8 = {tcpCSDataCount};");

        tcpCSDataDecodeImpl.L($"\t\t\t{tcpCSDataCount} => Ok(TcpCSData::{pascalSchemaName}(<{pascalSchemaName}Data as bincode::Decode>::decode(decoder)?)),");

        tcpCSDataCount++;
    }

    public void GenerateTcpCtoS()
    {
        Console.WriteLine(tcpCSDataEnum.Compile(
            "}"
        ));

        Console.WriteLine(tcpCSDataEnumImpl.Compile(
            "}"
        ));

        Console.WriteLine(tcpCSDataDecodeImpl.Compile(
            "\t\t\tvariant => Err(bincode::error::DecodeError::UnexpectedVariant {",
            "\t\t\t\tfound: variant as u32,",
            "\t\t\t\ttype_name: \"TcpCSData\",",
            "\t\t\t\tallowed: bincode::error::AllowedEnumVariants::Allowed(&[]),",
            "\t\t\t})",
            "\t\t}",
            "\t}",
            "}"
        ));
    }
    #endregion TcpClientToServer

    public void Generate()
    {
        GenerateTcpCtoS();
    }
}
