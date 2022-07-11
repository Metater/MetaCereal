namespace MetaCereal.CodeGen;

public abstract class AbstractCodeGen
{
    public abstract void SelectContainer(string container);
    public abstract void HandleSchema(bool isShared, string schemaName, string[] schema);
    public abstract void Generate();
}