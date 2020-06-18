using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StickObject : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    bool _isAim = false;
    Image _bg;
    Image _stick;
    Vector3 _dirInput;

    Player _ownerPlayer;

    public bool _isAimMotion { get { return _isAim; } }

    public Vector3 _direction { get { return _dirInput.normalized; } }

    public Vector3 _dirMov { get { return _dirInput.magnitude > 1.0f ? _dirInput.normalized : _dirInput; } }

    void Start()
    {
        _bg = GetComponent<Image>();
        _stick = transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        
    }

    public void SetOwnerPlayer(Player p)
    {
        _ownerPlayer = p;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_bg.rectTransform,eventData.position,eventData.pressEventCamera,out pos))
        {
            pos.x /= _bg.rectTransform.sizeDelta.x;
            pos.y /= _bg.rectTransform.sizeDelta.y;

            _dirInput = new Vector3(pos.y, 0, -pos.x);
            Vector3 dir = pos.magnitude > 1.0f ? pos.normalized : pos;

            _stick.rectTransform.anchoredPosition = new Vector3(dir.x * _bg.rectTransform.sizeDelta.x / 3, dir.y * _bg.rectTransform.sizeDelta.y / 3, 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isAim = true;
        _stick.color = new Color(0.5f, 0, 0, 0.5f);
        _ownerPlayer.InitializeDirection();
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isAim = false;
        _stick.color = new Color(1.0f, 0, 0, 0.5f);
        _dirInput = Vector3.zero;
        _stick.rectTransform.anchoredPosition = _dirInput;
    }
}
