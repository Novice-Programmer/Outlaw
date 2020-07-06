﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Outlaw
{
    public class LobbyStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] Color _downColor = Color.green;
        [Range(4.0f, 2.0f)] [SerializeField] float _scrollSpeed = 3.0f;

        Image _bg;
        Image _stick;

        Vector3 _dirInput;

        Color _orizinColor;

        public Vector3 _dirMove
        {
            get
            {
                return new Vector3(_dirInput.z, -_dirInput.x, 0);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _bg = GetComponent<Image>();
            _stick = transform.GetChild(0).GetComponent<Image>();
            _orizinColor = _stick.color;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_bg.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
            {
                pos.x /= _bg.rectTransform.sizeDelta.x;
                pos.y /= _bg.rectTransform.sizeDelta.y;

                _dirInput = new Vector3(pos.y, 0, -pos.x);
                Vector3 dir = pos.magnitude > 1.0f ? pos.normalized : pos;
                _stick.rectTransform.anchoredPosition = new Vector3(dir.x * _bg.rectTransform.sizeDelta.x / _scrollSpeed, dir.y * _bg.rectTransform.sizeDelta.y / _scrollSpeed, 0);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _stick.color = _downColor;
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _stick.color = _orizinColor;
            _dirInput = Vector3.zero;
            _stick.rectTransform.anchoredPosition = _dirInput;
        }
    }
}