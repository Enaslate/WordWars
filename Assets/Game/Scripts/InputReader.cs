using System;
using System.Text;

public class InputReader
{
    private readonly StringBuilder _currentInput;
    public int InputLength => _currentInput.Length;
    public int LastIndex => _currentInput.Length - 1;

    public InputReader()
    {
        _currentInput = new StringBuilder();
    }

    public void Append(char inputChar)
    {
        _currentInput.Append(inputChar);
    }

    public void Append(string inputChar)
    {
        _currentInput.Append(inputChar);
    }

    public override string ToString()
    {
        return _currentInput.ToString();
    }

    public void Clear()
    {
        _currentInput.Clear();
    }

    internal string Trim(int trimAmount, int trimThreshold)
    {
        return _currentInput.ToString(trimAmount, trimThreshold);
    }
}