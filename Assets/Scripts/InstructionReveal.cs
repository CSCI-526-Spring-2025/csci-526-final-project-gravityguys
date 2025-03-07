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
        if (gameObject.name == "Instruction6")//last instruction.
            return;
        int currentInstruction = Convert.ToInt32(gameObject.name.Substring(gameObject.name.Length - 1, 1)) + 1;
        String nextInstructionName = "Instruction" + currentInstruction;
        foreach (GameObject inst in FindObjectsOfType<GameObject>(true))
        {
            if(inst.name == nextInstructionName)
            {
                if (nextInstructionName == "Instruction5")
                {
                   EnableInactiveGameObject("Instruction_Beware");
                   EnableInactiveGameObject("Instruction_Climb");
                }
                inst.SetActive(true);
                return;
            }
            
        }
    }

    private static void EnableInactiveGameObject(String gameObjectName, bool shouldEnable = true)
    {
        foreach (GameObject inst in FindObjectsOfType<GameObject>(true))
        {
            if (inst.name == gameObjectName)
            {
                inst.SetActive(shouldEnable);
            }
        }
    }

    public static void ResetAllInstructions()
    {
        foreach (GameObject inst in FindObjectsOfType<GameObject>(true))
            if(inst.tag == "Instruction")
                inst.SetActive(false);
        EnableInactiveGameObject("Instruction1");
    }
}
