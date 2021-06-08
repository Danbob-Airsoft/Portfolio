using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Switch : MonoBehaviour
{
    [HideInInspector] public bool On;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Init()
    {
        anim = GetComponent<Animator>();

        if (!anim)
            Debug.Log("Anim got");
    }

    public virtual void Switched()
    {
        if (On)
            anim.SetTrigger("TurnOff");
        else
            anim.SetTrigger("TurnOn");

        On = !On;
    }

    private void OnMouseUpAsButton()
    {
        Switched();
    }
}
