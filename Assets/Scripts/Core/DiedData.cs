using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public struct DiedData
    {
        [SerializeField]
        private int _lastResult;
        [SerializeField]
        private int _bestResult;

        public DiedData(int lastResult, int bestResult)
        {
            _lastResult = lastResult;
            _bestResult = bestResult;
        }

        public int LastResult => _lastResult;
        public int BestResult => _bestResult;
    }
}