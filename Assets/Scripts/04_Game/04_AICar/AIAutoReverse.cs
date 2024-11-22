using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAutoReverse : MonoBehaviour
{
    private float stoppedTime;

    private float moveAmount;

    // 閾値 (これを下回ったらboolがtrueになる)
    [SerializeField]
    private float movementThreshold = 0.1f;

    private Vector3 lastPosition;

    private Vector3 replacePosition;
    private Quaternion replaceRotation;

    private Rigidbody rigid;

    [SerializeField]
    private CountDown countDown;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        SetReplacePoint(this.gameObject.transform);
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CalcMoveAmount();
    }

    private void CalcMoveAmount()
    {
        if (countDown.isPlay)
        {
            float movementDistance = Vector3.Distance(transform.position, lastPosition);

            // 閾値以下の場合、isMovingSlowlyをtrueにする
            if (movementDistance < movementThreshold)
            {
                stoppedTime += Time.deltaTime;
            }
            else
            {
                stoppedTime = 0;
            }

            // 現在の位置を保存して次のフレームで使用
            lastPosition = transform.position;

            if (stoppedTime > 5)
            {
                Replace();
            }
        }
    }

    public void SetReplacePoint(Transform point)
    {
        replacePosition = point.position;
        replaceRotation = point.transform.rotation;
    }

    public void Replace()
    {
        rigid.velocity = Vector3.zero;
        transform.position = replacePosition;
        transform.rotation = replaceRotation;
    }
}
