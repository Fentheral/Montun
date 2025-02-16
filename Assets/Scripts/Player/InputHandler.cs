using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Player _player;

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
        counter += Time.deltaTime;
        counterJump += Time.deltaTime;
        walkCounter += Time.deltaTime;

        AxV = Input.GetAxisRaw("Vertical");
        AxH = Input.GetAxisRaw("Horizontal");

        // Control de dirección para flip
        _player.SR.flipX = AxH < 0;
        

        // Guardar último input para animaciones
        UpdateLastInput();

        // Ejecutar comandos según input
        ExecuteCommands();

        // Otras mecánicas
        //BreakStun();
        //Hide();
        //StealthModeEnter();
    }

    private void UpdateLastInput()
    {
        if (AxH < 0 && AxV == 0) lastInput = 3;
        else if (AxH > 0 && AxV == 0) lastInput = 2;
        else if (AxH == 0 && AxV < 0) lastInput = 1;
        else if (AxH == 0 && AxV > 0) lastInput = 0;
    }
    public static void HandleInput(Player player, Vector2 movementDirection)
    {
        if (movementDirection != Vector2.zero)
        {
            ICommand moveCommand = new WalkCommand(player, movementDirection);
            moveCommand.Execute();
        }
    }
    private void ExecuteCommands()
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
    }
}
