using UnityEngine;
using System.Collections;

public class TempSprite {

    private Animation2D _anim;      //Reference to the the temporary sprites animation
    private GameObject _obj;
    private SpriteRenderer _renderer;
    private Timer _animTimer;
    private Vector2 velocity;

    public bool finished = false;

    //Constructor
    public TempSprite(Animation2D anim, Vector2 pos) {
        _obj = new GameObject();
        _obj.transform.position = pos;

        _obj.AddComponent<SpriteRenderer>();
        _renderer = _obj.GetComponent<SpriteRenderer>();

        _anim = anim;
        _animTimer = new Timer();
        _animTimer.StartTimer();

        velocity = Vector2.zero;
    }

    public TempSprite(Animation2D anim, Vector2 pos, Vector2 scale)
    {
        _obj = new GameObject();
        _obj.transform.position = pos;
        _obj.transform.localScale = scale;

        _obj.AddComponent<SpriteRenderer>();
        _renderer = _obj.GetComponent<SpriteRenderer>();

        _anim = anim;
        _animTimer = new Timer();
        _animTimer.StartTimer();

        velocity = Vector2.zero;
    }

    public TempSprite( Animation2D anim, Vector2 pos, Vector2 scale, string sortingLayer, int sortingOrder)
    {
        _obj = new GameObject();
        _obj.transform.position = pos;
        _obj.transform.localScale = scale;

        _obj.AddComponent<SpriteRenderer>();
        _renderer = _obj.GetComponent<SpriteRenderer>();
        _renderer.sortingLayerName = sortingLayer;
        _renderer.sortingOrder = sortingOrder;

        _anim = anim;
        _animTimer = new Timer();
        _animTimer.StartTimer();

        velocity = Vector2.zero;
    }

    public TempSprite(Animation2D anim, Vector2 pos, Vector2 scale, float rotation, string sortingLayer, int sortingOrder)
    {
        _obj = new GameObject();
        _obj.transform.position = pos;
        _obj.transform.localScale = scale;
        _obj.transform.rotation = Quaternion.Euler(0, 0, rotation);

        _obj.AddComponent<SpriteRenderer>();
        _renderer = _obj.GetComponent<SpriteRenderer>();
        _renderer.sortingLayerName = sortingLayer;
        _renderer.sortingOrder = sortingOrder;

        _anim = anim;
        _animTimer = new Timer();
        _animTimer.StartTimer();

        velocity = Vector2.zero;
    }

    public TempSprite(Animation2D anim, Vector2 pos, Vector2 scale, Vector2 velocity, float rotation, string sortingLayer, int sortingOrder)
    {
        _obj = new GameObject();
        _obj.transform.position = pos;
        _obj.transform.localScale = scale;
        _obj.transform.rotation = Quaternion.Euler(0, 0, rotation);

        _obj.AddComponent<SpriteRenderer>();
        _renderer = _obj.GetComponent<SpriteRenderer>();
        _renderer.sortingLayerName = sortingLayer;
        _renderer.sortingOrder = sortingOrder;

        _anim = anim;
        _animTimer = new Timer();
        _animTimer.StartTimer();

        this.velocity = velocity;
    }

    public TempSprite(Animation2D anim, Vector2 pos, Vector2 scale, Vector2 velocity, float rotation, string sortingLayer, int sortingOrder, Color color)
    {
        _obj = new GameObject();
        _obj.transform.position = pos;
        _obj.transform.localScale = scale;
        _obj.transform.rotation = Quaternion.Euler(0, 0, rotation);

        _obj.AddComponent<SpriteRenderer>();
        _renderer = _obj.GetComponent<SpriteRenderer>();
        _renderer.sortingLayerName = sortingLayer;
        _renderer.sortingOrder = sortingOrder;
        _renderer.color = color;

        _anim = anim;
        _animTimer = new Timer();
        _animTimer.StartTimer();

        this.velocity = velocity;
    }

    public void Update() {
        if (_renderer != null)
        {
            _anim.Play(_animTimer, _renderer, 1);
            _renderer.transform.position += new Vector3(velocity.x, velocity.y, 0.0f);
            finished = _anim.finished;
        }
    }

    public void Delete(){
        GameObject.Destroy(_obj);
    }

    public void Delay(float delay) {
        _anim.Pause(delay);
    }
}
