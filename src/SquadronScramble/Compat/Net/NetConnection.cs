using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace SquadronScramble.Net
{
    /// <summary>
    /// A TCP connection carrying length-prefixed messages. Incoming messages are read on a
    /// background thread and queued; the game thread drains them with <see cref="TryReceive"/>.
    /// </summary>
    internal sealed class NetConnection : IDisposable
    {
        private const int MaxMessageSize = 16 * 1024 * 1024;

        private readonly TcpClient client;
        private readonly NetworkStream stream;
        private readonly ConcurrentQueue<byte[]> incoming = new ConcurrentQueue<byte[]>();
        private readonly object sendLock = new object();
        private volatile bool closed;

        public NetConnection(TcpClient client)
        {
            this.client = client;
            client.NoDelay = true;
            stream = client.GetStream();
            var thread = new Thread(ReadLoop) { IsBackground = true, Name = "NetConnection reader" };
            thread.Start();
        }

        public bool IsClosed => closed;

        public string CloseReason { get; private set; }

        public string RemoteAddress
        {
            get
            {
                try { return client.Client.RemoteEndPoint?.ToString() ?? "?"; }
                catch (Exception) { return "?"; }
            }
        }

        public bool TryReceive(out byte[] message) => incoming.TryDequeue(out message);

        public void Send(byte[] message)
        {
            if (closed)
                return;
            try
            {
                var header = BitConverter.GetBytes(message.Length);
                lock (sendLock)
                {
                    stream.Write(header, 0, 4);
                    stream.Write(message, 0, message.Length);
                }
            }
            catch (Exception ex)
            {
                Close(ex.Message);
            }
        }

        private void ReadLoop()
        {
            try
            {
                var header = new byte[4];
                while (!closed)
                {
                    ReadExactly(header, 4);
                    int length = BitConverter.ToInt32(header, 0);
                    if (length <= 0 || length > MaxMessageSize)
                        throw new InvalidDataException("Bad message length.");
                    var body = new byte[length];
                    ReadExactly(body, length);
                    incoming.Enqueue(body);
                }
            }
            catch (Exception ex)
            {
                Close(ex is EndOfStreamException ? "Connection closed." : ex.Message);
            }
        }

        private void ReadExactly(byte[] buffer, int count)
        {
            int read = 0;
            while (read < count)
            {
                int n = stream.Read(buffer, read, count - read);
                if (n <= 0)
                    throw new EndOfStreamException();
                read += n;
            }
        }

        public void Close(string reason)
        {
            if (closed)
                return;
            CloseReason ??= reason;
            closed = true;
            try { client.Close(); } catch (Exception) { }
        }

        public void Dispose() => Close("Disposed.");
    }
}
