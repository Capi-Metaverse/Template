using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayTestsPrueba
{
    [SetUp]
    private void SetUp()
    {
        Debug.Log("SetUP");
    }

    [TearDown]
    private void TearDown()
    {
        Debug.Log("TearDown");
    }

    // A Test behaves as an ordinary method
    [Test]
    public void PlayTestsPruebaSimplePasses()
    {
        Debug.Log("1");
    }
}
