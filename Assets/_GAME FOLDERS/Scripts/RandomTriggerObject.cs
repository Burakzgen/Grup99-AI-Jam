using System.Collections.Generic;
using UnityEngine;

public class RandomTriggerObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsList;
    [SerializeField] private int minActiveCount = 5;
    [SerializeField] private int maxActiveCount = 6;

    private void Start()
    {
        ActivateRandomObjects();
    }

    private void ActivateRandomObjects()
    {
        foreach (GameObject obj in objectsList)
        {
            obj.SetActive(false);
        }

        int activeCount = Random.Range(minActiveCount, maxActiveCount + 1);

        List<GameObject> shuffledList = new List<GameObject>(objectsList);
        for (int i = 0; i < shuffledList.Count; i++)
        {
            GameObject temp = shuffledList[i];
            int randomIndex = Random.Range(i, shuffledList.Count);
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }

        for (int i = 0; i < activeCount; i++)
        {
            shuffledList[i].SetActive(true);
        }
    }
}
