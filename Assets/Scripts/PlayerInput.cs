﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;

    private int horizontal = 0, vertical = 0;

    public enum Axis
    {
        Horizontal,
        Vertical
    }

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        horizontal = 0;
        vertical = 0;

        GetKeyboardInput();
        SetMovement();
    }

    void SetMovement()
    {
        if(vertical!=0)
        {
            playerController.SetInputDirection((vertical == 1) ? PlayerDirection.UP : PlayerDirection.DOWN);
        }

        else if(horizontal!=0)
        {
            playerController.SetInputDirection((horizontal==1)?PlayerDirection.RIGHT:PlayerDirection.LEFT);
        }
    }

    void GetKeyboardInput()
    {
        horizontal = GetAxisRaw(Axis.Horizontal);
        vertical = GetAxisRaw(Axis.Vertical);

        if(horizontal!=0)
        {
            vertical = 0;
        }
    }

    int GetAxisRaw(Axis axis)
    {
        if (axis == Axis.Horizontal)
        {
            bool left = Input.GetKeyDown(KeyCode.LeftArrow);
            bool right = Input.GetKeyDown(KeyCode.RightArrow);

            if (left)
            {
                return -1;
            }

            if (right)
            {
                return 1;
            }
        }

        else if(axis==Axis.Vertical)
        {
            bool up = Input.GetKeyDown(KeyCode.UpArrow);
            bool down = Input.GetKeyDown(KeyCode.DownArrow);

            if(up)
            {
                return 1;
            }

            if(down)
            {
                return -1;
            }
        }

        return 0;
    }
}
