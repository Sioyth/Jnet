using System;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

public static class JnetExtensions
{
    #region Marshalling 
    public static T ToStruct<T>(this byte[] data) where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));
        System.IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(data, 0, ptr, size);

        T t = Marshal.PtrToStructure<T>(ptr);
        Marshal.FreeHGlobal(ptr);
        return t;
    }

    public static byte[] ToByteArray(this object obj)
    {
        int size = Marshal.SizeOf(obj);
        byte[] buf = new byte[size];

        System.IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(obj, ptr, true);
        Marshal.Copy(buf, 0, ptr, size);

        Marshal.FreeHGlobal(ptr);
        return buf;

    }
    #endregion

    #region Json Serialization
    public static byte[] ToJsonBinary(this object obj)
    {
        return Encoding.ASCII.GetBytes(JsonUtility.ToJson(obj));
    }

    public static T FromJsonBinary<T>(this byte[] data)
    {
        return JsonUtility.FromJson<T>(Encoding.ASCII.GetString(data));
    }
    #endregion
}


[Serializable]
[StructLayout(LayoutKind.Sequential,Pack = 1)]
public struct Packet
{
    public string msg;
}

