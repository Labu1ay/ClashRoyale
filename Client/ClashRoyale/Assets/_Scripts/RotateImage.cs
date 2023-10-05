using System;
using UnityEngine;

namespace _Scripts {
    public class RotateImage : MonoBehaviour {
        [SerializeField] private float _speed;

        private void Update() {
            transform.Rotate(0,0, _speed * Time.deltaTime);
        }
    }
}