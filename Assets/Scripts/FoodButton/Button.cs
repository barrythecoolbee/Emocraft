using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Button : MonoBehaviour
{
    private bool canPressButton;
    public Material lightColor, darkColor;

    MeshRenderer buttonMesh;

    public event EventHandler OnButtonPressed;

    private void Awake()
    {
        buttonMesh = GetComponent<MeshRenderer>();
        canPressButton = true;
    }

    public bool CanPressButton()
    {
        return canPressButton;
    }

    public void PressButton()
    {
        if(canPressButton)
        {
            canPressButton = false;

            buttonMesh.material = darkColor;

            transform.localScale = new Vector3(1, 0.05f, 1);

            OnButtonPressed?.Invoke(this, EventArgs.Empty);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetButton();
    }

    public void ResetButton()
    {
        buttonMesh.material = lightColor;

        transform.localPosition = new Vector3(UnityEngine.Random.Range(-9.5f, -0.5f), transform.localPosition.y, UnityEngine.Random.Range(-9.5f, -0.5f));
        transform.localScale = new Vector3(1, 0.2f, 1);

        canPressButton = true;
    }
}
