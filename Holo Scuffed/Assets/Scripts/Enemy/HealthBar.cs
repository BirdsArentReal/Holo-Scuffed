using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject senpai;
    private float upwards_offset = 1f;
    private Vector3 _initial_scale;
    private float _initial_length;
    public float proportion = 1f;
    private Material _color;

    // Start is called before the first frame update
    void Awake()
    {
        _initial_scale = this.transform.localScale;
        _initial_length = 1 * _initial_scale.x;
        this._color = this.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        ScaleAndColor();
    }

    void ScaleAndColor()
    {
        // Combined Scale and Color because alot of the possible
        // method names are already taken by the given Color class.
        this.transform.localScale = new Vector3(_initial_scale.x * proportion, _initial_scale.y, _initial_scale.z);

        float _space_on_the_left = _initial_length * (1 - proportion) * 0.5f;

        this.transform.position = new Vector3(senpai.transform.position.x + (-1 * _space_on_the_left), 
                                            senpai.transform.position.y + upwards_offset, 
                                            senpai.transform.position.z);

        string COLOR_PROPERTY = "_Color";
        if (this.proportion > 0.7)
        {
            _color.SetColor(COLOR_PROPERTY, Color.green);
        }
        else if (this.proportion > 0.3)
        {
            _color.SetColor(COLOR_PROPERTY, Color.yellow);
        }
        else
        {
            _color.SetColor(COLOR_PROPERTY, Color.red);
        }


    }

    public void Stalk(GameObject parent)
    {
        this.senpai = parent;
    }

    public void SetProportion(float proportion)
    {
        this.proportion = proportion;
    }
}
