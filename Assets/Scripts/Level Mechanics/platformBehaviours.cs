using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
// Code Written By Nathaniel King <3

public class platformBehaviours : MonoBehaviour
{
    enum pType 
    { 
        singleWayMoving,
        patterned,
        multiplePositions,
        disappearing
    }

    [SerializeField] float freezeTime;
    [SerializeField] float platformSpeed;
    [SerializeField] Transform platform;
    [SerializeField] Transform endPosition;
    [SerializeField] List<Transform> separateTrans;
    [SerializeField] pType platformType;
    [SerializeField] bool isPatteredDisappear;

    Vector3 startPos;
    Vector3 currentPos;
    Vector3 nextPos;
    List<Vector3> separatePositions;
    float freezeTimer;
    int currPosIndex;
    bool isFrozen;
    bool isMovingForward;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        freezeTimer = 0f;
        startPos = platform.position;
        currentPos = platform.position;
        separatePositions = new List<Vector3>(separateTrans.Count + 1);
        for (int i = 0; i < separateTrans.Count + 1; i++) separatePositions.Add(new Vector3());
        for (int i = 0; i < separateTrans.Count; i++) separatePositions[i] = separateTrans[i].position;
        if (separatePositions.Count > 0)
        {
            nextPos = separatePositions[0];
            separatePositions[separatePositions.Count - 1] = startPos;
        }
        currPosIndex = 0;
        isMovingForward = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen)
        {
            Freeze();
        }
        else if (platformType == pType.singleWayMoving)
        {
            moveOneDirection();
        }
        else if (platformType == pType.patterned)
        {
            moveBackAndForth();
        }
        else if(platformType == pType.multiplePositions)
        {
            moveMultipleLocations();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ice")) // TODO Make this check if this is the falling platform
        {
            isFrozen = true;
        }
    }

    void Freeze()
    {
        freezeTimer += Time.deltaTime;
        if (freezeTimer >= freezeTime)
        {
            freezeTimer = 0f;
            isFrozen = false;
        }
    }

    void moveOneDirection()
    {
        platform.position = Vector3.MoveTowards(platform.position, endPosition.position, platformSpeed * Time.deltaTime);
        if (platform.position == endPosition.position) // TODO Check if this is a falling platform and if it hit the endPos
        {
            platform.position = startPos;
        }
        else if (platformType == pType.patterned && platform.position == endPosition.position) // TODO Check if this is a falling platform and if it hit the endPos
        {
            platform.position = startPos;
        }
    }

    void moveBackAndForth()
    {
        if (isMovingForward)
        {
            platform.position = Vector3.MoveTowards(platform.position, endPosition.position, platformSpeed * Time.deltaTime);
            if (platform.position == endPosition.position)
            {
                isMovingForward = false;
            }
        }
        else if (!isMovingForward)
        {
            platform.position = Vector3.MoveTowards(platform.position, startPos, platformSpeed * Time.deltaTime);
            if (platform.position == startPos)
            {
                isMovingForward = true;
            }
        }
    }

    void moveMultipleLocations()
    {
        platform.position = Vector3.MoveTowards(platform.position, nextPos, platformSpeed * Time.deltaTime);
        if(platform.position == nextPos)
        {
            if (currPosIndex >= separatePositions.Count - 1) currPosIndex = 0;
            else currPosIndex++;
            nextPos = separatePositions[currPosIndex];
        }
    }
}
