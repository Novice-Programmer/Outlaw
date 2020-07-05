using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] GameObject _lookObj = null;

    CharacterController _controller;
    Animator _playerAnim;
    Button _actionBtn;
    Text _actionTxt;
    LobbyStick _stickMovement;

    LobbyObject _selectObject;

    public Transform _lookTr
    {
        get { return _lookObj.transform; }
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _playerAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("MoveStick");
        _stickMovement = go.GetComponent<LobbyStick>();
        go = GameObject.FindGameObjectWithTag("ActionButton");
        _actionBtn = go.GetComponent<Button>();
        _actionTxt = _actionBtn.transform.GetChild(0).GetComponent<Text>();
        _actionBtn.interactable = false;
    }


    void Update()
    {
        float mx = -_stickMovement._dirMove.x;
        float mz = -_stickMovement._dirMove.y;
#if UNITY_EDITOR
        float rotate = Input.GetAxis("Horizontal");
        float move = Input.GetAxis("Vertical");



        Vector3 mv = Vector3.zero;
        if (move != 0)
            mv = transform.forward * move * 5.0f * Time.deltaTime;
        else
            mv = transform.forward * mz * 3.0f * Time.deltaTime;

        if (rotate!=0)
            transform.Rotate(Vector3.up * rotate * Time.deltaTime * 180);
        else
            transform.Rotate(Vector3.up * mx * Time.deltaTime * 180);
#else
        mv = transform.forward * mz * 3.0f * Time.deltaTime;
        transform.Rotate(Vector3.up * mx * Time.deltaTime * 180);
#endif

        AnimChange(mv);

        _controller.Move(mv);
    }

    void AnimChange(Vector3 mv)
    {
        if (mv != Vector3.zero)
            _playerAnim.SetBool("IsWalk", true);
        else
            _playerAnim.SetBool("IsWalk", false);
    }

    public void SelectChange(string actionMsg = "Action", LobbyObject lbObj = null)
    {
        if (lbObj != null)
        {
            _selectObject = lbObj;
            _actionBtn.interactable = true;
        }
        else
        {
            _selectObject = null;
            _actionBtn.interactable = false;
        }
        _actionTxt.text = actionMsg;
    }

    public void Select()
    {
        if (_selectObject != null)
        {
            _selectObject.Select();
        }
        SelectChange();
    }

    public void Fire()
    {

    }
}
