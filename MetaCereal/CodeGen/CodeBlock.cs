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

    public CodeBlock A(string text)
    {
        sb.Append(text);
        return this;
    }

    public CodeBlock L(params string[] lines)
    {
        foreach (string line in lines)
        {
            sb.AppendLine(line);
        }
        return this;
    }

    public CodeBlock Append(params CodeBlock[] blocks)
    {
        foreach (CodeBlock block in blocks)
        {
            L(block.Compile());
        }
        return this;
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