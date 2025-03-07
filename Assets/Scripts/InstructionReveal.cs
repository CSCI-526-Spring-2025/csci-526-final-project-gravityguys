using System;
using TMPro;
using UnityEngine;

public class InstructionReveal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(gameObject.name != "Instruction1")
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        SetNextInstructionActive();
    }

    private void SetNextInstructionActive()
    {
        if (gameObject.name == "Instruction7")
            return;
        int currentInstruction = Convert.ToInt32(gameObject.name.Substring(gameObject.name.Length - 1, 1)) + 1;
        String nextInstructionName = "Instruction" + currentInstruction;
        foreach (GameObject inst in FindObjectsOfType<GameObject>(true))
        {
            if(inst.name == nextInstructionName)
            {
                inst.SetActive(true);
                return;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
