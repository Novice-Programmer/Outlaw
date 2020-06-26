using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [SerializeField] int[] _cameraSize = new int[3] { 30, 45, 60 };
    [SerializeField] Vector3 _offSet = Vector3.zero;
    [SerializeField] Text _txtMapName = null;
    Camera _camera;
    GameObject _playerObj;
    List<Marker> _markers = new List<Marker>();
    int _curSize = 1;
    int _minSize = 0;
    int _maxSize = 2;

    float _markSize
    {
        get { return (_curSize + 1) / 2.0f; }
    }

    static MinimapController _uniqueInstance;

    public static MinimapController Instance
    {
        get { return _uniqueInstance; }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        _uniqueInstance = this;
        _camera = GetComponent<Camera>();
    }
    void Start()
    {
        GetMarker();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerObj != null)
        {
            Vector3 target = _playerObj.transform.position + _offSet;
            Vector3 movePos = transform.position;
            if (target.x < 205 && target.x > -105)
                movePos.x = target.x;
            if (target.z > -55.0f && target.z < 165)
                movePos.z = target.z;
            transform.position = movePos;
        }
    }

    private void LateUpdate()
    {
        for(int i=0;i< _markers.Count; i++)
        {
            if(_markers[i] == null)
            {
                _markers.RemoveAt(i);
                break;
            }
        }
    }

    void CameraUpdate()
    {
        _camera.orthographicSize = _cameraSize[_curSize];
        MarkerChange();
    }

    void GetMarker()
    {
        GameObject[] markerObjs = GameObject.FindGameObjectsWithTag("Marker");
        for(int i = 0; i < markerObjs.Length; i++)
        {
            Marker sr = markerObjs[i].GetComponent<Marker>();
            _markers.Add(sr);
        }
    }

    void MarkerChange()
    {
        for(int i = 0; i < _markers.Count; i++)
        {
            _markers[i].ScaleSetting(_markSize);
        }
    }

    public void AddMarker(Marker marker)
    {
        marker.ScaleSetting(_markSize);
        _markers.Add(marker);
    }

    public void RemoveMarker(Marker marker)
    {
        _markers.Remove(marker);
    }

    public void InitSetData(string mapName)
    {
        _txtMapName.text = mapName;
    }

    public void InitMarker()
    {
        GameObject[] brokenObj = GameObject.FindGameObjectsWithTag("BrokenObject");
        for(int i = 0; i < brokenObj.Length; i++)
        {
            GameObject markerObj = Resources.Load("Prefabs/Objects/BrokenMarker") as GameObject;
            GameObject addmarker = Instantiate(markerObj, brokenObj[i].transform);
            Marker marker = addmarker.GetComponent<Marker>();
            _markers.Add(marker);
        }
    }

    public void UpScaleClick()
    {
        if (_curSize == _minSize)
            return;
        _curSize--;
        CameraUpdate();
    }

    public void DownScaleClick()
    {
        if (_curSize == _maxSize)
            return;
        _curSize++;
        CameraUpdate();
    }

    public void InitPlayer()
    {
        _playerObj = GameObject.FindGameObjectWithTag("Player");
    }
}
