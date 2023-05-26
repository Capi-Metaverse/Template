using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EditTestsPrueba
{
    //Here instaciate all objects for your testing
    [SetUp]
    public void SetUp()
    {
        Debug.Log("SetUP");
    }

    //Here delete all objects instanciated on SetUp
    [TearDown]
    public void TearDown()
    {
        Debug.Log("TearDown");
    }

    // A Test behaves as an ordinary method
    [Test]
    public void EditTestsPruebaSimplePasses()
    {
        // Use the Assert class to test conditions
    }
}
