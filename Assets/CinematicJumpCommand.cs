using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicJumpCommand : ICommand
{
    private Player _player;

    public CinematicJumpCommand(Player player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.StartCinematicJump();
    }
}
