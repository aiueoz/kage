using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayeController2 : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpPower = 5f;
    float Speed;

   [Header("参照")]
    [SerializeField] Transform cameraTransform;
    public bool UDRevers = false;
    public bool RLRevers =false;
    int UD = 1;
    int RL = 1;

    [Header("カメラ関連")]
    [SerializeField] Transform cameraPos;

    //レイキャスト関連
    [Header("地面設置関連")]
    [SerializeField]  float groungRaylength;
    [SerializeField] LayerMask ground;
    [SerializeField] GameObject groundCollider;

    [Header("アニメーション関連")]
    [SerializeField] Animator anim;
    bool RunFlag = false;
    bool WalkFlag = false;
    bool WaitFlag=false;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        //カメラワークの反転処理
        if (UDRevers) UD = -1; else UD = 1;
        if(RLRevers) RL = -1;else RL = 1;
        //接地判定       
        GroundCheck();
       
        //カメラ操作
        CameraWork();    
    }
    void Anim()
    {  
       

        //if ((Speed <= 0.6f && Speed != 0) && !WalkFlag) {
        //    anim.CrossFade("Walk", 0.15f);
        //    WalkFlag = true;
        //}
        //else WalkFlag = true;

        //if (Speed == 0 && !WaitFlag)
        //{
        //    anim.CrossFade("Wait", 0.15f);
        //}
        //else WaitFlag = true;
        
    }
    private void FixedUpdate()
    {
        //移動
        Move();
        
        //ジャンプ関連
        Jump();
    }
    //レイキャスト関連
    void GroundCheck()
    { 
        isGrounded = FootController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 接地安定化
        }
       // Debug.Log(isGrounded);
    }
    //プレイヤーの移動関連
    private void Move()
    { 
        
        // 入力
        float horizontal = Input.GetAxis("Horizontal");      
        float vertical = Input.GetAxis("Vertical");
        //スティックがニュートラル時に動かないようにする
        if (horizontal < -0.12f && horizontal > 0.12f) horizontal = 0;
        if (vertical < -0.12f&& vertical > 0.12f) vertical = 0;
        Vector3 inputDir = new Vector3(horizontal, 0, vertical).normalized;
        //スティックの方向け具合で速度を変えるための処理
        if (horizontal < 0) horizontal *= -1;
        if (vertical < 0) vertical *= -1;
        Speed =vertical+horizontal;
        if (Speed >= 1)
        {
            Speed = 1;
        }
        if (Speed > 0.6f)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                anim.CrossFade("Run", 0.15f);

            }

        }
        else if (Speed <= 0.6f && Speed != 0)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                anim.CrossFade("Walk", 0.15f);

            }

        }
        else if (Speed == 0)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Wait"))

            {
                anim.CrossFade("Wait", 0.15f);

            }

        }


        Debug.Log(Speed);
       
        if (inputDir.magnitude >= 0.1f)
        {
            // カメラ基準の移動方向に変換
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            Anim();
            controller.Move(moveDir.normalized * moveSpeed *Speed* Time.deltaTime);
        }
    }
    //カメラワーク関連
    void CameraWork()
    {
        
        cameraPos.position = transform.position;

        //右スティック入力
        float cx = Input.GetAxisRaw("Vertical2");   // 上下
        if (cx < -0.12f && cx > 0.12f)cx = 0;
        float cy = Input.GetAxisRaw("Horizontal2"); // 左右
        if (cy < -0.12f &&cy > 0.12f) cy = 0;
        // --- 左右回転 ---
        cameraPos.RotateAround(transform.position, Vector3.up, RL*cy * 250 * Time.deltaTime);

        // --- 上下回転 ---
        float nextAngleX = cameraPos.eulerAngles.x - cx *UD*-1* 250 * Time.deltaTime;

        // UnityのEuler角は0〜360なので補正
        if (nextAngleX > 180) nextAngleX -= 360;

        // 制限（例: 上下45度ずつ）
        nextAngleX = Mathf.Clamp(nextAngleX, -45f, 45f);

        // 回転を適用
        Quaternion rot = Quaternion.Euler(nextAngleX, cameraPos.eulerAngles.y, 0f);
        cameraPos.rotation = rot;

    }
    //ジャンプ
    void Jump()
    {
        // ジャンプ
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
        }

        // 重力
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
  

    private float rotationVelocity;
}