using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    public class SceneChenge : MonoBehaviour
    {
        // states
        private SceneState _sceneState = SceneState.Inspect;
        private AnimationState _animationState = AnimationState.Finished;

        private float _animTime = 0;

        // positions
        private Vector3 _defaultPosition;
        private Quaternion _defaultRotation;
        private Vector3 _modifyPosition;
        private Quaternion _modifyRotation;

        // objects
        [SerializeField] public GameObject WeaponObject;
        [SerializeField] public GameObject ResetViewTextUIObject;
        [SerializeField] public GameObject CustomizeMenuUIObject;
        [SerializeField] public GameObject StickerMenuUIObject;

        // rotation mouse
        private float _sensitivity;
        private Vector3 _mouseReference;
        private Vector3 _mouseOffset;
        private Vector3 _rotation;
        private bool _isRotating;
        private bool _isCustomizeMenuActive;
        private bool _isStickerMenuActive;

        void Start()
        {
            ResetViewTextUIObject.SetActive(false);
            CustomizeMenuUIObject.SetActive(false);
            StickerMenuUIObject.SetActive(false);

            _defaultPosition = WeaponObject.transform.position;
            _defaultRotation = WeaponObject.transform.rotation;

            _modifyPosition = _defaultPosition + new Vector3(-1.2f, 0.5f, 4);
            _modifyRotation = Quaternion.Euler(_defaultRotation.eulerAngles + new Vector3(0, -90, 0));

            _sensitivity = 0.4f;
            _rotation = Vector3.zero;

            ChangeWeaponTransformation();
        }

        void Update()
        {

            if (_isRotating)
            {
                CheckModifyInputs();
                return;
            }

            CheckChangeStateInput();

            if (_animationState == AnimationState.Changing)
            {
                ChangeWeaponTransformation();
                return;
            }

            if (_sceneState == SceneState.Inspect)
            {
                CheckInspectInputs();
            }
            else
            {
                CheckModifyInputs();
            }
        }

        void CheckMouseDown()
        {
            _isRotating = true;
            _mouseReference = Input.mousePosition;
        }

        void CheckMouseUp()
        {
            _isRotating = false;
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            Debug.Log(name + " Game Object Clicked!");
        }

        private void CheckChangeStateInput()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                _sceneState = _sceneState == SceneState.Modify ? SceneState.Inspect : SceneState.Modify;
                _animTime = 0;

                ResetViewTextUIObject.SetActive(_sceneState == SceneState.Modify);

                CustomizeMenuUIObject.SetActive(false);
                _isCustomizeMenuActive = false;

                ChangeWeaponTransformation();

                Debug.Log($"--- SCENE CHANGED TO {Helpers.GetDescription(_sceneState)} ---");
            }
        }

        private void CheckInspectInputs()
        {

        }

        private void CheckModifyInputs()
        {
            if (Input.GetButtonDown("Fire3"))
            {
                CheckMouseDown();
            }

            if (Input.GetButtonUp("Fire3"))
            {
                CheckMouseUp();
            }

            if (_isRotating)
            {
                _mouseOffset = (Input.mousePosition - _mouseReference);

                _rotation.y = (_mouseOffset.x) * _sensitivity;

                _rotation.x = (_mouseOffset.y - _mouseOffset.x) * _sensitivity / 12;

                _rotation.z = (_mouseOffset.y) * _sensitivity;

                WeaponObject.transform.Rotate(_rotation);

                _mouseReference = Input.mousePosition;
            }

            if (!_isRotating)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    ChangeWeaponTransformation();
                }

                if (Input.GetKeyDown(KeyCode.M))
                {
                    ToggleCustomizeMenu();

                    if (_isStickerMenuActive)
                    {
                        ToggleStickerMenu();
                    }
                }

                if (Input.GetKeyDown(KeyCode.T))
                {
                    ToggleStickerMenu();

                    if (_isCustomizeMenuActive)
                    {
                        ToggleCustomizeMenu();
                    }
                }
            }
        }

        private void ToggleCustomizeMenu()
        {
            if (!_isCustomizeMenuActive)
            {
                ChangeWeaponTransformation();
                CustomizeMenuUIObject.SetActive(true);
                _isCustomizeMenuActive = true;
            }
            else
            {
                CustomizeMenuUIObject.SetActive(false);
                _isCustomizeMenuActive = false;
            }
        }

        private void ToggleStickerMenu()
        {
            if (!_isStickerMenuActive)
            {
                ChangeWeaponTransformation();
                StickerMenuUIObject.SetActive(true);
                _isStickerMenuActive = true;
            }
            else
            {
                StickerMenuUIObject.SetActive(false);
                _isStickerMenuActive = false;
            }
        }

        private void ChangeWeaponTransformation()
        {
            if (_sceneState == SceneState.Inspect)
            {
                WeaponObject.transform.position = Vector3.Lerp(WeaponObject.transform.position, _defaultPosition, _animTime);
                WeaponObject.transform.rotation = Quaternion.Lerp(WeaponObject.transform.rotation, _defaultRotation, _animTime);
            }
            else if (_sceneState == SceneState.Modify)
            {
                WeaponObject.transform.position = Vector3.Lerp(WeaponObject.transform.position, _modifyPosition, _animTime);
                WeaponObject.transform.rotation = Quaternion.Lerp(WeaponObject.transform.rotation, _modifyRotation, _animTime);
            }

            CheckAndChangeAnimationState();
        }

        private void CheckAndChangeAnimationState()
        {
            _animationState = AnimationState.Changing;

            if (_animTime >= 1)
            {
                _animationState = AnimationState.Finished;
                _animTime = 0;
            }
            else
            {
                _animTime += Time.deltaTime;
            }
        }
    }
}