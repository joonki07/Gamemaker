using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{

    private BoxCollider2D boxCollider;
    public LayerMask layerMask; // 통과 불가 레이어 설정하는 LayerMask

    public float speed; // 캐릭터 스피드 변수

    private Vector3 vector; //세개의 값을 동시에 가지고 있음 하나 선언으로 3개값(x,y,z)을 동시에 갖는 변수

    public float runSpeed; //그냥 뛰는 속도인듯
    private float applyRunSpeed; //쉬프트를 눌렀을때 실제 적용값
    private bool applyRunFlag = false; // 뛸때 텀 오기 전 속도 2배

    public int walkCount;
    private int currentWalkCount;
    // speed = 2.4, walkCount = 20
    //2.4 * 20 = 48 한번 방향키 눌릴때마다 48픽셀씩 움직임

    private bool canMove = true; // 코루틴 여러개 한꺼번에 실행되는거 막기용

    private Animator animator; // 그냥은 껍데기

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>(); // 컴포넌트 불러오기
        animator = GetComponent<Animator>(); // 껍데기에 캐릭터의 있는 Animator 컴포넌트 통제권 넣어주기
    }

    IEnumerator MoveCoroutine() //이 함수에서 실행하다가 코루틴 실행시 코루틴도 같이 실행됨. 다중처리느낌내줌, 대기명령어 가짐
    {
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) //  계속 코루틴 신호로 동작초기화되는걸 막는듯
        {
            if (Input.GetKey(KeyCode.LeftShift)) //왼쪽 쉬프트가 눌렸을경우
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z); // 백터에 x,y,z값을 저장. z는 차피 안바뀌어서 스크립트에 적용되는 개체의 z값을 개속적용

            if (vector.x != 0) //대충 좌우 움직일때 상하 못움직이도록 해서 대각선느낌의 입력이 됬을때 동작과 움직임 다름 방지
            {
                vector.y = 0;
            }

            animator.SetFloat("DirX", vector.x); // DirX에 위에 백터 셋에서 입력받은 Horizontal(좌,우 1,-1) 넣어주기
            animator.SetFloat("DirY", vector.y);

            RaycastHit2D hit;
            // A지점은 B지점으로 레이저 발싸!
            //도달 성공시 hit = Null;
            //방해물에 충돌해 실패시 hit = 방해물

            Vector2 start = transform.position; // A지점 캐릭터의 현재 위치값
            Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount); // B지점 캐릭터의 이동하고자 하는 위치값 , vector.x * speed * walkCount은 48픽셀 이동 + 현재 위치값

            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, layerMask); // 위의 RaycastHit2D hit; 에서 설명한게 이거. 근데 이거만쓰면 캐릭터가 스스로값을 가져서 스스로 충돌함
            boxCollider.enabled = true;

            if (hit.transform != null) // 레이저 쏴서 레이어마스크에 해당하는벽 있으면(hit에 방해물값 들어옴) 이후 내용 실행 안함  
            {
                break;
            }
            // 위에 if 통과는 레이저가 방해물(레이어마스크에 해당하는 벽)에 안부딪힌거니까 움직이는 담당의 아래 코드 실행

            animator.SetBool("Walking", true); // 멈춰있는 Standing Tree상태에서 Walking Tree 움직이는 상태로 상태전환일어남

            while (currentWalkCount < walkCount)//walkCount가 20일때 courrentWalkCount는 while문을 계속돌며 조건수치를 맞추고 끝나면 나오고 0으로 초기화함. 이러면 48픽셀 움직이고 while문이 끝나는듯
            {
                if (vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0); //translate는 현재 값에서 괄호안의 수치만큼 더해주는 역할


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
                yield return new WaitForSeconds(0.01f); //1초동안 대기해 움직임 간의 띄움주기 , 반복문 20번 실행되면 0.2초틈생김
            }
            currentWalkCount = 0;


        }
        animator.SetBool("Walking", false); //이동 끝났으니 다시 Standing Tree 상태로
        canMove = true; // while문 실행 끝나면 다시 방향키 누름 실행할 수 있도록
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) // 방향키 누르면 canMove false로 바꿈
        {
            //input.GetAxisRaw("Horizontal")은 우 방향키가 눌리면 1리턴 좌방향키가 눌리면 -1리턴 
            //눌려서 값이 리턴되면 0이 아니게 되어 실행\
            //Input.GetAxisRaw("Vertical")은 위 경우의 상,하 경우

            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false; // 코루틴 다중실행 방지용 다중실행 되면 축짓법쓰는듯;;
                StartCoroutine(MoveCoroutine()); // 코루틴 실행
            }
        }
    }
}
