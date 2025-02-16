using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkCommand : ICommand
{
    private Player _player;
    private Vector2 _direction;

    public WalkCommand(Player player, Vector2 direction)
    {
        _player = player;
        _direction = direction;
    }

    public void Execute()
    {
        _player.Walk(_direction);
    }
}
