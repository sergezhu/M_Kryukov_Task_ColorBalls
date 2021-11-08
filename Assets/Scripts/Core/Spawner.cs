using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Spawner : MonoBehaviour
    {
        public event Action<int> IntensityChanged; 
        
        [SerializeField]
        private MonoBehaviour _spawnedBody;
        [SerializeField]
        private SpawnedObjectsHolder _spawnedObjectsHolder;
        [SerializeField]
        private TapParticle _tapParticle;
        [SerializeField]
        private Transform _particleGroundPlane;

        [Space]
        [SerializeField]
        private AwardHandler _awardHandler;

        [Space]
        [SerializeField]
        private List<SpawnIntensityData> _spawnIntensityData;

        [Header("Movable Body Spawn Settings")]
        [SerializeField][Range(0f, 2.5f)]
        private float _directionDeviation = 0f;
        [SerializeField]
        private RandomBounds _speedRange;
        [SerializeField]
        private RandomBounds _awardPointsRange;
        [SerializeField]
        private RandomBounds _damagePointsRange;
        [SerializeField]
        private Color[] _colors;

        [Space]
        [SerializeField]
        private GameObject _debugObj;

        public IMovableBody SpawnedBody => (IMovableBody) _spawnedBody;

        private Transform _transform;
        private SpawnIntensityData _currentSpawnIntensityData;
        private int _currentSpawnIntensityDataIndex;
        private Timer _intensityTimer;
        private Timer _spawnTimer;

        private List<List<SpawnSlot>> _spawnPoints;

        private void OnValidate()
        {
            if (_spawnedBody is null)
                return;

            if(_spawnedBody is IMovableBody == false)
                Debug.LogError(_spawnedBody.name + " needs to implement " + nameof(IMovableBody));


            if (_speedRange.Min < 0f)
                _speedRange = new RandomBounds(0, _speedRange.Max);

            if (_speedRange.Max < 0f)
                _speedRange = new RandomBounds(_speedRange.Min, 0);
            
            if (_awardPointsRange.Min < 0f)
                _awardPointsRange = new RandomBounds(0, _awardPointsRange.Max);
            
            if (_awardPointsRange.Max < 0f)
                _awardPointsRange = new RandomBounds(_awardPointsRange.Min, 0);
            
            if (_damagePointsRange.Min < 0f)
                _damagePointsRange = new RandomBounds(0, _damagePointsRange.Max);
            
            if (_damagePointsRange.Max < 0f)
                _damagePointsRange = new RandomBounds(_damagePointsRange.Min, 0);
        }

        private void Awake()
        {
            _spawnedObjectsHolder.transform.position = transform.position;
            
            var spawnedBodySizeX = _spawnedBody.transform.localScale.x;
            var spawnAreaSizeX = transform.localScale.x;

            _spawnPoints = GenerateSpawnPoints(spawnedBodySizeX, spawnAreaSizeX, 3, 0.25f);
            
            //InstantiateDebugObjects();
        }


        private List<List<SpawnSlot>>  GenerateSpawnPoints(float spawnedBodySize, float spawnAreaSize, int rows, float spaceBetweenObjects)
        {
            if (spaceBetweenObjects < 0 || rows < 1 || spawnAreaSize <= 0 || spawnedBodySize <= 0)
                throw new ArgumentException("Invalid Arguments");

            var oddRowObjectsCapacity = (int)Mathf.Floor(spawnAreaSize / (spawnedBodySize + spaceBetweenObjects));
            var evenRowObjectsCapacity = oddRowObjectsCapacity - 1;

            var result = new List<List<SpawnSlot>>();
            var realSize = spawnedBodySize * oddRowObjectsCapacity + spaceBetweenObjects * (oddRowObjectsCapacity - 1);
            var leftObjectPosX = -0.5f * realSize + 0.5f * (spawnAreaSize - realSize);

            for (var row = 0; row < rows; row++)
            {
                var rowResult = new List<SpawnSlot>();
                var rowCapacity = row % 2 == 1 ? evenRowObjectsCapacity : oddRowObjectsCapacity;
                var horizontalOffset = row % 2 == 0 ? 0 : (spawnedBodySize + spaceBetweenObjects) / 2f;
                var verticalOffset = row * spawnedBodySize;
                
                for (var i = 0; i < rowCapacity; i++)
                {
                    var posX = leftObjectPosX + horizontalOffset + (spawnedBodySize + spaceBetweenObjects) * i;
                    var posY = verticalOffset;
                    var slot = new SpawnSlot(new Vector3(posX, 0, posY));
                    rowResult.Add(slot);
                }
                
                result.Add(rowResult);
            }

            return result;
        }

        private void Start()
        {
            _intensityTimer = new Timer();
            
            _currentSpawnIntensityDataIndex = -1;
            TrySetNextIntensityData();

            _spawnTimer = new Timer();
            StartSpawnTimer();
        }

        private void Update()
        {
            _intensityTimer.Tick();
            _spawnTimer.Tick();
        }

        private void TrySetNextIntensityData()
        {
            _currentSpawnIntensityDataIndex++;
            if (_currentSpawnIntensityDataIndex == _spawnIntensityData.Count)
                return;
            
            Debug.Log($"Setup Spawn Intensity, Index = {_currentSpawnIntensityDataIndex}");
            
            _currentSpawnIntensityData = _spawnIntensityData[_currentSpawnIntensityDataIndex];
            _intensityTimer.Start(_currentSpawnIntensityData.WaveDuration, TrySetNextIntensityData);
            
            IntensityChanged?.Invoke(_currentSpawnIntensityDataIndex);
        }

        private void StartSpawnTimer()
        {
            if (_currentSpawnIntensityDataIndex == _spawnIntensityData.Count)
                return;
            
            var spawnDelay = 1f / _spawnIntensityData[_currentSpawnIntensityDataIndex].SpawnFrequency;
            _spawnTimer.Start(spawnDelay, () =>
            {
                Spawn();
                StartSpawnTimer();
            });
        }

        private void Spawn()
        {
            var startDirection = Vector3.back;
            var color = _colors[UnityEngine.Random.Range(0, _colors.Length)];
            var deviation = UnityEngine.Random.Range(-1f * _directionDeviation, _directionDeviation);
            var spawnedBodySettings = new MovableSettings(_speedRange.Random, deviation, startDirection, _awardPointsRange.Random, _damagePointsRange.Random, color);

            RefreshSlotsState();
            var slot = FindFreeRandomSpawnSlot();
            
            if (slot == null)
            {
                Debug.Log("All spawn slots are busy. Spawn is failed.");
                return;
            }

            var spawnPosition = _spawnedObjectsHolder.transform.position + slot.RelativePosition;
            var instantiatedObject = Instantiate(_spawnedBody, spawnPosition, Quaternion.identity, _spawnedObjectsHolder.transform) as MovableBody;
            instantiatedObject.Init(spawnedBodySettings);
            slot.Put(instantiatedObject);

            var movableBodyTap = instantiatedObject.GetComponent<MovableBodyTap>();
            movableBodyTap.Init(_spawnedObjectsHolder);
            
            _awardHandler.AddAndSubscribeMovableBodyTap(movableBodyTap);

            movableBodyTap.ParticleRequested += OnParticleRequested;
            movableBodyTap.MovableDestroyed += OnMovableDestroyed;
        }

        private void OnMovableDestroyed(MovableBody movableBody)
        {
            var movableBodyTap = movableBody.GetComponent<MovableBodyTap>();
            movableBodyTap.ParticleRequested -= OnParticleRequested;
            movableBodyTap.MovableDestroyed -= OnMovableDestroyed;
        }

        private void OnParticleRequested(Vector3 position, Color color)
        {
            SpawnParticle(position, color);
        }

        private void RefreshSlotsState()
        {
            for (var row = 0; row < _spawnPoints.Count; row++)
            {
                var rowSlots = _spawnPoints[row];
                
                for (int i = 0; i < rowSlots.Count; i++)
                {
                    if(rowSlots[i].IsFree == false)
                        if(rowSlots[i].CanClear())
                            rowSlots[i].Clear();
                }
            }
        }

        private SpawnSlot FindFreeRandomSpawnSlot()
        {
            for (var row = 0; row < _spawnPoints.Count; row++)
            {
                var rowSlots = _spawnPoints[row];
                var freeSlots = new List<SpawnSlot>();
                
                for (int i = 0; i < rowSlots.Count; i++)
                {
                    if(rowSlots[i].IsFree)
                        freeSlots.Add(rowSlots[i]);
                }

                if (freeSlots.Count > 0)
                {
                    return freeSlots[UnityEngine.Random.Range(0, freeSlots.Count - 1)];
                }
            }

            return null;
        }

        private void SpawnParticle(Vector3 position, Color color)
        {
            var particle = Instantiate(_tapParticle, position, Quaternion.identity);
            particle.Init(color, _particleGroundPlane);
        }
        
        private void InstantiateDebugObjects()
        {
            for (int row = 0; row < _spawnPoints.Count; row++)
            {
                var rowSlots = _spawnPoints[row];
                for (int i = 0; i < rowSlots.Count; i++)
                {
                    var spawnPosition = _spawnedObjectsHolder.transform.position + rowSlots[i].RelativePosition;
                    var instantiatedObject = Instantiate(_debugObj, spawnPosition, Quaternion.identity, _spawnedObjectsHolder.transform);
                }
            }
        }
    }
}
