using System;
using UnityEngine;

public interface IMetaEvent
{
    public GameObject eventObject { get; set; }
    void activate(bool host);
}