public interface ISnapshotHandler
{
    void ApplySnapshot(string value);
    string TakeSnapshot();
}