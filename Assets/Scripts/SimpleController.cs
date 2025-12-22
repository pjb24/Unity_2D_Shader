using UnityEngine;

public class SimpleController : MonoBehaviour
{
    // === 인스펙터에서 조정 가능한 변수 ===

    [Tooltip("캐릭터의 이동 속도")]
    public float moveSpeed = 5f;

    [Tooltip("점프 시 가해지는 힘의 크기")]
    public float jumpForce = 8f;

    // === 컴포넌트 참조 ===

    private Rigidbody2D rb;

    // === 스케일 변수 추가 ===
    // 캐릭터의 원래 X 스케일 값(양수)을 저장할 변수입니다.
    private float originalScaleX;

    // Start는 첫 번째 프레임 업데이트 이전에 호출됩니다.
    void Start()
    {
        // Rigidbody2D 컴포넌트 참조를 가져옵니다.
        rb = GetComponent<Rigidbody2D>();

        // Rigidbody2D가 없을 경우를 대비한 안전 장치
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D 컴포넌트가 없습니다! 스크립트가 작동하지 않습니다.");
            enabled = false;
        }

        // 캐릭터의 회전을 막아 넘어지는 것을 방지합니다.
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // 💡 중요: 시작할 때 현재 스케일의 절댓값을 저장합니다.
        originalScaleX = Mathf.Abs(transform.localScale.x);
    }

    // FixedUpdate는 물리 계산에 사용됩니다.
    void FixedUpdate()
    {
        // 1. 수평 입력 감지
        float horizontalInput = Input.GetAxis("Horizontal");

        // 2. 새로운 이동 속도 계산 및 적용
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y); // rb.velocity 사용 권장
        rb.linearVelocity = movement; // rb.velocity 사용 권장
    }

    // Update는 매 프레임마다 호출되며, 사용자 입력 처리에 사용됩니다.
    void Update()
    {
        // 1. 점프 입력 감지 (무한 점프 버전)
        if (Input.GetButtonDown("Jump"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // 수직 속도를 0으로 초기화
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 2. 캐릭터 방향 뒤집기 (수정된 로직)
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            // 입력 방향을 나타내는 부호 (오른쪽: 1, 왼쪽: -1)
            float directionSign = Mathf.Sign(horizontalInput);

            // 현재 스케일을 가져와서 X축만 변경합니다.
            Vector3 newScale = transform.localScale;

            // 저장된 원래 스케일(양수)에 방향 부호를 곱합니다.
            // (예: 0.5 * 1 = 0.5 또는 0.5 * -1 = -0.5)
            newScale.x = originalScaleX * directionSign;

            transform.localScale = newScale;
        }
    }
}