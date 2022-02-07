using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignID
{
   private List<uint> _ids = new List<uint>();

    public uint Assign()
    {
        return 0;
    }

    public void RemoveID(uint id)
    {
        _ids.Remove(id);
    }
}
