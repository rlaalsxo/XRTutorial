using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARImageAnimal : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    [SerializeField] GameObject[] gameObjects;

    private Dictionary<string, GameObject> spawnObjects;
    private void Awake()
    {
        spawnObjects = new Dictionary<string, GameObject>();
    }
    private void Start()
    {
        foreach (GameObject obj in gameObjects)
        {
            GameObject newObj = Instantiate(obj);
            newObj.name = obj.name;
            newObj.SetActive(false);

            spawnObjects.Add(newObj.name, newObj);
        }
    }
    private void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrakedImageChanged;
    }
    private void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrakedImageChanged;
    }
    void OnTrakedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            UpdateSqawnObject(newImage);
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            UpdateSqawnObject(updatedImage);
        }

        foreach (var removedImage in eventArgs.removed)
        {
            spawnObjects[removedImage.name].SetActive(false);
        }
    }
    void UpdateSqawnObject(ARTrackedImage arTrackedImage)
    {
        string referencename = arTrackedImage.referenceImage.name;

        spawnObjects[referencename].transform.position = arTrackedImage.transform.position;
        spawnObjects[referencename].transform.rotation = arTrackedImage.transform.rotation;
        spawnObjects[referencename].SetActive(true);
    }
}
