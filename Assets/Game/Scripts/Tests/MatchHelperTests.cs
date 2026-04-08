#if UNITY_EDITOR
using NUnit.Framework;
using System.Collections.Generic;

public class MatchHelperTests
{
    List<(string word, int startIndex)> _enemies = new()
    {
        ("hello", 0),
        ("world", 0)
    };

    [Test]
    public void GetMatchLength_ExactMatch()
    {
        int len = MatchHelper.GetMatchLength("hello", 0, "hello");
        Assert.AreEqual(5, len);
    }

    [Test]
    public void GetMatchLength_PartialMatch()
    {
        int len = MatchHelper.GetMatchLength("hel", 0, "hello");
        Assert.AreEqual(3, len);
    }

    [Test]
    public void GetMatchLength_NoMatch()
    {
        int len = MatchHelper.GetMatchLength("xyz", 0, "hello");
        Assert.AreEqual(0, len);
    }

    [Test]
    public void GetMatchLength_StartIndexOffset()
    {
        int len = MatchHelper.GetMatchLength("abchello", 3, "hello");
        Assert.AreEqual(5, len);
    }

    [Test]
    public void GetMatchLength_StartIndexBeyondBuffer()
    {
        int len = MatchHelper.GetMatchLength("hi", 5, "hello");
        Assert.AreEqual(0, len);
    }

    [Test]
    public void GetMatchLength_WithMistakeInMiddle()
    {
        int len = MatchHelper.GetMatchLength("hexx", 0, "hello");
        Assert.AreEqual(2, len);
    }
}
#endif