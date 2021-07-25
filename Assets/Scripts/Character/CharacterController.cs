using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour, IResetable
{


    [Header("Character Attributes")]
    public float Speed;
    public float JumpForce;

    public static CharacterController Instance;

    private CharacterInstance character;

    private PlayerInput input;

    private bool wasOnAir = true;
    private float projectileCd;

    private CharacterState state;

    private float inputDelay;

    private float coyoteJump;

    private Vector2 startPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);

        Setup();
    }

    public void ForceState(CharacterState newState)
    {
        state = newState;
    }

    private void Start()
    {
        GameplayController.Instance.RegisterToReset(this);
    }


    public void ResetObject()
    {
        character.SetMovement(Vector2.zero);
        character.transform.position = startPosition;
    }
    public void Setup()
    {
        if (character == null)
        {
            character = GetComponentInChildren<CharacterInstance>(true);
            character.Setup();
        }

        if (input == null)
            input = new PlayerInput();

        state = CharacterState.Normal;

        startPosition = character.transform.position;
    }

    public CharacterInstance GetPlayer()
    {
        return character;
    }

    public bool IsGrounded()
    {
        return character.CheckIfIsOnGround();
    }

    private void PlayCharacterSound(string name)
    {
        //  SoundController.instance.PlayAudioEffect(name);
    }
    private void PlayGenericSound(string name)
    {
        //    SoundController.instance.PlayAudioEffect(name);
    }
    public void UpdateCharacter()
    {
        if (inputDelay > 0)
            inputDelay -= Time.deltaTime;

        if (projectileCd > 0)
            projectileCd -= Time.deltaTime;

        if (coyoteJump > 0)
            coyoteJump -= Time.deltaTime;

        input.GetInputs();

        bool isWalking = false;

        switch (state)
        {
            case CharacterState.Normal:
                {
                    var horizontalMOvement = 0f;

                    var grounded = character.CheckIfIsOnGround();

                    if (grounded)
                    {
                        coyoteJump = 0.25f;

                        if (wasOnAir)
                            PlayGenericSound("Fall");

                        character.SetAnimationBool("DoubleJ", false);
                        character.SetAnimationBool("IsJumping", false);
                    }
                    else
                    {
                        character.SetAnimationBool("IsJumping", true);
                    }



                    wasOnAir = !grounded;
                    // JUMP
                    if (input.JumpPressed && inputDelay <= 0)
                    {
                        // isHoldingJumpButton = true;

                        if (grounded || coyoteJump > 0)
                        {
                            character.Jump(JumpForce);
                            inputDelay = 0.2f;
                            coyoteJump = 0;

                            PlayGenericSound("Jump");
                            PlayCharacterSound("jump1");

                        }
                    }
                    else if (input.Horizontal != 0)
                    {
                        if (grounded)
                            isWalking = true;

                        horizontalMOvement = input.Horizontal * Speed;

                        if (grounded)
                            PlayGenericSound("Step");
                    }

                    character.SetXVelocity(horizontalMOvement);

                    character.SetAnimationBool("IsWalking", isWalking);

                    break;
                }

            case CharacterState.SnapToGround:
                {
                    character.SnapToGround();
                    break;
                }
        }


    }
}


public enum CharacterState
{
    Normal,
    Dashing,
    SnapToGround,
}