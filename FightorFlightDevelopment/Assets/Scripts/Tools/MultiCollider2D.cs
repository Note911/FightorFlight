using UnityEngine;
using System.Collections;

public class MultiCollider2D {

    public PolygonCollider2D[] colliders;
    private PolygonCollider2D _activeCollider;

    //public bool showColliders;
    //public MeshFilter meshFilter;
   // private int currentPointCount = 0;
	
    // Use this for initialization
	public MultiCollider2D(PolygonCollider2D[] _colliders) {
            colliders = _colliders;
        SwitchCollider(0);

	}

    public void SwitchAndEnableCollider(int colliderIndex) {
        //Range check
        if(colliderIndex < colliders.Length) {
            foreach (PolygonCollider2D collider in colliders)
                collider.enabled = false;
            _activeCollider = colliders[colliderIndex];
            _activeCollider.enabled = true;
        } 
    }

     public void SwitchCollider(int colliderIndex) {
        //Range check
        if(colliderIndex < colliders.Length) {
            foreach (PolygonCollider2D collider in colliders)
                collider.enabled = false;
            _activeCollider = colliders[colliderIndex];
        }
    }

    public void EnableCollider(bool enable) {
        _activeCollider.enabled = enable;
    }

    public PolygonCollider2D GetActiveCollider() {
        return _activeCollider;
    }

    public void AttachCollider(BaseGameEntity gameObject) {

    }

    public void UpdateCollider(BaseGameEntity gameObject) {
    }
}
