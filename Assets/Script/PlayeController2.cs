using Unity.VisualScripting;
using UnityEngine;

public class PlayeController2 : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpPower = 5f;

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
    [SerializeField] BoxCollider groundCollider;

   

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
        Debug.Log(isGrounded);
    }
    //プレイヤーの移動関連
    private void Move()
    {
        // 入力
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            // カメラ基準の移動方向に変換
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }
    //カメラワーク関連
    void CameraWork()
    {
        
        cameraPos.position = transform.position;

        //右スティック入力
        float cx = Input.GetAxisRaw("Vertical2");   // 上下
        float cy = Input.GetAxisRaw("Horizontal2"); // 左右

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
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Ground"))
    //    {
    //        Debug.Log("jlkajf");
    //        isGrounded = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Ground"))
    //    {
    //        isGrounded =false;
    //    }
    //}

    private float rotationVelocity;
}