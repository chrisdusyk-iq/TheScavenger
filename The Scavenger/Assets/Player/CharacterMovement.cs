﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	private Controls _controls;
	private Vector2 _playerVelocity;
	EntityManager entityManager;
	Entity bulletPrefab;

	void Awake()
	{
		_controls = new Controls();
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
		_playerVelocity = obj.ReadValue<Vector2>();
		Debug.Log(_playerVelocity);
	}

	private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		Entity bullet = entityManager.Instantiate(bulletPrefab);
		entityManager.SetComponentData(bullet, new Translation { Value = GameManager.PlayerPosition });
	}

	private void Update()
	{

	}

}
