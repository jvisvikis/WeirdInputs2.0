using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleChoiceWindow : Window
{
    [SerializeField] private int correctIdx;
    private int currentlySelected = -1;
    public override bool CheckAnswer()
    {
        if (currentlySelected == -1)
        {
            throw new System.NotImplementedException();
        }
        bool correct = correctIdx == currentlySelected;
        currentlySelected = -1;
        return correct;
    }

    public void Select(int i)
    {
        currentlySelected = i;
    }
}
