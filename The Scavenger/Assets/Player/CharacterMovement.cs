﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Controls _controls;
    private Vector2 _inputVector;
    private Vector3 _movementVector;
    [SerializeField]
    private float _movementSpeed = 3.0f;

    void Awake()
    {
        _controls = new Controls();
        _movementVector = Vector3.zero;
    }

    private void OnEnable()
    {
        EnableCharacterControls();
    }

    private void OnDisable()
    {
        DisableCharacterControls();
    }

    private void EnableCharacterControls()
    {
        _controls.Character.Move.performed += Move_performed;
        _controls.Character.Fire.performed += Fire_performed;
        _controls.Character.Enable();

    }
    private void DisableCharacterControls()
    {
        _controls.Character.Move.performed -= Move_performed;
        _controls.Character.Fire.performed -= Fire_performed;
        _controls.Character.Disable();
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _inputVector = obj.ReadValue<Vector2>().normalized;
    }

    private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Fire");
    }

    private void Update()
    {

        transform.position += Time.deltaTime * new Vector3(_inputVector.x, 0, _inputVector.y);
    }

}
