using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager {

    private static EffectManager _instance;
    public static EffectManager GetInstance() {
        if (_instance == null)
            _instance = new EffectManager();
        return _instance;
    }
    private EffectManager() {
        effectedObjects = new List<EffectedObject>();
    }

    private List<EffectedObject> effectedObjects;

    public void Shake(Vector2 direction, GameObject obj, float duration, float intensity, float decay) {
        EffectedObject container = new EffectedObject(obj);
        container.shake = true;
        container.shakeDuration = duration;
        container.shakeVector = direction;
        container.intensity = intensity;
        container.decay = decay;
        container.startPos = obj.transform.position;
        if (effectedObjects.Count > 0)
            for (int i = effectedObjects.Count - 1; i >= 0; --i)
                if (effectedObjects[i].obj == obj)
                    effectedObjects.Remove(effectedObjects[i]);
        effectedObjects.Add(container);
    }

    public void CreateDustCloud(Vector2 position, float rotationAngle)
    {
        TempSpriteManager spriteManager = TempSpriteManager.GetInstance();

        Vector2 dustTrailSpeed = new Vector2(0.015f, 0.0f);
        Vector2 smallDustSpeed = new Vector2(0.005f, 0.0f);

        float cs = Mathf.Cos(Mathf.Deg2Rad * rotationAngle);
        float sn = Mathf.Sin(Mathf.Deg2Rad * rotationAngle);

        Vector2 offset1 = new Vector2(0.0f, 1.0f);
        offset1 = new Vector2(offset1.x * cs - offset1.y * sn, offset1.x * sn + offset1.y * cs);

        Vector2 offset2 = new Vector2(0.7f, 0.0f);
        offset2 = new Vector2(offset2.x * cs - offset2.y * sn, offset2.x * sn + offset2.y * cs);
        Vector2 offset3 = new Vector2(-0.7f, 0.0f);
        offset3 = new Vector2(offset3.x * cs - offset3.y * sn, offset3.x * sn + offset3.y * cs);

        spriteManager.PlayAnimation("Large_Hit_02", position, new Vector2(1.5f, 1.5f), rotationAngle, "Foreground2", 0);
        spriteManager.PlayAnimation("Large_Dust_01", position + offset1, new Vector2(1, 1), rotationAngle, "Ground", 0, 0.1f);
        spriteManager.PlayAnimation("Dust_Trail_Left", position, new Vector2(1, 1), new Vector2(-dustTrailSpeed.x * cs - dustTrailSpeed.y * sn, -dustTrailSpeed.x *sn + dustTrailSpeed.y * cs), rotationAngle, "Ground", 0, 0.2f);
        spriteManager.PlayAnimation("Dust_Trail_Right", position, new Vector2(1, 1), new Vector2(dustTrailSpeed.x * cs - dustTrailSpeed.y * sn, dustTrailSpeed.x * sn + dustTrailSpeed.y * cs), rotationAngle, "Ground", 0 , 0.2f);
        spriteManager.PlayAnimation("Small_Dust_01", position + offset2, new Vector2(1, 1), new Vector2(smallDustSpeed.x * cs - smallDustSpeed.y * sn, smallDustSpeed.x * sn + smallDustSpeed.y * cs), rotationAngle, "Ground", 0, 0.3f);
        spriteManager.PlayAnimation("Small_Dust_01", position + offset3, new Vector2(-1, 1), new Vector2(-smallDustSpeed.x * cs - smallDustSpeed.y * sn, -smallDustSpeed.x * sn + smallDustSpeed.y * cs), rotationAngle, "Ground", 0, 0.3f);
    }

    public void Update() {
        if(effectedObjects.Count > 0) {
            for(int i = effectedObjects.Count - 1; i >= 0; --i) {
                if(effectedObjects[i].shake) {
                    effectedObjects[i].Shake();
                }
                else if (!effectedObjects[i].shake && !effectedObjects[i].scaleY)
                    effectedObjects.Remove(effectedObjects[i]);
            }
        }
    }
}
