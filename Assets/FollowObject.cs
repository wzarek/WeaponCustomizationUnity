using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class FollowObject : MonoBehaviour
    {
        public enum Positioning
        {
            LeftTop,
            RightTop,
            Top,
            Bottom,
            LeftBottom,
            RightBottom
        }

        [SerializeField] public Transform lookAt;
        [SerializeField] public Positioning pos;

        private Camera cam;

        private Vector3 offset;

        void Start()
        {
            cam = Camera.main;
            SetOffset();
        }

        void SetOffset()
        {
            switch (pos)
            {
                case Positioning.LeftTop:
                    offset = new Vector3(0.35f, 0.05f, 0);
                    break;
                case Positioning.RightTop:
                    offset = new Vector3(-0.6f, -0.25f, 0);
                    break;
                case Positioning.LeftBottom:
                    offset = new Vector3(-0.25f, -0.65f, 0);
                    break;
                case Positioning.RightBottom:
                    offset = new Vector3(1.05f, -0.45f, 0);
                    break;
            }
        }

        void Update()
        {
            Vector3 posToChange = cam.WorldToScreenPoint(lookAt.position + offset);

            if (transform.position != posToChange)
            {
                transform.position = posToChange;
            }
        }
    }
}