using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject target; //ī�޶� ���� ���
    public float moveSpeed; //ī�޶� �󸶳� ���� �ӵ��� �i����
    private Vector3 targetPosition; //����� ���� ��ġ��

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() // �� �����Ӹ��� ���󰡾��ؼ� ������
    {
        if(target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z); // this�� �� ��ũ��Ʈ�� ����� ��ü�� ���� ���� �� this�� ī�޶� ����Ŵ(��������)
            //this�� target ��� ���� ������ ĳ���Ϳ� ī�޶��� z���� ��ġ�� �Ⱥ����� ī�޶� ��󺸴� �ڿ� �־���ؼ� ���� ī�޶��� -10�� ��ӽ���ؼ��ε�

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime); //Lerp�� ���� A���� ���� B���� t�� �ӵ��� �����̰� �ϴ°�, �� ���� ��ġ���� ��� ��ġ����, Time.deltaTime�� moveSpeed �տ� ���Ͽ�����ϸ� 1�ʿ� moveSpeed��ŭ �̵��Ѵٴµ�. �� �ڼ��Ѱ� 4������ �����ô�.
        }
    }
}
