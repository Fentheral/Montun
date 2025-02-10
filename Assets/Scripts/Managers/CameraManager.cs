using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private float zoom;
    private float zoomMoultiplier = 10f;
    private float minZoom = 6f;
    private float maxZoom = 10f;
    private float speed = 0f;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    private Vector3 velocity = Vector3.zero;
    private Vector3 offset=new Vector3(0f,0f,-10f);
    public float smoothTime=0.25f;

    public float counter;
    public float cooldown;
    public float duration;
    public bool isZoomed;


    private void Start()
    {
        isZoomed = false;
        counter = cooldown;
        zoom = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        ZoomEnter();
        FollowUp();
    }

    private void ZoomEnter()
    {
        counter += Time.deltaTime;

        if (counter > cooldown)
        {
            float scroll = -System.Math.Abs(Input.GetAxis("Mouse ScrollWheel"));

            
                zoom -= scroll * zoomMoultiplier;
                zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
                cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref speed, smoothTime);
                if (cam.orthographicSize > maxZoom - 0.1f)
                {
                    isZoomed = true;
                    StartCoroutine(ZoomModeExit());
                }
            
            
        }
    }

    IEnumerator ZoomModeExit()
    {
        if (isZoomed)
        {
            yield return new WaitForSeconds(duration);
            zoom = 0;
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, minZoom, ref speed, smoothTime);
            counter = 0;
            isZoomed = false;
        }
    }

    public void FollowUp()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

}
