namespace DataTransferObjects.Interfaces
{
    public interface IListDTO<T>
    {
        IEnumerable<T> List { get; }
        int RecordCount { get; }
        IValidate Validate { get; }
    }
}