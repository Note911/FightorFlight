using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    public Vector3 speed;
    public Vector3 startPos;
    private Vector3 copyStartPos;

    private Vector2 offset = Vector2.zero;
    public GameObject copy;
    float pictureWidth = 0;

	// Use this for initialization
	void Start () {
        transform.position = startPos;
        pictureWidth = (copy.GetComponent<SpriteRenderer>().sprite.rect.width / 100.0f) * transform.localScale.x;
        copyStartPos = startPos + Vector3.right * pictureWidth;
        copy.transform.position = copyStartPos;
	}

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, startPos) >= pictureWidth)
        {
            transform.position += -speed.normalized * pictureWidth;
        }
        if (Vector3.Distance(copy.transform.position, copyStartPos) >= pictureWidth)
        {
            copy.transform.position += -speed.normalized * pictureWidth;
        }
    }
}
