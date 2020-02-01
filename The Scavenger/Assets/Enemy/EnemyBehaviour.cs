﻿using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class EnemyBehaviour : MonoBehaviour, IConvertGameObjectToEntity
{
    [Header("Movement")]
    public float speed = 5f;
    [Header("Life Settings")]
    public int enemyHealth = 1;

    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!GameManager.IsPlayerDead())
        {
            Vector3 heading = GameManager.PlayerPosition - transform.position;
            heading.y = 0f;
            transform.rotation = Quaternion.LookRotation(heading);
        }

        Vector3 movement = transform.forward * speed * Time.deltaTime;
        rigidBody.MovePosition(transform.position + movement);
    }

    public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
    {
        manager.AddComponent(entity, typeof(EnemyTag));
        manager.AddComponent(entity, typeof(MoveForward));

        MovementData moveSpeed = new MovementData { speed = speed };
        manager.AddComponentData(entity, moveSpeed);

        Health health = new Health { Value = enemyHealth };
        manager.AddComponentData(entity, health);
    }
}