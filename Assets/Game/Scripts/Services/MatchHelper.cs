using UnityEngine;

public static class MatchHelper
{
    public static int GetMatchLength(string buffer, int startIndex, string word)
    {
        if (startIndex >= buffer.Length) return 0;
        int maxLen = Mathf.Min(buffer.Length - startIndex, word.Length);
        for (int i = 0; i < maxLen; i++)
            if (buffer[startIndex + i] != word[i]) return i;
        return maxLen;
    }

    public static int GetMaxMatchLength(string buffer, string sentence, int startIndex)
    {
        int maxLen = Mathf.Min(buffer.Length, sentence.Length);
        for (int len = maxLen; len > 0; len--)
        {
            int startPos = buffer.Length - len;
            if (startPos < startIndex) continue;

            bool match = true;
            for (int i = 0; i < len; i++)
            {
                if (buffer[startPos + i] != sentence[i])
                {
                    match = false;
                    break;
                }
            }

            if (match) return len;
        }
        return 0;
    }
}