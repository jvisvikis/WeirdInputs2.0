using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FakeImageCAPTCHA : Window
{
    [SerializeField] private List<Toggle> squares;

    private List<int> greenList;
    // Start is called before the first frame update
    void Start()
    {
        ResetGreenSquares();
    }

    private void ResetGreenSquares()
    {
        List<int> numbers = new List<int>();
        for (int i = 0; i < squares.Count; i++)
        {
            numbers.Add(i);
        }

        greenList = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int num = Random.Range(0, numbers.Count);
            greenList.Add(numbers[num]);
            numbers.Remove(num);
        }
        foreach (Toggle toggle in squares)
        {
            toggle.isOn = false;

            if (greenList.Contains(squares.IndexOf(toggle)))
                toggle.transform.GetChild(0).GetComponent<Image>().color = Color.green;
            else
                toggle.transform.GetChild(0).GetComponent<Image>().color = new Color(Random.Range(0.0f, 1.0f), 0, Random.Range(0.0f, 1.0f));
        }
    }

    public override bool CheckAnswer()
    {
        int greenCount = 0;
        for (int i = 0; i < squares.Count; i++)
        {
            if (squares[i].isOn && !greenList.Contains(i))
            {
                return false;
            }
            if (squares[i].isOn && greenList.Contains(i))
            {
                greenCount++;
                squares[i].isOn = false;
            }
        }
        ResetGreenSquares();
        return greenCount == 3;
    }
}
