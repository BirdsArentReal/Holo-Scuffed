using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _max_health = 10f;
    [SerializeField] private float _health;
    [SerializeField] private float _regeneration_rate = 1f;
    [SerializeField] private HealthBar _health_bar;
    private SpawnManager _spawn_manager;
    


    // Start is called before the first frame update
    void Start()
    {
        _health = _max_health;
        _spawn_manager = GameObject.FindWithTag(Tags.SPAWN_MANAGER).GetComponent<SpawnManager>();
        if (_spawn_manager == null)
        {
            Debug.Log("Player could not find Spawn Manager!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Regenerate();
        _health_bar.SetProportion(_health/_max_health);
    }

    private void Move()
    {
        // up and right positive
        float horizontal_input = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
        float vertical_input = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S)? -1 : 0);        

        Vector2 direction = new Vector2(horizontal_input, vertical_input);
        this.transform.Translate(direction * Time.deltaTime * _speed);

    }

    public void Damage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Regenerate()
    {
        if (_health < _max_health)
        {
            _health +=  _regeneration_rate * Time.deltaTime;
        }
        
    }

    private void Die()
    {
        Destroy(_spawn_manager.gameObject);
        _max_health = 0;
        _speed = 0;
        
    }


}
