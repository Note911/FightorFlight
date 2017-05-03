using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public static int padding = 25;

    public int playerIndex;

    private PlayableEntity player;
    public RectTransform healthBar;
    public RectTransform tempHealthBar;

    private RectTransform thisTransform;
    private float posX;

    public Sprite lifeCounter;
    private List<GameObject> lifeSprites;

    private int numLives;
    private int currentLife;
    public float livesPadding = 5.0f;

    private bool init = false;

    // Use this for initialization
    void Start () {	}
	
	// Update is called once per frame
	void Update ()
    {

        if(player == null)
            Init();
        if (player != null)
        {
            healthBar.localScale = new Vector3(player.GetHealth() / player.maxHealth, 1.0f, 1.0f);
            tempHealthBar.localScale = new Vector3(player.GetTempHealth() / player.maxHealth, 1.0f, 1.0f);

            //If player lost lives
            if (player.lives < numLives)
            {
                currentLife = player.lives;
                for (int i = currentLife; i < numLives; i++)
                {
                    lifeSprites[i].SetActive(false);
                }
                numLives = currentLife;
            }
            //If player gained lives
            else if (player.lives > numLives)
            {
                numLives = player.lives;
                for (int i = 0; i < numLives; i++)
                {
                    lifeSprites[i].SetActive(true);
                }
                currentLife = numLives;
            }
        }
    }

    void Init()
    {
        if (!init)
        {
            if (GameControl.control.playersInScene[playerIndex] != null)
            {
                    player = GameControl.control.playersInScene[playerIndex].GetComponent<PlayableEntity>();
            }
            else
            {
                gameObject.SetActive(false);
            }
            if (player != null)
            {
                healthBar.sizeDelta = new Vector2(Screen.width / 4.0f - padding, Screen.height / 15.0f);
                healthBar.GetComponent<Image>().color = player.color2;
                tempHealthBar.sizeDelta = new Vector2(Screen.width / 4.0f - padding, Screen.height / 15.0f);
                tempHealthBar.GetComponent<Image>().color = player.color;

                thisTransform = GetComponent<RectTransform>();
                posX = Screen.width / 2.0f; //Middle
                switch (playerIndex)
                {
                    case (0):
                        posX = posX - (healthBar.rect.width * 2.0f + padding * 1.5f);
                        break;
                    case (1):
                        posX = posX - (healthBar.rect.width + (padding * 0.5f));
                        break;
                    case (2):
                        posX = posX + (healthBar.rect.width + (padding * 0.5f)) - healthBar.rect.width;
                        break;
                    case (3):
                        posX = posX + (healthBar.rect.width * 2.0f + padding * 1.5f) - healthBar.rect.width;
                        break;
                }
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(posX, 0.0f, 0.0f));
                thisTransform.position = new Vector3(newPosition.x, thisTransform.position.y, 0.0f);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
                initLives();
                init = true;
            }
        }

    }


    void initLives()
    {
        if (!init)
        {
            numLives = player.lives;
            currentLife = numLives;
            lifeSprites = new List<GameObject>();

            for (int i = 0; i < numLives; i++)
            {
                GameObject lifeObj = new GameObject();
                lifeObj.transform.parent = transform;
                SpriteRenderer rend = lifeObj.AddComponent<SpriteRenderer>();
                rend.sortingLayerName = "UI";
                rend.sortingOrder = 1;
                rend.color = player.color;
                rend.sprite = lifeCounter;
                //temp
                lifeObj.transform.localScale = new Vector3(75, 75, 1.0f);

                float x = i * (lifeCounter.rect.width / 100.0f) + (padding / (100.0f));
                float y = 1.0f;
                lifeObj.transform.position = thisTransform.position + new Vector3(x, y, thisTransform.position.z);

                lifeSprites.Add(lifeObj);
            }
        }
    }
}
