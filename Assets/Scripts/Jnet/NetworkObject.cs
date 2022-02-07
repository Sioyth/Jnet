using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{

    /// <summary>
    /// Object Network ID;
    /// </summary>
    [SerializeField] private uint _id;
    [SerializeField] private uint _owner;
    [SerializeField] private bool _isOwner;

    public uint Id { get => _id;}
    public uint Owner { get => _owner;}

    public void SetOwner(uint id)
    {
        _owner = id;
    }
}
