using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject target; //카메라가 따라갈 대상
    public float moveSpeed; //카메라가 얼마나 빠른 속도로 쫒는지
    private Vector3 targetPosition; //대상의 현재 위치값

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() // 매 프레임마다 따라가야해서 였따슴
    {
        if(target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z); // this는 이 스크립트가 적용될 개체를 말함 따라서 이 this는 카메라를 가리킴(생략가능)
            //this를 target 대신 쓰는 이유는 캐릭터와 카메라의 z값이 겹치면 안보여서 카메라가 대상보다 뒤에 있어야해서 기존 카메라값의 -10을 계속써야해서인듯

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime); //Lerp는 벡터 A에서 벡터 B까지 t의 속도로 움직이게 하는것, 즉 현재 위치에서 대상 위치까지, Time.deltaTime은 moveSpeed 앞에 곱하여사용하며 1초에 moveSpeed만큼 이동한다는듯. 더 자세한건 4강에서 들읍시다.
        }
    }
}
