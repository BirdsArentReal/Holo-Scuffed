using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float _shield_speed = 300f;
    [SerializeField] private Player _player;
    private Vector3 _rotation_axis = new Vector3(0, 0, -1);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveShield();
    }

    private void MoveShield()
    {
        float horizontal_input = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) + (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0);
        float vertical_input = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) + (Input.GetKey(KeyCode.DownArrow) ? -1 : 0);
        bool dash = Input.GetKey(KeyCode.RightShift);

        if (dash && ((horizontal_input != 0) || (vertical_input != 0))){
            this.transform.position = new Vector3(horizontal_input, vertical_input) + _player.transform.position;
        }
        this.transform.RotateAround(_player.transform.position, _rotation_axis, horizontal_input * Time.deltaTime * _shield_speed); 
    }
}
