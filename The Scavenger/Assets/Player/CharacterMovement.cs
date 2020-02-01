using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	private Controls _controls;
	private Vector2 _inputVector;
	private Vector3 _movementVector;
	[SerializeField]
	private float _movementSpeed = 20.0f;

	[Header("Bullets")]
	public GameObject bulletPrefab;

	EntityManager entityManager;
	private Entity bulletEntityPrefab;

	void Awake()
	{
		_controls = new Controls();
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, World.DefaultGameObjectInjectionWorld);
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
		_inputVector = obj.ReadValue<Vector2>();
	}

	private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		Debug.Log("Bullet fired");
		Entity bullet = entityManager.Instantiate(bulletEntityPrefab);
		entityManager.SetComponentData(bullet, new Translation { Value = GameManager.PlayerPosition });
	}

	private void Update()
	{
		transform.position += Time.deltaTime * new Vector3(_inputVector.x, _inputVector.y, 0) * _movementSpeed;

	}
}
