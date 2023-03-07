using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    //change class name "Tree" to another name
    public abstract class BTree : MonoBehaviour
    {
        private Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null) {
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }


}