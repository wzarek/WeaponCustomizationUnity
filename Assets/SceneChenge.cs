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
        private GameObject _weaponObject;
        private GameObject _resetViewTextUIObject;
        private GameObject _customizeMenuUIObject;

        // rotation mouse
        private float _sensitivity;
        private Vector3 _mouseReference;
        private Vector3 _mouseOffset;
        private Vector3 _rotation;
        private bool _isRotating;
        private bool _isCustomizeMenuActive;

        void Start()
        {
            _weaponObject = GameObject.Find("HK416");
            _resetViewTextUIObject = GameObject.Find("ModifySceneInputHelpers");
            _customizeMenuUIObject = GameObject.Find("CustomizeView");

            _resetViewTextUIObject.SetActive(false);
            _customizeMenuUIObject.SetActive(false);

            _defaultPosition = _weaponObject.transform.position;
            _defaultRotation = _weaponObject.transform.rotation;

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
            if (!_isCustomizeMenuActive && Input.GetKeyDown(KeyCode.C))
            {
                _sceneState = _sceneState == SceneState.Modify ? SceneState.Inspect : SceneState.Modify;
                _animTime = 0;

                _resetViewTextUIObject.SetActive(_sceneState == SceneState.Modify);

                ChangeWeaponTransformation();

                Debug.Log($"--- SCENE CHANGED TO {Helpers.GetDescription(_sceneState)} ---");
            }
        }

        private void CheckInspectInputs()
        {

        }

        private void CheckModifyInputs()
        {
            if (!_isCustomizeMenuActive && Input.GetButtonDown("Fire3"))
            {
                CheckMouseDown();
            }

            if (!_isCustomizeMenuActive && Input.GetButtonUp("Fire3"))
            {
                CheckMouseUp();
            }

            if (_isRotating)
            {
                _mouseOffset = (Input.mousePosition - _mouseReference);

                _rotation.y = -(_mouseOffset.x) * _sensitivity;

                _rotation.x = -(_mouseOffset.y - _mouseOffset.x) * _sensitivity / 12;

                _rotation.z = -(_mouseOffset.y) * _sensitivity;

                _weaponObject.transform.Rotate(_rotation);

                _mouseReference = Input.mousePosition;
            }

            if (!_isRotating)
            {
                if (!_isCustomizeMenuActive && Input.GetKeyDown(KeyCode.R))
                {
                    ChangeWeaponTransformation();
                }

                if (Input.GetKeyDown(KeyCode.M))
                {
                    ToggleCustomizeMenu();
                }
            }
        }

        private void ToggleCustomizeMenu()
        {
            if (!_isCustomizeMenuActive)
            {
                ChangeWeaponTransformation();
                _customizeMenuUIObject.SetActive(true);
                _isCustomizeMenuActive = true;
            }
            else
            {
                _customizeMenuUIObject.SetActive(false);
                _isCustomizeMenuActive = false;
            }
        }

        private void ChangeWeaponTransformation()
        {
            if (_sceneState == SceneState.Inspect)
            {
                _weaponObject.transform.position = Vector3.Lerp(_weaponObject.transform.position, _defaultPosition, _animTime);
                _weaponObject.transform.rotation = Quaternion.Lerp(_weaponObject.transform.rotation, _defaultRotation, _animTime);
            }
            else if (_sceneState == SceneState.Modify)
            {
                _weaponObject.transform.position = Vector3.Lerp(_weaponObject.transform.position, _modifyPosition, _animTime);
                _weaponObject.transform.rotation = Quaternion.Lerp(_weaponObject.transform.rotation, _modifyRotation, _animTime);
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