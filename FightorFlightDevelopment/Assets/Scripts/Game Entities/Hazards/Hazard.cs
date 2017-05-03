using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class Hazard : MonoBehaviour {
    public float damage = 0.0f;
    public float damageMod = 1.0f;

    protected Camera cameraRef;
    public GameObject player;

    protected AnimationController2D _animator;
    protected Animation2D _sprite;
    protected AudioSource _audioSource;

    public PolygonCollider2D[] colliderList;
    private MultiCollider2D _multiCollider;
    public Frame[] sprite; //public sprite array so we can define in the editor
    public int[] hitFrames;

    public bool showColliders;
    public bool loop = false;
    public bool animFadeOut = false;
    private GameObject meshHolder; //This object is to hold a meshrender and mesh filter to show hitboxes.  This is for debugging and testing purposes and the reason we need another gameobject to hold these components is because the mesh renderer will conflict with the hazard's sprite renderer.
    private MeshFilter meshFilter;
    //private Mesh mesh;
    private int currentPointCount = 0;

    protected  void Awake() {
    }

	// Use this for initialization
	protected void Start () {
        _animator = new AnimationController2D(GetComponent<SpriteRenderer>(), new Animation2D(sprite, 1, "sprite",true, loop, animFadeOut));
        GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
        //create a multicollider via the polygon collider array
        _multiCollider = new MultiCollider2D(colliderList);
        _audioSource = GetComponent<AudioSource>();
        //Set all the colliders to triggers, as a hazard, we dont to have any physics or interact with physics dyrectly, if gravity of physics is applicable ulter the rigidbody of the gameOb
        foreach (PolygonCollider2D collider in _multiCollider.colliders)
            collider.isTrigger = true;

        //Create the meshholder object and attach it as a child, it holds just the mesh renderer and the mesh filter. and is fed the shape from our colliders
        meshHolder = new GameObject();
        meshHolder.transform.position = transform.position;
        meshHolder.transform.rotation = transform.rotation;
        meshHolder.transform.localScale = transform.localScale;
        //Attach the object as a child

        meshHolder.transform.parent = gameObject.transform;
        //Add mesh renderer component
        meshHolder.AddComponent<MeshRenderer>();
        meshHolder.AddComponent<MeshFilter>();
        //Finally set out pointer to the meshfilter to the one now attached to the child gameobject
        meshFilter = GetComponentInChildren<MeshFilter>();

        //grab point set form the first hitbox
        currentPointCount = _multiCollider.GetActiveCollider().GetTotalPointCount();

        Mesh mesh = new Mesh();

        Vector2[] points = _multiCollider.GetActiveCollider().points;
        Vector3[] vertices = new Vector3[currentPointCount];
        for (int j = 0; j < currentPointCount; j++)
        {
            Vector2 actual = points[j];
            vertices[j] = new Vector3(actual.x, actual.y, gameObject.transform.position.z);
        }
        Triangulator tr = new Triangulator(points);
        int[] triangles = tr.Triangulate();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        if (showColliders)
            meshFilter.mesh = mesh;
        else
            meshFilter.mesh = null;
        _animator.currAnim.paused = false;
    }
	
	// Update is called once per frame
	protected void Update () {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);


        _multiCollider.EnableCollider(false);
        if (!_multiCollider.GetActiveCollider().enabled) { 
            meshFilter.mesh = null;
        }
        for (int i = 0; i < hitFrames.Length; ++i)
        {
            if (_animator.currAnim.currFrame == hitFrames[i])
            {
                //Range Check
                if (i < _multiCollider.colliders.Length)
                    SwitchHitBox(i);
            }
        }
        _animator.Update();
        if (_animator.currAnim.finished && !_audioSource.isPlaying)
            Delete();
    }

    private void SwitchHitBox(int index) {
        _multiCollider.SwitchAndEnableCollider(index);
        if (showColliders)
        {
            Mesh mesh = new Mesh();
            currentPointCount = _multiCollider.GetActiveCollider().GetTotalPointCount();
            Vector2[] points = _multiCollider.GetActiveCollider().points;
            Vector3[] vertices = new Vector3[currentPointCount];
            for (int j = 0; j < currentPointCount; j++)
            {
                Vector2 actual = points[j];
                vertices[j] = new Vector3(actual.x, actual.y, gameObject.transform.position.z);
            }
            Triangulator tr = new Triangulator(points);
            int[] triangles = tr.Triangulate();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            meshFilter.mesh = mesh;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject != player) {
            OnHit(other);
        }
    }

    public virtual void OnHit(Collider2D other) {
       
    }

    //protected void OnTriggerStay2D(Collision2D other) {
    //    OnHit(other);
    //}

    public virtual void Delete() {
        Destroy(gameObject);
    }
}
