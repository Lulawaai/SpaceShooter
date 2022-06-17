using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
	}
}
