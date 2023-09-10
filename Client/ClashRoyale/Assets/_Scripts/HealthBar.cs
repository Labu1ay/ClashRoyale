using UnityEngine;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Transform _foreground;
    
    public void UpdateHealth(float current, float max){
        float percent = current / max;
        _foreground.localScale = new Vector3(Mathf.Clamp01(percent), _foreground.localScale.y, _foreground.localScale.z);
    }
}