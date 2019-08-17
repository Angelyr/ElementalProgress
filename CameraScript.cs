using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldController;

public class CameraScript : MonoBehaviour
{
    public bool culling = true;
    private GameObject player;
    private const float scrollSpeed = 3;
    private const float minZoom = 2;
    private const float maxZoom = 5;
    private bool cameraLocked = true;
    private Vector3 prevPosition;
    private Transform camerafocus;
    private Camera myCamera;
    private const float moveSpeed = .5f;
    
    private void Awake()
    {
        player = GameObject.Find("Player");
        camerafocus = player.transform;
        myCamera = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        CameraLock();
        CameraPan();
        CameraScroll();
        if(culling) FrustumCulling();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (transform.position == camerafocus.position) return;
        Vector3 target = camerafocus.position;
        target.z = -10;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed);
    }

    public void SetPosition(Transform target)
    {
        //transform.position = new Vector3(target.position.x, target.position.y, -10);
        camerafocus = target;
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
                if (i < x-width || j < y-height) Disable(i, j);
                else if (i < x + width && j < y + height) Enable(i, j);
                else Disable(i, j);
            }
        }
    }

    private void CameraLock()
    {
        if (cameraLocked)
        {
            SetPosition(camerafocus);
        }
        if (Input.GetKeyDown("left ctrl")) cameraLocked = true;
    }

    private void CameraPan()
    {
        if (Input.GetMouseButtonDown(2))
        {
            prevPosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 direction = prevPosition - myCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position += direction;
            SetPosition(transform);
            cameraLocked = false;
        }
    }

    private void CameraScroll()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            if (myCamera.orthographicSize < minZoom && Input.GetAxis("Mouse ScrollWheel") > 0f) return;
            if (myCamera.orthographicSize > maxZoom && Input.GetAxis("Mouse ScrollWheel") < 0f) return;

            myCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }
    }
}
