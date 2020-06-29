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
    GameObject _prefabBrokenMarker;
    GameObject _prefabAnimalMarker;
    List<Marker> _markers = new List<Marker>();
    int _curSize = 1;
    int _minSize = 0;
    int _maxSize = 2;

    float _markSize
    {
        get
        {
            float size = 1.0f;
            switch (_curSize)
            {
                case 0:
                    size = 1.1f;
                    break;
                case 2:
                    size = 0.8f;
                    break;
            }
            return size; 
        }
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
        _prefabBrokenMarker = Resources.Load("Prefabs/Objects/BrokenMarker") as GameObject;
        _prefabAnimalMarker = Resources.Load("Prefabs/Objects/AnimalMarker") as GameObject;
        GetMarker();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerObj != null)
        {
            Vector3 target = _playerObj.transform.position + _offSet;
            Vector3 movePos = transform.position;
            int addSize = 15 * Mathf.Abs(_maxSize - _curSize);
            int xMinRange = -90 - addSize;
            int xMaxRange = 190 + addSize;
            int zMinRange = -45 - addSize;
            int zMaxRange = 150 + addSize;
            if (target.x >= xMinRange && target.x < xMaxRange)
                movePos.x = target.x;
            else if (target.x >= xMaxRange)
                movePos.x = xMaxRange;
            else
                movePos.x = xMinRange;
            
            if (target.z >= zMinRange && target.z < zMaxRange)
                movePos.z = target.z;
            else if (target.z >= zMaxRange)
                movePos.z = zMaxRange;
            else
                movePos.z = zMinRange;
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
            GameObject addMarker = Instantiate(_prefabBrokenMarker, brokenObj[i].transform);
            Marker marker = addMarker.GetComponent<Marker>();
            marker._sizeScale = brokenObj[i].GetComponent<BrokenObject>()._sizeMarker;
            _markers.Add(marker);
        }

        Animal[] animalObj = FindObjectsOfType<Animal>();
        for(int i = 0; i < animalObj.Length; i++)
        {
            GameObject addMarker = Instantiate(_prefabAnimalMarker, animalObj[i].transform);
            Marker marker = addMarker.GetComponent<Marker>();
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
