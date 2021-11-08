using UnityEngine;

namespace Core
{
    public class SaveSystem : MonoBehaviour
    {
        private const string SaveName = "ProgressData";

        private PlayerProgress _currentProgress;

        public PlayerProgress CurrentProgress => _currentProgress;

        private void OnEnable()
        {
            Load();
            CurrentProgress.Changed += OnProgressChanged;
        }
        
        private void OnDisable()
        {
            Save();
            CurrentProgress.Changed -= OnProgressChanged;
        }

        private void OnProgressChanged()
        {
            Save();
        }

        public void Save()
        {
            var playerProgressJson = JsonUtility.ToJson(CurrentProgress);;
            PlayerPrefs.SetString(SaveName, playerProgressJson);
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(SaveName))
            {
                var playerProgressJson = PlayerPrefs.GetString(SaveName);
                _currentProgress = JsonUtility.FromJson<PlayerProgress>(playerProgressJson);
            }
            else
            {
                _currentProgress = new PlayerProgress();
                CurrentProgress.Init();
            }
        }
    }
}