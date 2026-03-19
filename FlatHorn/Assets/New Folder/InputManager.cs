using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    Dictionary<BtnType, InputActionData> btnActionMap;

    class InputActionData
    {
        public InputAction inputAction;

        uint pressTime = 0;

        public void Update()
        {
            if (inputAction.IsPressed()) pressTime++; else pressTime = 0;
        }

        public bool IsPress()
        {
            if (pressTime > 0) return true;
            return false;
        }

        public bool IsTrigger()
        {
            if (pressTime == 1) return true;
            return false;
        }

        public Vector2 GetStick()
        {
            return inputAction.ReadValue<Vector2>();
        }
    }

    public enum BtnType
    {
        A_Action,

        PlayerMove,
        CameraMove,
        Rowling,
    }

    private void Awake()
    {
        btnActionMap = new();

        InputActionData inputActionData = new InputActionData();
        inputActionData.inputAction = InputSystem.actions.FindAction("A_Action");
        btnActionMap.Add(BtnType.A_Action, inputActionData);    }

    private void Update()
    {
        foreach(var data in btnActionMap)
        {
            data.Value.Update();
        }
    }

    public bool IsPressed(BtnType tag)
    {
        bool flag = false;
        if (btnActionMap.ContainsKey(tag))
            flag = btnActionMap[tag].IsPress();

        return flag;
    }

    public bool IsTrigger(BtnType tag)
    {
        bool flag = false;
        if (btnActionMap.ContainsKey(tag))
            flag = btnActionMap[tag].IsTrigger();

        return flag;
    }

    public Vector2 GetStick(BtnType tag)
    {
        Vector2 vector = Vector2.zero;
        if (btnActionMap.ContainsKey(tag))
            vector = btnActionMap[tag].GetStick();

        return vector;
    }
}
