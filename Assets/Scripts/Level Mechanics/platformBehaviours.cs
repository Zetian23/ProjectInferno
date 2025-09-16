using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
// Code Written By Nathaniel King <3

public class platformBehaviours : MonoBehaviour, IFreezable
{
    enum pType 
    { 
        singleWayMoving,
        patterned,
        multiplePositions,
        disappearing
    }

    [SerializeField] float freezeTime;
    [SerializeField] float disappearTime;
    [SerializeField] float platformSpeed;
    [SerializeField] GameObject platform;
    [SerializeField] Transform endPosition;
    [SerializeField] List<Transform> separateTrans;
    [SerializeField] pType platformType;
    [SerializeField] bool isPatteredDisappear;

    Vector3 startPos;
    Vector3 currentPos;
    Vector3 nextPos;
    List<Vector3> separatePositions;
    float freezeTimer;
    float disappearTimer;
    int currPosIndex;
    bool disappeared;
    bool isFrozen;
    bool isMovingForward;
    bool playerTouched;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        freezeTimer = 0f;
        startPos = platform.transform.position;
        currentPos = platform.transform.position;
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
            freezeTimer += Time.deltaTime;
            if (freezeTimer >= freezeTime)
            {
                freezeTimer = 0f;
                isFrozen = false;
            }
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
        else if(platformType == pType.disappearing)
        {
            disappear();
        }
    }
    public void freeze()
    {
        if(platformType != pType.disappearing) isFrozen = true;
    }

    public void unfreeze()
    { }

    void moveOneDirection()
    {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, endPosition.position, platformSpeed * Time.deltaTime);
        if (platform.transform.position == endPosition.position) // TODO Check if this is a falling platform and if it hit the endPos
        {
            platform.transform.position = startPos;
        }
        else if (platformType == pType.patterned && platform.transform.position == endPosition.position) // TODO Check if this is a falling platform and if it hit the endPos
        {
            platform.transform.position = startPos;
        }
    }

    void moveBackAndForth()
    {
        if (isMovingForward)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, endPosition.position, platformSpeed * Time.deltaTime);
            if (platform.transform.position == endPosition.position)
            {
                isMovingForward = false;
            }
        }
        else if (!isMovingForward)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, startPos, platformSpeed * Time.deltaTime);
            if (platform.transform.position == startPos)
            {
                isMovingForward = true;
            }
        }
    }

    void moveMultipleLocations()
    {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, nextPos, platformSpeed * Time.deltaTime);
        if(platform.transform.position == nextPos)
        {
            if (currPosIndex >= separatePositions.Count - 1) currPosIndex = 0;
            else currPosIndex++;
            nextPos = separatePositions[currPosIndex];
        }
    }

    void disappear()
    {
        disappearTimer += Time.deltaTime;
        if (disappearTimer >= disappearTime)
        {
            if (!isPatteredDisappear && playerTouched && !disappeared)
            {
                gamemanager.instance.player.transform.parent = null;
                disappeared = true;
                platform.SetActive(false);
                playerTouched = false;
            }
            else if(disappeared)
            {
                disappeared = false;
                platform.SetActive(true);
            }
            else if(isPatteredDisappear && !disappeared)
            {
                gamemanager.instance.player.transform.parent = null;
                disappeared = true;
                platform.SetActive(false);
            }
            disappearTimer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Player")) playerTouched = true;
        }
    }
}
