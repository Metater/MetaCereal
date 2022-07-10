namespace MetaCereal.CodeGen;

public class CodeBlock
{
    private readonly StringBuilder sb;

    public CodeBlock()
    {
        sb = new();
    }

    public CodeBlock(params string[] lines)
    {
        sb = new();
        foreach (string line in lines)
        {
            sb.AppendLine(line);
        }
    }

    public void A(string text)
    {
        sb.Append(text);
    }

    public void L(string line)
    {
        sb.AppendLine(line);
    }

    public void Append(CodeBlock other)
    {
        L(other.Compile());
    }

    public string Compile()
    {
        return sb.ToString();
    }

    public string Compile(params string[] lines)
    {
        foreach (string line in lines)
        {
            sb.AppendLine(line);
        }
        return Compile();
    }
}