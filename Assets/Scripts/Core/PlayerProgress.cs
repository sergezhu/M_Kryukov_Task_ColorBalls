using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class PlayerProgress
    {
        public event Action Changed;
        
        [SerializeField]
        private List<int> _resultsHistory;

        public void Init()
        {
            _resultsHistory = new List<int>();
            Changed?.Invoke();
        }

        public void AddResult(int result)
        {
            _resultsHistory.Add(result);
            Changed?.Invoke();
        }

        public int GetLastResult() => _resultsHistory[_resultsHistory.Count - 1];

        public int GetBestResult()
        {
            var bestResult = 0;
            _resultsHistory.ForEach(result=> bestResult = Mathf.Max(result, bestResult));

            return bestResult;
        }
    }
}