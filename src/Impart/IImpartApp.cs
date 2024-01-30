namespace Impart;

public interface IImpartApp : IDisposable
{
    bool IsDisposed { get; }

    bool IsInitialized { get; }

    string? Arguments { get; }

    void Initialize();
}