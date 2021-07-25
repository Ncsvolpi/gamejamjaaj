using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IResetable
{
    public int KeyId;
    private void Start()
    {
        GameplayController.Instance.RegisterToReset(this);
    }
    public void ResetObject()
    {
        gameObject.SetActive(true);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        GameplayController.Instance.AddKey(KeyId);
        gameObject.SetActive(false);
    }

}
