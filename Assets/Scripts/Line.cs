using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Line : MonoBehaviour
{
    private List<GameObject> waypoints = new();
    public int initialZones = 2;
    private int zones = 0;
    private List<Monster> queue = new();

    public static Line instance;

    public GameObject zonePrefab;
    public GameObject[] monsterPrefabs;

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        foreach (Transform child in transform)
        {
            waypoints.Add(child.gameObject);
        }

        for (int i = 0; i < initialZones; i++)
        {
            AddZone();
        }
    }

    public void AddZone()
    {
        var spot = waypoints[zones];
        var zone = Instantiate(zonePrefab, spot.transform);
        zone.transform.localPosition = Vector3.zero;
        zones++;
    }

    public void RemoveFromLine(Monster monster)
    {
        int index = queue.IndexOf(monster);
        if (index == -1)
        {
            return;
        }

        for (int i = index + 1; i < queue.Count; i++)
        {
            queue[i].QueueMovement(waypoints[i - 1].transform.position);
        }

        queue.Remove(monster);
    }


    private void Spawn(GameObject monsterPrefab)
    {
        GameObject lastWaypoint = waypoints[^1];
        GameObject monster = Instantiate(monsterPrefab, lastWaypoint.transform.position, Quaternion.identity);
        for (int i = waypoints.Count - 2; i >= queue.Count; i--)
        {
            monster.GetComponent<Monster>().QueueMovement(waypoints[i].transform.position);
        }
        queue.Add(monster.GetComponent<Monster>());
    }

    private void FixedUpdate()
    {
        if (Random.Range(1, 40) == 1 && queue.Count < waypoints.Count)
        {
            GameObject monster = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
            Spawn(monster);
        }

        for (int i = 0; i < Math.Min(queue.Count, zones); i++)
        {
            queue[i].UnGray();
        }
    }

    public bool isInBoardingZone(Monster monster)
    {
        int index = queue.IndexOf(monster);
        if (index == -1)
        {
            return false;
        }

        return index < zones;
    }
}