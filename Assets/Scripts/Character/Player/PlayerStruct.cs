using System;
using Unity.Netcode;

public struct PlayerStruct : IEquatable<PlayerStruct>, INetworkSerializable
{
    public ulong clientID;

    public bool Equals(PlayerStruct other)
    {
        return clientID == other.clientID;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientID);
    }
}
