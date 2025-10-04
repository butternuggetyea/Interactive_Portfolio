using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class LightSwitch : InteractableObject
{
    public Light[] _lights;
    public override void Interact()
    {
        ToggleLights();
    }

    private void ToggleLights() 
    {
        if (_lights.Length > 0) 
        {
            if (_lights[0].enabled)
            {
                foreach (Light light in _lights)
                {
                    light.enabled = false;
                }
            }
            else 
            {
                foreach (Light light in _lights)
                {
                    light.enabled = true;
                }
            }
        }
    }

}
