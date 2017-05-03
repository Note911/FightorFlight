using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {
    private float xMargin;		    // Distance in the x axis the player can move before the camera follows.
    private float yMargin;		    // Distance in the y axis the player can move before the camera follows.
    private float zoomInMargin;     // Distance in the x axis the player can move before the gamera zooms in.
    public float marginPercent;     // The percent of the screen size the player can move before the camera follow.
    public float marginZoomPercent; // The percent of the screen size the player can move before the camera zooms in.
    public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
    public float zSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the z axis.
    public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
    public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.
    public float maxZoom;           // The maximum size the camera can have
    private float initZoom;         // The initial size the camera has.
    public bool bindToScreen = false;       // If true restricts all follow targets to the screens bounds 

    public static List<Transform> players;		// Reference to the player's transform.
    private Camera camera;              // Reference to this camera object


    void Awake() {
        // Setting up the reference.
        camera = GetComponent<Camera>();
        players = new List<Transform>();

        //Set up margins
        //Convert marginPercent into a percent
        xMargin = camera.pixelWidth * (marginPercent / 100);
        yMargin = camera.pixelHeight * (marginPercent / 100);
        zoomInMargin = camera.pixelWidth * (marginZoomPercent / 100);
        //Save camera size
        initZoom = camera.orthographicSize;

    }

    void Start() {
    }


    bool CheckXMargin(Transform target, float margin) {
        //Get targets screen position
        Vector3 screenPos = camera.WorldToViewportPoint(target.position);
        //If they player is on screen's x axis...
        //if (screenPos.x > 0 && screenPos.x <= 1)
            // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
            return Mathf.Abs(camera.WorldToScreenPoint(transform.position).x - camera.WorldToScreenPoint(target.position).x) > margin;
       // else
            //return false;
    }


    bool CheckYMargin(Transform target, float margin) {
        //Get targets screen position
        Vector3 screenPos = camera.WorldToViewportPoint(target.position);
        //If they player is on screen's y axis...
        //if (screenPos.y > 0 && screenPos.y <= 1)
            // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
            return Mathf.Abs(camera.WorldToScreenPoint(transform.position).y - camera.WorldToScreenPoint(target.position).y) > margin;
       // else
           // return false;
    }


    void FixedUpdate() {
        TrackPlayers();
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
    }


    void TrackPlayers() {
        // By default the target x, y and z coordinates of the camera are it's current x, y and z coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;
        //init target zoom to the current camera size
        float targetZoom = camera.orthographicSize;


        //Set the initial min max values of player positions to the cameras current position
        Transform min = transform;
        Transform max = transform;

        foreach (Transform player in players) {
            //Find min max positions of players for camera zoom
            if (player.position.x > max.position.x)
                max = player;
            if (player.position.x < min.position.x)
                min = player;
        }
        //    //Make sure players arnt fighting over the camera
        //    if (!CheckXMargin(min, xMargin) || !CheckXMargin(max, xMargin)) {
        //        // If the player has moved beyond the x margin...
        //        if (CheckXMargin(player, xMargin))
        //            // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
        //            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

        //        // If the player has moved beyond the y margin...
        //        if (CheckYMargin(player, yMargin))
        //            // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
        //            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
        //    }
        //}
        //find the average position of all players
        Vector2 targetPos = Vector2.zero;
        int count = 0;

        foreach (Transform player in players) {
            Vector2 position = camera.WorldToScreenPoint(player.position);
            if (position.x <= camera.pixelWidth + 1000 && position.x >= -1000 && position.y <= camera.pixelHeight +1000 && position.y >= -1000) {
                targetPos.x += player.position.x;
                targetPos.y += player.position.y;
                count++;
            }
            //Bind to Screen
            if(bindToScreen) {
                if (position.x <= 0)
                    position.x = 0;
                else if (position.x >= camera.pixelWidth)
                    position.x = camera.pixelWidth;
                if (position.y <= 0)
                    position.y = 0;
                else if (position.y >= camera.pixelHeight)
                    position.y = camera.pixelHeight;
                player.transform.position = new Vector3(camera.ScreenToWorldPoint(position).x,camera.ScreenToWorldPoint(position).y,0);
            }
          
        }
        targetPos.x /= count;
        targetPos.y /= count;

        targetX = Mathf.Lerp(transform.position.x, targetPos.x, xSmooth * Time.deltaTime);
        targetY = Mathf.Lerp(transform.position.y, targetPos.y, ySmooth * Time.deltaTime);
        
        //If both the min and max players are past the camera margin...
        if (CheckXMargin(min, xMargin) && CheckXMargin(max, xMargin))
            //Start zooming out
            targetZoom = Mathf.Lerp(camera.orthographicSize, maxZoom, zSmooth * Time.deltaTime);
        //If no players past the zoom in margin...
        else if (!CheckXMargin(min, zoomInMargin) && !CheckXMargin(max, zoomInMargin))
            //Start zooming in
            targetZoom = Mathf.Lerp(camera.orthographicSize, initZoom, zSmooth * Time.deltaTime);

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        //The target z cannot go past the min Z
        targetZoom = Mathf.Clamp(targetZoom, initZoom, maxZoom);

        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, transform.position.z);

        //Set the camera's zoom to the target zoom
        camera.orthographicSize = targetZoom;
    }

    public static void RegisterPlayers() {
        for(int i = 0; i < 4; ++i) {
            if (GameControl.control.playersInScene[i] != null) { 
                Debug.Log("Adding Player");
                players.Add(GameControl.control.playersInScene[i].transform);
            }
            else
                Debug.Log("Player was null");
        }
    }
}
