using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEnableEvent : MonoBehaviour, IResetable
{
    public Door[] DoorsToEnable;
    public GameObject InteractButton;
    public SpriteRenderer Renderer;
    private ObjectState objectState;
    private Transform playerTf;
    private float distanceToStopInteracting = 3f;

    public void ResetObject()
    {
        objectState = ObjectState.Start;
        Renderer.color = Color.red;
        ToogleInteractButton(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameplayController.Instance.RegisterToReset(this);
        ResetObject();
    }

    private void Update()
    {
        if (objectState == ObjectState.PlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Renderer.color = Color.green;

                if(DoorsToEnable != null)
                {
                    for(int i = 0; i < DoorsToEnable.Length; i++)
                    {
                        DoorsToEnable[i].EnableDoor();
                    }
                }

                ToogleInteractButton(false);
                objectState = ObjectState.Finished;

            }
            else if (Vector2.Distance(transform.position, playerTf.position) > distanceToStopInteracting)
            {
                ToogleInteractButton(false);
                objectState = ObjectState.Start;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" || objectState == ObjectState.Finished)
            return;

        playerTf = collision.gameObject.transform;
        objectState = ObjectState.PlayerInRange;
        ToogleInteractButton(true);
    }

    private void ToogleInteractButton(bool toogle)
    {
        InteractButton.SetActive(toogle);
    }
}
