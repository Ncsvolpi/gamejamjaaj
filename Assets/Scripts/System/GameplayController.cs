using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Instance;
    public float TotalTimeBeforeReset;
    public Text TempoDecorridoText;

    private GameState state;

    private float currentTimeBeforeReset;

    private List<int> keys = new List<int>();

    private List<IResetable> resetables = new List<IResetable>();

    
    private float tempoDecorrido;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);

        ResetWorld();
    }

    public bool HasKey(int key)
    {
        for(int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == key)
                return true;
        }

        return false;
    }

    public void AddKey(int key)
    {
        keys.Add(key);
    }

    public void RegisterToReset(IResetable resetable)
    {
        resetables.Add(resetable);
    }

    private void ResetWorld()
    {
        currentTimeBeforeReset = TotalTimeBeforeReset;
        keys.Clear();
        tempoDecorrido = 0;

        for (int i = 0; i < resetables.Count; i++)
        {
            resetables[i].ResetObject();
        }
    }

    private void Update()
    {
        tempoDecorrido += Time.deltaTime;
        var tempo = tempoDecorrido / 60;
        TempoDecorridoText.text = "Tempo Decorrido: " + tempo.ToString("0.00");
        switch (state)
        {
            case GameState.InGame:
                {
                    currentTimeBeforeReset -= Time.deltaTime;

                    CharacterController.Instance.UpdateCharacter();

                    if (Input.GetKeyDown(KeyCode.R))
                        ResetWorld();

                    break;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                    Application.Quit();
        }
    }


}

public enum GameState
{
    InGame,
}

public enum ObjectState
{
    Start,
    Moving,
    PlayerInRange,
    Finished,
    Disabled,
}
