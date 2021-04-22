using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts
{
    public class PlayerShip : MonoBehaviour
    {
        #region Variables
        
        [field: SerializeField, 
                Tooltip("Distance from the center for player to stop")] 
        private float horizontalBoundDistance = 3f;

        [field: SerializeField, 
                Tooltip("Speed of the ship")] 
        private float movementSpeed = .005f;

        [field: SerializeField, 
                Tooltip("Point where bullet spawn")] 
        private Transform cannonShip;

        [field: SerializeField, 
                Tooltip("Bullet prefab")] 
        private GameObject bulletPrefab;

        private Transform _transform;

        private bool _lockShooting;
        
        /// <summary>
        /// New input system generated class
        /// </summary>
        private PlayerActions _playerActions;
        
        /// <summary>
        /// Current value of horizontal movement
        /// </summary>
        private float _currentMoveValue;

        public Action gameOverEvent;
        
        #endregion

        private void Start()
        {
            _transform = transform;
            EnableControls();
            StartCoroutine(MoveShip());
        }

        private void OnDestroy()
        {
            DisableControls();
        }

        #region ControlHandlers

        private void EnableControls()
        {
            _playerActions = new PlayerActions();
            
            _playerActions.PlayerControl.Movement.performed += MoveShipHandler;
            _playerActions.PlayerControl.Movement.canceled += StopShipHandler;
            _playerActions.PlayerControl.Shoot.performed += ShootBulletHandler;
            _playerActions.Enable();
        }

        private void DisableControls()
        {
            _playerActions.Disable();
            _playerActions.PlayerControl.Movement.performed -= MoveShipHandler;
            _playerActions.PlayerControl.Movement.canceled -= StopShipHandler;
            _playerActions.PlayerControl.Shoot.performed -= ShootBulletHandler;
        }

        private void MoveShipHandler(InputAction.CallbackContext obj)
        {
            _currentMoveValue = obj.ReadValue<float>();
        }
        
        private void StopShipHandler(InputAction.CallbackContext obj)
        {
            _currentMoveValue = 0;
        }

        private void ShootBulletHandler(InputAction.CallbackContext obj)
        {
            if (_lockShooting) return;
            
            var bullet = Instantiate(bulletPrefab, cannonShip.position, cannonShip.rotation);
            bullet.GetComponent<BulletPlayer>().AllowToShoot = AllowToShoot;
            _lockShooting = true;
        }

        #endregion

        private void AllowToShoot()
        {
            _lockShooting = false;
        }

        public void DestroyShip()
        {
            gameOverEvent();
            Destroy(gameObject);
        }

        private IEnumerator MoveShip()
        {
            while(true)
            {
                if (_transform.position.x >= horizontalBoundDistance && _currentMoveValue > 0 ||
                    _transform.position.x <= -horizontalBoundDistance && _currentMoveValue < 0)
                {
                    _currentMoveValue = 0;
                }
            
                _transform.position += Vector3.right * (movementSpeed * _currentMoveValue * Time.deltaTime);
            
                yield return new WaitForEndOfFrame();
            }

        }
    }
}
