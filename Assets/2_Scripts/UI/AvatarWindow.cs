using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarWindow : MonoBehaviour
{
    UserInfo _userInfo;

    public void OpenWindow()
    {
        gameObject.SetActive(true);
    }

    public void CloseWnd()
    {
        gameObject.SetActive(false);
    }
}
