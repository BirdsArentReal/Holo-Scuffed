using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _movement_speed = 8f;
    [SerializeField] private float _rotation_speed = 300f;
    [SerializeField] private int _health;
    private int _max_health = 5;
    [SerializeField] private GameObject _bullet_parent;
    [SerializeField] private GameObject _bullet_prefab;
    [SerializeField] private float _time_between_shots = 0.3f;
    private float _next_shot_ready;
    private Vector3 _rotation_axis = new Vector3(0, 0, -1);
    [SerializeField] private EnemyState _state;
    [SerializeField] private Vector3 _spawn_position;
    [SerializeField] private float _attack_range = 3f;
    [SerializeField] private float _detect_range = 5f;
    [SerializeField] private float _roam_range = 7f;
    [SerializeField] private Vector3 _roam_target;
    private Player _player;
    private Vector3 _relative_player_position;
    public HealthBar health_bar;
    

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag(Tags.PLAYER).GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("Enemy unable to find Player!");
        }
        
        SpawnManager spawn_manager = GameObject.FindWithTag(Tags.SPAWN_MANAGER).GetComponent<SpawnManager>();
        if (spawn_manager == null)
        {
            Debug.Log("Enemy could not find Spawn Manager!");
        }
        else
        {
            _bullet_parent = spawn_manager.GetBulletContainer();
        }
    }

    void Awake()
    {
        _spawn_position = this.transform.position;
        _state = EnemyState.IDLE;
        _next_shot_ready = Time.deltaTime;

        _health = _max_health;
    }

    void Update()
    {
        _relative_player_position = _player.transform.position - this.transform.position;
        float distance_from_player = Vector3.Magnitude(_relative_player_position);
        float distance_from_target = Vector3.Magnitude(this.transform.position - _roam_target);
        
        switch(_state)
        {
            case EnemyState.IDLE:
                _roam_target = GenerateRoamTarget();
                _state = EnemyState.MOVE;
                break;

            case EnemyState.MOVE:
                if (distance_from_target < 1f)
                {
                    _state = EnemyState.IDLE;
                }
                else if (distance_from_player < _detect_range)
                {
                    _state = EnemyState.CHASE;
                }
                else {
                    Move(_roam_target);
                }
                break;

            case EnemyState.CHASE:
                if (distance_from_player > _roam_range)
                {
                    _state = EnemyState.IDLE;
                }
                else if (distance_from_player < 0.9 * _attack_range)
                {
                    _state = EnemyState.SHOOT;
                }
                else
                {
                    Move(_player.transform.position);
                }
                break;

            case EnemyState.SHOOT:
                if (distance_from_player > _attack_range)
                {
                    _state = EnemyState.CHASE;
                }
                else
                {
                    FaceDirection(_relative_player_position);
                    Fire();
                }
                break;

            default: break;
        }      
        
    }

    private Vector3 GenerateRoamTarget()
    {
        float distance_from_spawn = _roam_range * Random.Range(0.7f, 1);
        float angle = Random.Range(-180, 180);
        return _spawn_position + distance_from_spawn * (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)));
    }
    
    private void Fire()
    {
        if (Time.time > _next_shot_ready)
        {
            // 0.5f is a magic number that spawns the bullet on the nose of the triangle
            GameObject bullet = Instantiate(_bullet_prefab, this.transform.position + this.transform.up * 0.5f, this.transform.rotation);
            bullet.transform.parent = _bullet_parent.transform;

            _next_shot_ready = Time.time + _time_between_shots;
        }
    }

    private void Move()
    {
        this.transform.Translate(this.transform.up * Time.deltaTime * _movement_speed, Space.World);
    }

    private void Move(Vector3 destination)
    {
        float angle = FaceDirection(destination - this.transform.position);
        if (Mathf.Abs(angle) < 2f)
        {
            Move();
        }
    }

    private float FaceDirection(Vector3 relative_position)
    {
        float angle = Vector3.SignedAngle(this.transform.up, relative_position, _rotation_axis);
        if (Mathf.Abs(angle) < 2f)
        {
            return 0;
        }
        this.transform.Rotate(_rotation_axis * Mathf.Sign(angle) * _rotation_speed * Time.deltaTime);
        return angle;
    }

    public void Damage(int damage = 1)
    {
        _health -= damage;
        health_bar.SetProportion(((float)_health) / _max_health);

        if (_health <= 0)
        {
            Die();
        }
    }

    // TODO
    // Drop powerups
    private void Die()
    {
        // randomly drop powerups? 
        SpawnManager.EnemyDeath();
        Destroy(this.transform.parent.gameObject);
    }

    public void AttachHealthBar(HealthBar health_bar)
    {
        this.health_bar = health_bar;
    }
}

enum EnemyState
{
    IDLE,
    MOVE,
    CHASE,
    SHOOT
}
