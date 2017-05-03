using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectedObject{

    public GameObject obj;

    public float elapsedTime;
    public float shakeDuration;
    public Vector2 shakeVector;
    public Vector2 startPos;
    public float intensity;
    public float decay;
    public bool shake = false;

    public bool scaleY = false;

	public EffectedObject(GameObject obj) {
        this.obj = obj;
    }

    public void Shake() {
        if (obj != null)
        {
            if (shake)
            {
                if (elapsedTime <= shakeDuration)
                {
                    float result = intensity * (Mathf.Pow(10, -decay * elapsedTime)) * Mathf.Sin(2 * Mathf.PI * (10.0f * elapsedTime));
                    Vector2 targetPos = startPos - shakeVector * result;
                    obj.transform.position = new Vector3(targetPos.x, targetPos.y, obj.transform.position.z);
                }
                else
                {
                    obj.transform.position = new Vector3(startPos.x, startPos.y, obj.transform.position.z);
                    elapsedTime = 0.0f;
                    shake = false;
                }
                elapsedTime += Time.deltaTime;
            }
        }
    }
}
