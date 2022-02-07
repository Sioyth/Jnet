using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IDEA IS to inheric anyclass that is gonna be a PED from some other class inspired in RDR2 and GTA fivem netcode.
public class Player : MonoBehaviour
{
    private int _id;
    private string _username;
    [SerializeField] private float _speed = 5;

    public int Id { get => _id; set => _id = value; }

    // Update is called once per frame
    void Update()
    {
        // TODO: Change method to check if it's Owner
        if (NetworkManager.Instance.Id != GetComponent<NetworkObject>().Owner)
            return;

        if(Input.GetKey(KeyCode.W))
            transform.position += Vector3.forward * _speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * _speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            transform.position += Vector3.back * _speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * _speed * Time.deltaTime;

        
    }
}
