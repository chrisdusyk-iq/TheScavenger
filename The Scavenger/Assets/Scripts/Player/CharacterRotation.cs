using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
	private Controls _controls;
	private Vector2 _aimPoint;

	void Awake()
	{
		_controls = new Controls();
	}

	void OnEnable()
	{
		_controls.Character.Aim.performed += Aim_performed;
		_controls.Character.Aim.Enable();
	}

	void OnDisable()
	{
		_controls.Character.Aim.performed -= Aim_performed;
		_controls.Character.Aim.Disable();
	}

	private void Aim_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		_aimPoint = obj.ReadValue<Vector2>();
	}

	void Update()
	{
		var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
		var angle = Mathf.Atan2(screenPosition.y - _aimPoint.y, screenPosition.x - _aimPoint.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
