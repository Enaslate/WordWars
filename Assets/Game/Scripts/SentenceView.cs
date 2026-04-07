using TMPro;
using UnityEngine;

public class SentenceView : View
{
    [SerializeField] private Camera _camera;
    [SerializeField] private TMP_Text _tmpSentence;
    [SerializeField] private Vector3 _offset;

    private Transform _target;
    private string _sentence;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void Setup(Transform target, string sentence)
    {
        _target = target;
        _sentence = sentence;
        _tmpSentence.text = _sentence;

        if (target != null && _camera != null)
        {
            var position = _camera.WorldToScreenPoint(target.position);
            transform.position = position;
        }
    }

    public void UpdateHighlight(string buffer, int startIndex)
    {
        _tmpSentence.text = GetHighlighted(buffer, startIndex);
    }

    public string GetHighlighted(string buffer, int startIndex)
    {
        int maxLen = Mathf.Min(buffer.Length, _sentence.Length);
        int matchLen = 0;

        for (int len = maxLen; len > 0; len--)
        {
            int startPos = buffer.Length - len;

            if (startPos < startIndex)
                continue;

            bool match = true;
            for (int i = 0; i < len; i++)
            {
                if (buffer[startPos + i] != _sentence[i])
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                matchLen = len;
                break;
            }
        }

        if (matchLen == 0)
            return _sentence;

        string highlighted = "";
        for (int i = 0; i < _sentence.Length; i++)
        {
            if (i < matchLen)
                highlighted += $"<color=yellow>{_sentence[i]}</color>";
            else
                highlighted += _sentence[i];
        }
        return highlighted;
    }

    public override void Reset()
    {
        _tmpSentence.text = string.Empty;
        _sentence = null;
    }
}