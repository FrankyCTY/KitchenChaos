using System;
using System.Collections; using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<HandleProgressChangedEventArgs> HandleProgressChanged;

    public class HandleProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
