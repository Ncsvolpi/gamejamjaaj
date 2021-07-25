using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IResetable
{
    public float OpeningSpeed;
    public Transform EndPoint;
    public Transform DoorVisual;
    public GameObject OpenButtonDoor;
    public ObjectState InitialState = ObjectState.Start;
    [Header("Mesmo da chave")]
    public int KeyIdRequired = -1;
    [Header("Pode ser vazio")]
    public GameObject VisualToHide;


    private float distanceToStopInteracting = 5f;
    private ObjectState objectState = ObjectState.Start;
    private Vector2 doorVisualStartPosition;
    private Transform playerTf;

    private void Awake()
    {
        doorVisualStartPosition = DoorVisual.position;
    }
    private void Start()
    {
        ResetObject();
        GameplayController.Instance.RegisterToReset(this);
    }

    public void EnableDoor()
    {
        objectState = ObjectState.Start;
        ToogleVisualToHide(false);
    }

    public void DisableDoor()
    {
        objectState = ObjectState.Start;
        ToogleVisualToHide(true);
    }

    private void ToogleVisualToHide(bool toogle)
    {
        if (VisualToHide != null)
            VisualToHide.SetActive(toogle);
    }

    private void ChangeDoorState(ObjectState newState)
    {
        objectState = newState;
    }
    public void ResetObject()
    {
        ChangeDoorState(InitialState);
        DoorVisual.position = doorVisualStartPosition;
        ToogleDoorButton(false);
        ToogleVisualToHide(true);
    }

    void Update()
    {
        if (objectState == ObjectState.Moving)
        {
            DoorVisual.position = Vector2.MoveTowards(DoorVisual.position, EndPoint.position, OpeningSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, EndPoint.position) <= 0.05f)
            {
                ChangeDoorState(ObjectState.Finished);
            }
        }
        else if (objectState == ObjectState.PlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (KeyIdRequired != -1 && !GameplayController.Instance.HasKey(KeyIdRequired))
                    return;

                ChangeDoorState(ObjectState.Moving);
                ToogleDoorButton(false);

            }
            else if (Vector2.Distance(DoorVisual.position, playerTf.position) > distanceToStopInteracting)
            {
                ToogleDoorButton(false);
                ChangeDoorState(ObjectState.Start);
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
            ChangeDoorState(ObjectState.PlayerInRange);
        }
    }

    private void ToogleDoorButton(bool toogle)
    {
        OpenButtonDoor.SetActive(toogle);
    }
}

