using System;
using UnityEngine;

public class CovarianceContravarianceExample : MonoBehaviour
{
    private void Start()
    {
        Action<Base> baseAction = (target) => { Debug.Log(target.GetType().Name + " contravariance worked with a parameter."); };
        Action<Derived> derivedAction = baseAction;
        derivedAction(new Derived());

        Func<Derived> derivedFunc = () =>
        {
            var derived = new Derived();
            Debug.Log(derived.GetType().Name + " covariance worked with a return value.");
            return derived;
        };

        Func<Base> baseFunc = derivedFunc;
        baseFunc();
    }
}

public class Base { }

public class Derived : Base { }