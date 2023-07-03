using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlob : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private HealthBar _health_bar;
    void Awake()
    {
        _health_bar.Stalk(_enemy.gameObject);
        _enemy.AttachHealthBar(_health_bar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
