using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{
    // arrays indexes must correspond to indexes of "Type" enums of child classes (0 for pistol projectile, 1 for assaultRifle e.t.c. in this example)
    [SerializeField] protected GameObject[] _prefabsArray;
    [SerializeField] private int[] _startingAmountsArray;
    protected Stack<GameObject>[] _poolsArray;

    // will be initialize in child classes, before calling "CreateStartingPool()"
    protected int _typeEnumNamesCount;

    protected void CreateStartingPool()
    {
        // Set lenght of pools (stacks) array equal to number of different object types within the pool
        _poolsArray = new Stack<GameObject>[_typeEnumNamesCount];

        for (int i = 0; i < _typeEnumNamesCount; i++)
        {
            // initialize each stack
            _poolsArray[i] = new Stack<GameObject>();

            // filling the stack with the required objects amount
            for (int j = 0; j < _startingAmountsArray[i]; j++)
            {
                _poolsArray[i].Push(Instantiate(_prefabsArray[i], transform.position, Quaternion.identity, transform));
            }
        }
    }
}
