using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spinner
{
    public class SpinAround : MonoBehaviour
    {
        #region Spin Around

        public float degreesPerSec = 360f; 

        void Update()
        {
            if(gameObject.activeSelf)
            {
                float rotAmount = degreesPerSec * Time.deltaTime; 
                float curRot = transform.localRotation.eulerAngles.z; 
                transform.localRotation = Quaternion.Euler(new Vector3(0,0,curRot+rotAmount));
            }
        }

        #endregion
    }
}
