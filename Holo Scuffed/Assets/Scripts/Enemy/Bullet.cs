using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _time_to_live = 0.5f;
    [SerializeField] private BulletState mode = BulletState.HIT_PLAYER;
    [SerializeField] private float _speed = 30f;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision with " + other.tag);
        switch(other.tag)
        {
            case Tags.BULLET:
                // do nothing
                break;

            case Tags.ENEMY:
                if (mode == BulletState.HIT_ENEMY)
                {
                    DamageEnemy(other);
                }
                break;

            case Tags.PLAYER:
                if (mode == BulletState.HIT_PLAYER)
                {
                    DamagePlayer(other);
                }
                break;
            case Tags.SHIELD:
                Deflect();
                mode = BulletState.HIT_ENEMY;
                break;
            case Tags.OBSTACLE:
                Destroy(this.gameObject);
                break;
            default: break;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        _time_to_live -= Time.deltaTime;
        if (_time_to_live < 0)
        {
            Destroy(this.gameObject);
        }
        Move();
    }

    private void Move()
    {
        this.transform.Translate(this.transform.up * _speed * Time.deltaTime, Space.World);
    }

    private void Deflect()
    {
        this.transform.Rotate(new Vector3(0, 0, 180), Space.Self);
    }

    void DamageEnemy(Collider2D other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Damage(1);
        }
        Destroy(this.gameObject);
    }
    
    void DamagePlayer(Collider2D other)
    {
        Player player = other.transform.GetComponent<Player>();
        if (player != null)
        {
            player.Damage(1);
        }
        Destroy(this.gameObject);
    }

}

enum BulletState
{
    HIT_ENEMY,
    HIT_PLAYER
}