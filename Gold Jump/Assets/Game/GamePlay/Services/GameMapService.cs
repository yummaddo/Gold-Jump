using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Core.Abstraction;
using UnityEngine;
using Zenject;

namespace Game.GamePlay.Services
{
    public class GameMapService : AbstractServices
    {
        [Inject] private DiContainer containerDi;
        public RectTransform root;
        public RectTransform uiRect;
        public GameObject winTriggerZone;
        public int lastHeightPanel = 0;
        public Transform rootOfCamera;
        public LevelScene levelScene;
        private int _currentIterator = 0;
        private float _delta = 0;
        private int _countOfEnemy = 0;
        private int _countOfEnemyKill = 0;
        public List<GameObject> panels = new List<GameObject>(); 
#if UNITY_EDITOR
        [ContextMenu("Generate")]
        public void LevelGenerationEditor()
        {
            LevelGeneration();
        }
#endif
        internal void KillEnemy() => _countOfEnemyKill++;
        internal bool KilledAll()
        {
            Debug.Log($"_countOfEnemyKill [{_countOfEnemyKill}] == _countOfEnemy [{_countOfEnemy}] result ");
            return _countOfEnemyKill == _countOfEnemy;
        }

        private async UniTask LevelPreGeneration()
        {
            var level = levelScene.data;
            float screenWidth = Camera.main!.orthographicSize * Screen.width / Screen.height;
            lastHeightPanel = 0;
            var pos = winTriggerZone.transform.position;
            pos.y = level.height;
            winTriggerZone.transform.position = pos;
            var deltaOfPreGenerator = level.preGenerateSize; 
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                if (lastHeightPanel < rootOfCamera.position.y + deltaOfPreGenerator)
                {
                    _currentIterator += IterationCreator(level,screenWidth);
                }
                
                await UniTask.Yield();
                
            }
        }
        private void  LevelGeneration()
        {
            var level = levelScene.data;
            float screenWidth = Camera.main!.orthographicSize * 2.0f * Screen.width / Screen.height;
            Vector3 startPosition = Vector3.zero;
            startPosition.y = _currentIterator * level.panelsIteratorSize;
            while (startPosition.y < level.height)
            {
                _currentIterator += IterationCreator(level,screenWidth);
                startPosition.y = _currentIterator;
            }
        }
        private int IterationCreator( Level level, float screenWidth )
        {
            if (level.height < _currentIterator) return 0;
            var valueOfPlatform = Random.Range(0,1f);
            var valueOfPlatformWidth = Random.Range(0,1f);
            var valueOfPlatformIs = Random.Range(0,1f);
            var valueOfIterator = Random.Range(0,1f);

            
            var chanceOfIterator = level.spawnPlatformNextIterationChance.Evaluate(valueOfIterator);
            var chanceOfPlatformIs = level.spawnPlatformChance.Evaluate(valueOfPlatformIs);
            var chanceOfPlatformWidth = level.chanceOfWidth.Evaluate(valueOfPlatformWidth);
            var chanceOfPlatformType = level.dispersionOfBasePlatformType.Evaluate(valueOfPlatform);

            if (chanceOfPlatformIs < 0)
                return 1;
            var iterationResultValue = (int)(chanceOfIterator * level.iterationBaseSize);
            var platformWidth = (screenWidth*-0.5f) + chanceOfPlatformWidth * screenWidth;
            var platformHeight = ( _delta+_currentIterator+iterationResultValue );
            var platformPosition = new Vector3(platformWidth, platformHeight,0);
            var platformType = level.platformListDictionary[chanceOfPlatformType];
            Create(platformType, platformPosition);
            if (_countOfEnemy < levelScene.maxEnemy && _currentIterator > levelScene.createHeightMinimal)
            {
                var enemy = Random.Range(0,1f);
                if (enemy < levelScene.percentSpawnEnemyByIteration)
                {
                    _countOfEnemy++;
                    CreateEnemy(levelScene.enemyPrefab,platformPosition);
                }
            }
            return iterationResultValue;
        }

        private void CreateEnemy(GameObject enemyPrefab, Vector3 position)
        {
            var obj = containerDi.InstantiatePrefab(enemyPrefab, root);
            var positionOfTheNewPanel = obj.transform.position;
            positionOfTheNewPanel += position;
            obj.transform.position = positionOfTheNewPanel;
            lastHeightPanel = (int)positionOfTheNewPanel.y;
        }

        private void Create(GameObject platformPrefab ,Vector3 position)
        {
            var obj = containerDi.InstantiatePrefab(platformPrefab, root);
            var positionOfTheNewPanel = obj.transform.position;
            positionOfTheNewPanel += position;
            obj.transform.position = positionOfTheNewPanel;
            lastHeightPanel = (int)positionOfTheNewPanel.y;
        }

        public override void OnAwake()
        {
            levelScene = ApplicationContext.Instance.LevelsSetting.currentSceneLevel;
        }
        public override void OnStart()
        {
            LevelPreGeneration().Forget();
        }
    }
}