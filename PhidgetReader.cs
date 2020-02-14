using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phidget22;

public class PhidgetReader : MonoBehaviour
{
    private VoltageRatioInput force0, force1, force2;
    private VoltageRatioInput magnet3, magnet4, magnet5;

    private bool force0Pressed, force1Pressed, force2Pressed, magnet3Activated, magnet4Activated, magnet5Activated;

    private void Start()
    {
        force0 = new VoltageRatioInput
        {
            HubPort = 0,
            Channel = 0,
            IsHubPortDevice = true,
            IsLocal = true
        };
        force0.Attach += ratioAttachCallback;
        force0.VoltageRatioChange += ratioChangeCallback;

        force1 = new VoltageRatioInput
        {
            HubPort = 1,
            Channel = 0,
            IsHubPortDevice = true,
            IsLocal = true
        };
        force1.Attach += ratioAttachCallback;
        force1.VoltageRatioChange += ratioChangeCallback;

        force2 = new VoltageRatioInput
        {
            HubPort = 2,
            Channel = 0,
            IsHubPortDevice = true,
            IsLocal = true
        };
        force2.Attach += ratioAttachCallback;
        force2.VoltageRatioChange += ratioChangeCallback;

        magnet3 = new VoltageRatioInput
        {
            HubPort = 3,
            Channel = 0,
            IsHubPortDevice = true,
            IsLocal = true
        };
        magnet3.Attach += ratioAttachCallback;
        magnet3.VoltageRatioChange += ratioChangeCallback;

        magnet4 = new VoltageRatioInput
        {
            HubPort = 4,
            Channel = 0,
            IsHubPortDevice = true,
            IsLocal = true
        };
        magnet4.Attach += ratioAttachCallback;
        magnet4.VoltageRatioChange += ratioChangeCallback;

        magnet5 = new VoltageRatioInput
        {
            HubPort = 5,
            Channel = 0,
            IsHubPortDevice = true,
            IsLocal = true
        };
        magnet5.Attach += ratioAttachCallback;
        magnet5.VoltageRatioChange += ratioChangeCallback;

        try
        {
            force0.Open(1000);
            force1.Open(1000);
            force2.Open(1000);
            magnet3.Open(1000);
            magnet4.Open(1000);
            magnet5.Open(1000);
        }
        catch (PhidgetException e) { Debug.Log("force0: " + e.Description); }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha3)) force0Pressed = true;
        else force0Pressed = false;

        if (Input.GetKey(KeyCode.Alpha1)) force1Pressed = true;
        else force1Pressed = false;

        if (Input.GetKey(KeyCode.Alpha2)) force2Pressed = true;
        else force2Pressed = false;

        if (Input.GetKey(KeyCode.Keypad2)) magnet3Activated = true;
        else magnet3Activated = false;

        if (Input.GetKey(KeyCode.Keypad1)) magnet4Activated = true;
        else magnet4Activated = false;

        if (Input.GetKey(KeyCode.Keypad0)) magnet5Activated = true;
        else magnet5Activated = false;
    }

    private void OnDestroy()
    {
        force0.Close();
        force1.Close();
        force2.Close();
        magnet3.Close();
        magnet4.Close();
        magnet5.Close();
    }

    private void ratioAttachCallback(object sender, Phidget22.Events.AttachEventArgs e)
    {
        VoltageRatioInput attachedDevice = (VoltageRatioInput)sender;
        attachedDevice.DataInterval = attachedDevice.MinDataInterval;
        Debug.Log("Attached device " + attachedDevice.DeviceSerialNumber);
    }

    private void ratioChangeCallback(object sender, Phidget22.Events.VoltageRatioInputVoltageRatioChangeEventArgs e)
    {
        if (sender == force0)
        {
            if ((float)e.VoltageRatio >= 0.05f && !force1Pressed && !force2Pressed) force0Pressed = true;
            else force0Pressed = false;
        }
        else if (sender == force1)
        {
            if ((float)e.VoltageRatio >= 0.05f && !force0Pressed && !force2Pressed) force1Pressed = true;
            else force1Pressed = false;
        }
        else if (sender == force2)
        {
            if ((float)e.VoltageRatio >= 0.05f && !force0Pressed && !force1Pressed) force2Pressed = true;
            else force2Pressed = false;
        }
        else if (sender == magnet3)
        {
            if (((float)e.VoltageRatio <= 0.49 || (float)e.VoltageRatio >= 0.51) && !magnet4Activated && !magnet5Activated) magnet3Activated = true;
            else magnet3Activated = false;
        }
        else if (sender == magnet4)
        {
            if (((float)e.VoltageRatio <= 0.49 || (float)e.VoltageRatio >= 0.51) && !magnet3Activated && !magnet5Activated) magnet4Activated = true;
            else magnet4Activated = false;
        }
        else if (sender == magnet5)
        {
            if (((float)e.VoltageRatio <= 0.49 || (float)e.VoltageRatio >= 0.51) && !magnet3Activated && !magnet4Activated) magnet5Activated = true;
            else magnet5Activated = false;
        }
    }

    public bool IsLeftArmpitSafe()
    {
        return force0Pressed;
    }

    public bool IsRightArmpitSafe()
    {
        return force1Pressed;
    }

    public bool IsStomachSafe()
    {
        return force2Pressed;
    }

    public bool IsLeftArmpitTickled()
    {
        return (magnet3Activated && !force0Pressed);
    }

    public bool IsRightArmpitTickled()
    {
        return (magnet4Activated && !force1Pressed);
    }

    public bool IsStomachTickled()
    {
        return (magnet5Activated && !force2Pressed);
    }

    public bool IsBeingTickled()
    {
        return ((magnet3Activated && !force0Pressed) || (magnet4Activated && !force1Pressed) || (magnet5Activated && !force2Pressed));
    }
}
