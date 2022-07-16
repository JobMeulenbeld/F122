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
    byte[] received;

    public void StartListener()
    {
           Client.BeginReceive(new AsyncCallback(recv), null);
    }

    


    void recv(IAsyncResult res)
    {
        var RemoteIP = new IPEndPoint(IPAddress.Any, 60240);
        received = Client.EndReceive(res, ref RemoteIP);

        ReadOnlySpan<byte> remaining = received;
        var header = MemoryMarshal.Cast<byte, F122_Stucts.PacketHeader>(remaining)[0];

        if (header.m_packetId == F122_Stucts.PacketType.Session)
        {
            
                Console.WriteLine($"Current id = {header.m_packetId}");
                var telemetry = MemoryMarshal.Cast<byte, F122_Stucts.PacketSessionData>(remaining);
                Console.WriteLine($"{telemetry[0].m_marshalZones[0].m_zoneFlag}");
            
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