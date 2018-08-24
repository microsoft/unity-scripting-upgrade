using System;
using UnityEngine;

public class CovarianceContravarianceExample : MonoBehaviour
{
    private void Start()
    {
        Action<Base> baseAction = (target) => { Debug.Log(target.GetType().Name + " covariance / contravariance worked"); };
        Action<Derived> derivedAction = baseAction;
        derivedAction(new Derived());        
    }
}
public class Base { }
public class Derived : Base { }
