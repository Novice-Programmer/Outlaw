using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    [SerializeField] Text _txtMessage = null;

    public void OpenMessageBox(string msg = "", bool isOpen = false)
    {
        gameObject.SetActive(isOpen);
        _txtMessage.text = msg;
    }
}
