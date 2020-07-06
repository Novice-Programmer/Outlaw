using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw
{
    public class ShowTextUI : MonoBehaviour
    {
        [SerializeField] Text _goods = null;
        // Start is called before the first frame update

        public void SetGoodsValue(double val)
        {
            if (val >= 10000)
            {
                _goods.text = string.Format("{0:#,###}", (long)(val / 10000)) + "만";
            }
            else
            {
                _goods.text = string.Format("{0:#,###.##}", val);
            }
        }
    }
}