using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{ 

    [SerializeField] string type;

    bool isActive = false;

    [Header("Cursor/Hammer")]
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;


    //References
    GameController gameController;
    Animator animator;
    void Awake()
    {
        //Get References
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        animator = GetComponent<Animator>();

        //Set Animation to in Active
        isActive = false;
        UpdateAnimation();
    }



    void OnMouseDown()
    {
        // Start Game if not yet started
        if (gameController.GetAvailableToStart())
        {

            StartGameOnClick();
        }

        // Normal Gameplay
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        gameController.SetHammerType(type);
        gameController.TurnOffHammerTypeAnimation();

        isActive = true;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        animator.SetBool("isActive", isActive);
    }

    private void StartGameOnClick()
    {
            gameController.StartGame();
    }



    
}
