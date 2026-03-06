using System.Net.Sockets;

public sealed class SQTcpClient : IDisposable
{
    private readonly string _ip;
    private readonly int _port;
    private TcpClient? _sqTcp;

    public SQTcpClient(string ip, int port = 51325)
    {
        _ip = ip;
        _port = port;
    }

    public async Task ConnectAsync(CancellationToken ct = default)
    {
        _sqTcp = new TcpClient();

        // Connect to SQ (MIDI over TCP/IP uses port 51325 per A&H documentation)
        await _sqTcp.ConnectAsync(_ip, _port, ct);
    }

    /// <summary>
    /// Send raw MIDI bytes over the TCP connection.
    /// IMPORTANT: The exact message bytes/ordering must match the SQ MIDI Protocol (NRPN/CC sequences, etc.).
    /// </summary>
    public async Task SendMidiAsync(byte[] midiBytes, CancellationToken ct = default)
    {
        // ToDo: add cancelation token support to this method and all async methods in this class.
        // using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        if (GetStream() is null) throw new InvalidOperationException("Not connected.");

        await GetStream().WriteAsync(midiBytes, 0, midiBytes.Length, ct);
        await GetStream().FlushAsync(ct);
    }

    public NetworkStream GetStream()
    {
        return _sqTcp.GetStream();
    }
    

    public void Dispose()
    {
        try { GetStream()?.Dispose(); } catch { /* ignore */ }
        try { _sqTcp?.Close(); } catch { /* ignore */ }
        _sqTcp = null;
    }
}