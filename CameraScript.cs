using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public bool culling = true;
    public GameObject player;
    public WorldGen world;

    private const float scrollSpeed = 5;
    private bool cameraLocked = true;
    private Vector3 prevPosition;

    void Update()
    {
        CameraLock();
        CameraPan();
        CameraScroll();
        if(culling) FrustumCulling();
    }

    private void PlaceBackground()
    {
        int height = Mathf.RoundToInt(2f * Camera.main.orthographicSize);
        int width = Mathf.RoundToInt(height * Camera.main.aspect);
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        //keep edges of current backgrounds
        //if edge of camera approaches edge of background create new one and delete old one
        //decided which background to pick based on camera position
    }

    private void FrustumCulling()
    {
        int height = Mathf.RoundToInt(2f * Camera.main.orthographicSize);
        int width = Mathf.RoundToInt(height * Camera.main.aspect);
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        for(int i=x-width-5; i<x+width+5; i++)
        {
            for(int j=y-height-5; j<y+height+5; j++)
            {
                if (i < x-width || j < y-height) world.Disable(i, j);
                else if (i < x + width && j < y + height) world.Enable(i, j);
                else world.Disable(i, j);
            }
        }
    }

    private void CameraLock()
    {
        if (cameraLocked)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        }
        if (Input.GetKeyDown("left ctrl")) cameraLocked = true;
    }

    private void CameraPan()
    {
        if (Input.GetMouseButtonDown(2))
        {
            prevPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 direction = prevPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
            cameraLocked = false;
        }
    }

    private void CameraScroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            gameObject.GetComponent<Camera>().orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }
    }
}
