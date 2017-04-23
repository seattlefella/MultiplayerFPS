using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class Util {

        public static void SetLayerRecursivly(GameObject _obj, int _newLayer)
        {
            if (_obj == null)
            {
                return;
            }

            // Set the layer on the root object
            _obj.layer = _newLayer;

            // Recursively step through all child objects in the tree
            foreach (Transform _child in _obj.transform)
            {
                if (_child == null)
                {
                    continue;
                }
                else
                {
                    SetLayerRecursivly(_child.gameObject, _newLayer);
                }
            }
        }

    }
}
