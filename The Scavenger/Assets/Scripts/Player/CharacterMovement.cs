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

	[Header("Life Settings")]
	public float playerHealth = 1f;

	[Header("Bullets")]
	public GameObject bulletPrefab;
	public Transform gunBarrel;

	EntityManager entityManager;
	private Entity bulletEntityPrefab;

	private BlobAssetStore blobAssetStore;

	void Awake()
	{
		blobAssetStore = new BlobAssetStore();
		_controls = new Controls();
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore));
	}

	private void OnDestroy()
	{
		blobAssetStore.Dispose();
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
		_controls.Character.FireRelease.performed += FireRelease_performed;
		_controls.Character.Enable();
	}

	private void DisableCharacterControls()
	{
		_controls.Character.Move.performed -= Move_performed;
		_controls.Character.Fire.performed -= Fire_performed;
		_controls.Character.FireRelease.performed -= FireRelease_performed;
		_controls.Character.Disable();
	}

	private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		_inputVector = obj.ReadValue<Vector2>();
	}

	private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		StartCoroutine(nameof(FireBullets));
	}

	private void FireRelease_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		StopCoroutine(nameof(FireBullets));
	}

	IEnumerator FireBullets()
	{
		while (true)
		{
			Entity bullet = entityManager.Instantiate(bulletEntityPrefab);
			entityManager.SetComponentData(bullet, new Translation { Value = gunBarrel.position });
			entityManager.SetComponentData(bullet, new Rotation { Value = gunBarrel.rotation });
			
			yield return new WaitForSeconds(.15f);
		}
	}

	private void Update()
	{
		transform.position += Time.deltaTime * new Vector3(_inputVector.x, _inputVector.y, 0) * _movementSpeed;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Enemy"))
			return;

		playerHealth--;

		if (playerHealth <= 0)
			GameManager.PlayerDied();
	}
}
