using F122_UDP;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

public class UDPListener
{
    UdpClient Client = new UdpClient(20777); //Connectionport
    byte[]? received;

    public void StartListener()
    {
           Client.BeginReceive(new AsyncCallback(recv), null);
    }

    

    public T ByteArrayToStruct<T>(byte[] bytes)
    {
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        T? stuff;
        try
        {
            stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
        }
        finally
        {
            handle.Free();
        }
        return stuff;
    }


    unsafe void recv(IAsyncResult res)
    {
        var RemoteIP = new IPEndPoint(IPAddress.Any, 60240);
        received = Client.EndReceive(res, ref RemoteIP);
        
        F122_Structs.PacketHeader packetheader = ByteArrayToStruct<F122_Structs.PacketHeader>(received);

        if (packetheader.m_packetId == F122_Structs.PacketType.Session)
        {
            F122_Structs.PacketSessionData telemetry = ByteArrayToStruct<F122_Structs.PacketSessionData>(received);
            ReadOnlySpan<byte> remaining = received;
            remaining = remaining.Slice(43, 105);
            var tel = MemoryMarshal.Cast<byte, F122_Structs.MarshalZone>(remaining);
            for (int i = 0; i < telemetry.m_numMarshalZones; i++)
            { 
                Console.WriteLine($"Marshal zone {i}: {tel[i].m_zoneStart} flag: {tel[i].m_zoneFlag}");
            }
        }
        Client.BeginReceive(new AsyncCallback(recv), null);
    }

}

public class main
{
    public static void Main()
    {
        UDPListener listener = new UDPListener();
        listener.StartListener();
        Console.ReadLine();
    }
}