using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    private BoxCollider2D boxCollider;
    public LayerMask layerMask; // ��� �Ұ� ���̾� �����ϴ� LayerMask

    public float speed; // ĳ���� ���ǵ� ����

    private Vector3 vector; //������ ���� ���ÿ� ������ ���� �ϳ� �������� 3����(x,y,z)�� ���ÿ� ���� ����

    public float runSpeed; //�׳� �ٴ� �ӵ��ε�
    private float applyRunSpeed; //����Ʈ�� �������� ���� ���밪
    private bool applyRunFlag = false; // �۶� �� ���� �� �ӵ� 2��

    public int walkCount;
    private int currentWalkCount;
    // speed = 2.4, walkCount = 20
    //2.4 * 20 = 48 �ѹ� ����Ű ���������� 48�ȼ��� ������

    private bool canMove = true; // �ڷ�ƾ ������ �Ѳ����� ����Ǵ°� �����

    private Animator animator; // �׳��� ������

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>(); // ������Ʈ �ҷ�����
        animator = GetComponent<Animator>(); // �����⿡ ĳ������ �ִ� Animator ������Ʈ ������ �־��ֱ�
    }

    IEnumerator MoveCoroutine() //�� �Լ����� �����ϴٰ� �ڷ�ƾ ����� �ڷ�ƾ�� ���� �����. ����ó����������, ����ɾ� ����
    {
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) //  ��� �ڷ�ƾ ��ȣ�� �����ʱ�ȭ�Ǵ°� ���µ�
        {
            if (Input.GetKey(KeyCode.LeftShift)) //���� ����Ʈ�� ���������
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z); // ���Ϳ� x,y,z���� ����. z�� ���� �ȹٲ� ��ũ��Ʈ�� ����Ǵ� ��ü�� z���� ��������

            if (vector.x != 0) //���� �¿� �����϶� ���� �������̵��� �ؼ� �밢�������� �Է��� ������ ���۰� ������ �ٸ� ����
            {
                vector.y = 0;
            }

            animator.SetFloat("DirX", vector.x); // DirX�� ���� ���� �¿��� �Է¹��� Horizontal(��,�� 1,-1) �־��ֱ�
            animator.SetFloat("DirY", vector.y);

            RaycastHit2D hit;
            // A������ B�������� ������ �߽�!
            //���� ������ hit = Null;
            //���ع��� �浹�� ���н� hit = ���ع�

            Vector2 start = transform.position; // A���� ĳ������ ���� ��ġ��
            Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount); // B���� ĳ������ �̵��ϰ��� �ϴ� ��ġ�� , vector.x * speed * walkCount�� 48�ȼ� �̵� + ���� ��ġ��

            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, layerMask); // ���� RaycastHit2D hit; ���� �����Ѱ� �̰�. �ٵ� �̰Ÿ����� ĳ���Ͱ� �����ΰ��� ������ ������ �浹��
            boxCollider.enabled = true;

            if (hit.transform != null) // ������ ���� ���̾��ũ�� �ش��ϴº� ������(hit�� ���ع��� ����) ���� ���� ���� ����  
            {
                break;
            }
            // ���� if ����� �������� ���ع�(���̾��ũ�� �ش��ϴ� ��)�� �Ⱥε����Ŵϱ� �����̴� ����� �Ʒ� �ڵ� ����

            animator.SetBool("Walking", true); // �����ִ� Standing Tree���¿��� Walking Tree �����̴� ���·� ������ȯ�Ͼ

            while (currentWalkCount < walkCount)//walkCount�� 20�϶� courrentWalkCount�� while���� ��ӵ��� ���Ǽ�ġ�� ���߰� ������ ������ 0���� �ʱ�ȭ��. �̷��� 48�ȼ� �����̰� while���� �����µ�
            {
                if (vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0); //translate�� ���� ������ ��ȣ���� ��ġ��ŭ �����ִ� ����


                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }
                if (applyRunFlag)
                {
                    currentWalkCount++;
                }
                currentWalkCount++;
                yield return new WaitForSeconds(0.01f); //1�ʵ��� ����� ������ ���� ����ֱ� , �ݺ��� 20�� ����Ǹ� 0.2��ƴ����
            }
            currentWalkCount = 0;


        }
        animator.SetBool("Walking", false); //�̵� �������� �ٽ� Standing Tree ���·�
        canMove = true; // while�� ���� ������ �ٽ� ����Ű ���� ������ �� �ֵ���
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) // ����Ű ������ canMove false�� �ٲ�
        {
            //input.GetAxisRaw("Horizontal")�� �� ����Ű�� ������ 1���� �¹���Ű�� ������ -1���� 
            //������ ���� ���ϵǸ� 0�� �ƴϰ� �Ǿ� ����\
            //Input.GetAxisRaw("Vertical")�� �� ����� ��,�� ���

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false; // �ڷ�ƾ ���߽��� ������ ���߽��� �Ǹ� ���������µ�;;
                StartCoroutine(MoveCoroutine()); // �ڷ�ƾ ����
            }
        }
    }
}
