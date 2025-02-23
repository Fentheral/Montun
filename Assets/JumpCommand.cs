using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : ICommand
{
    private Player _player;
    private Vector2 _direction;


    public JumpCommand(Player player, Vector2 direction)
    {
        _player = player;
        _direction = direction;
    }

    public void Execute()
    {
        _player.Jump(_direction);
    }
}
