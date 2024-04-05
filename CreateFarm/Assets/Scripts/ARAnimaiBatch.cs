using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARAnimaiBatch : MonoBehaviour
{
    [SerializeField] ARRaycastManager arRaycastManager;
    [SerializeField] Text text;
    GameObject selectedAnimal;
    List<ARRaycastHit> arHits = new List<ARRaycastHit>();
    private void Awake()
    {
        selectedAnimal = arRaycastManager.raycastPrefab;
    }
    public void SelectedAnimal(GameObject animal)
    {
        this.selectedAnimal = animal;
    }
    private void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        Vector2 touchPostion = touch.position;
        arHits.Clear();
        if (IsPointOverUI(touchPostion))
            return;

        if(touch.phase == TouchPhase.Began && arRaycastManager.Raycast(touchPostion, arHits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = arHits[0].pose;
            text.text = pose.position.ToString();
            Instantiate(selectedAnimal, pose.position, pose.rotation);
        }
    }
    bool IsPointOverUI(Vector2 pos)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        return results.Count > 0;
    }
}
