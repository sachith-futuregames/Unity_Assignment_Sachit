using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player_Controller : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    //Variables For Movement
    private float _gravity = -9.8f;
    private Vector3 _velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        _velocity.y += _gravity;
        characterController.Move(_velocity * Time.deltaTime);
    }

    
}
