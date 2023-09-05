using UnityEngine;
[RequireComponent(typeof(UnitParameters))]
public class Unit : MonoBehaviour {
   [field: SerializeField] public bool _isEnemy { get; private set; } = false;
   [field: SerializeField] public UnitParameters Parameters;

   [SerializeField] private UnitState _defaultStateSO;
   [SerializeField] private UnitState _chaseStateSO;
   [SerializeField] private UnitState _attackStateSO;
   private UnitState _defaultState;
   private UnitState _chaseState;
   private UnitState _attackState;

   private UnitState _currentState;

   private void Start() {
      _defaultState = Instantiate(_defaultStateSO);
      _defaultState.Constructor(this);
      
      _chaseState = Instantiate(_chaseStateSO);
      _chaseState.Constructor(this);
      
      _attackState = Instantiate(_attackStateSO);
      _attackState.Constructor(this);

      _currentState = _defaultState;
      _currentState.Init();
   }

   private void Update() {
      _currentState.Run();
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
   }
}