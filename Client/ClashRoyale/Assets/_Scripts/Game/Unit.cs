using System;
using UnityEngine;

[RequireComponent(typeof(UnitParameters), typeof(Health), typeof(UnitAnimation))]
public class Unit : MonoBehaviour, IHealth, IDestroyed {
   public event Action Destroyed;
   [field: SerializeField] public Health Health { get; private set; }
   [field: SerializeField] public bool _isEnemy { get; private set; } = false;
   [field: SerializeField] public UnitParameters Parameters;
   [SerializeField] private UnitAnimation _animation;

   [SerializeField] private UnitState _defaultStateSO;
   [SerializeField] private UnitState _chaseStateSO;
   [SerializeField] private UnitState _attackStateSO;
   private UnitState _defaultState;
   private UnitState _chaseState;
   private UnitState _attackState;

   private UnitState _currentState;


   public void Init(bool isEnemy) {
      _isEnemy = isEnemy;
      
      _animation.Init(this);
      CreateStates();

      _currentState = _defaultState;
      _currentState.Init();

      Health.UpdateHealth += CheckDestroy;
   }

   private void CreateStates() {
      _defaultState = Instantiate(_defaultStateSO);
      _defaultState.Constructor(this);

      _chaseState = Instantiate(_chaseStateSO);
      _chaseState.Constructor(this);

      _attackState = Instantiate(_attackStateSO);
      _attackState.Constructor(this);
   }

   private void Update() {
      _currentState.Run();
   }

   private void CheckDestroy(float currentHealth) {
      if(currentHealth > 0) return;
      Health.UpdateHealth -= CheckDestroy;
      Destroy(gameObject);
      
      Destroyed?.Invoke();
   }

   public void SetState(UnitStateType type) {
      _currentState.Finish();
      
      switch (type) {
         case UnitStateType.Default :
            _currentState = _defaultState;
            break;
         case UnitStateType.Chase : 
            _currentState = _chaseState;
            break;
         case UnitStateType.Attack :
            _currentState = _attackState;
            break;
         default: Debug.LogError("Не обрабатывается состояние " + type); break;
      }
      
      _currentState.Init(); 
      _animation.SetState(type);
   }
   
#if UNITY_EDITOR

   [Space(24)]
   [SerializeField] private bool _debug = false;

   private void OnDrawGizmos() {
      if(_debug == false) return;
      if(_chaseStateSO != null) _chaseStateSO.DebugDrawDistance(this);
   }
#endif
}