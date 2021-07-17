using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerable
{
    public IEnumerator Activate(float delay);
}
