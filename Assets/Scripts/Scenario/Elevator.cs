using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour, IResetable
{
    public float OpeningSpeed;
    public Transform EndPoint;
    public Transform DoorVisual;
    public GameObject OpenButtonDoor;

    private float distanceToStopInteracting = 3f;
    public ObjectState objectState;
    private Vector2 startPosition;
    private Transform playerTf;
    private Vector2 currentTarget;
    private bool isOnFloor = false;
    private void Start()
    {
        startPosition = DoorVisual.position;
        ResetObject();
        GameplayController.Instance.RegisterToReset(this);
        currentTarget = EndPoint.position;
    }

    public void ResetObject()
    {
        objectState = ObjectState.Start;
        DoorVisual.position = startPosition;
        ToogleDoorButton(false);
        isOnFloor = false;
        ChangeTarget();
    }

    void Update()
    {
        if (objectState == ObjectState.Moving)
        {
            DoorVisual.position = Vector2.MoveTowards(DoorVisual.position, currentTarget, OpeningSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, currentTarget) <= 0.05f)
            {
                objectState = ObjectState.PlayerInRange;
                ToogleDoorButton(true);
                ChangeTarget();
                CharacterController.Instance.ForceState(CharacterState.Normal);
            }
        }
        else if (objectState == ObjectState.PlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                /*if (KeyIdRequired != -1 && !GameplayController.Instance.HasKey(KeyIdRequired))
                    return;*/

                objectState = ObjectState.Moving;
                ToogleDoorButton(false);
                CharacterController.Instance.ForceState(CharacterState.SnapToGround);

            }
            else if (Vector2.Distance(DoorVisual.position, playerTf.position) > distanceToStopInteracting)
            {
                ToogleDoorButton(false);
                objectState = ObjectState.Start;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        if (objectState == ObjectState.Start)
        {
            playerTf = collision.transform;
            ToogleDoorButton(true);
            objectState = ObjectState.PlayerInRange;
        }
    }

    private void ToogleDoorButton(bool toogle)
    {
        OpenButtonDoor.SetActive(toogle);
    }


    private void ChangeTarget()
    {
        if (!isOnFloor)
            currentTarget = EndPoint.position;
        else
            currentTarget = startPosition;

        isOnFloor = !isOnFloor;
    }

}
