using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Player _player;
    private abilityEnabler _abilityEnabler;

    private float counter = 0;
    private float counterJump = 0;
    private float walkCounter = 0;

    private float AxH, AxV;
    private int lastInput;

    void Start()
    {
        _player = FindObjectOfType<Player>();

    }

    void Update()
    {
        

        walkCounter += Time.deltaTime;
        _player.SR.flipX = AxH < 0;
        
        UpdateLastInput();

        
        // Otras mecánicas
        //BreakStun();
        //Hide();
        //StealthModeEnter();
    }

    public  void UpdateLastInput()
    {
        if (AxH < 0 && AxV == 0) lastInput = 3;
        else if (AxH > 0 && AxV == 0) lastInput = 2;
        else if (AxH == 0 && AxV < 0) lastInput = 1;
        else if (AxH == 0 && AxV > 0) lastInput = 0;
    }
    public static void HandleInput(Player player, Vector2 movementDirection)
    {
        if (Player.playerInstance.canMove==false)
        {
            return;
        }
        ICommand moveCommand = new WalkCommand(player, movementDirection);
        moveCommand.Execute();
        ICommand jumpCommand = new JumpCommand(player);
        jumpCommand.Execute();
    }
    /*private void ExecuteCommands()
    {

        Vector2 movementDirection = new Vector2(AxH, AxV).normalized;

        if (movementDirection != Vector2.zero)
        {
            ICommand moveCommand = new WalkCommand(_player, movementDirection);
            moveCommand.Execute();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ICommand jumpCommand = new JumpCommand(_player);
           // jumpCommand.Execute();
        }
    }*/
}
