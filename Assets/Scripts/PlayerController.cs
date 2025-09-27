using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody; //Rigidbody2d型の変数
    float axisHx, axisHy; //float型のxとy移動用の変数
    public float speed = 3.0f; //float型プレイヤーの移動用の変数
    public Menu menu;
    Animator animator; //animetorのオブジェクトを取ってくるためのAnimator型の変数

    public float angleZ = -90.0f; //回転角度
    bool isMoving = false;

    int direction = 0;//移動方向 


    public float interactRange = 1.0f; //調べる距離
    public LayerMask check;
    public Vector2 FacingDirection { get; private set; } = Vector2.down;
    Collider2D col;



    float GetAngle(Vector2 p1, Vector2 p2)
    {
        float angle;
        if (axisHx != 0 || axisHy != 0)
        {
            // 移動中であれば角度を更新する
            // p1からp2への差分(原点を0にするため)

            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;

            // アークタンジェント2関数で角度(ラジアン)を求める
            float rad = Mathf.Atan2(dy, dx);
            // ラジアンを度に変換して返す
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            //停止中であれば以前の角度を維持
            angle = angleZ;
        }

        return angle;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        col = GetComponent<Collider2D>();
        rbody = GetComponent<Rigidbody2D>(); //Rigidbody2Dを得る
        rbody.gravityScale = 0; //落ちないように重力0
        animator = GetComponent<Animator>(); //Animatorを得る



    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.playing)
        return; // メニュー中などは入力を受け付けない

        if (menu != null && menu.isMenuOpen)
        {
            axisHx = 0;
            axisHy = 0;
            rbody.linearVelocity = Vector2.zero;
            animator.SetInteger("Direction", direction);
            return;
        }



        axisHx = Input.GetAxisRaw("Horizontal");
        axisHy = Input.GetAxisRaw("Vertical");



        //キー入力から移動角度を求める
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisHx, fromPt.y + axisHy);
        angleZ = GetAngle(fromPt, toPt);

        //移動角度から向いている方向とアニメーション更新
        int dir;


        if (angleZ >= -45 && angleZ < 45)
        {
            //右向き
            dir = 3;
        }
        else if (angleZ >= 45 && angleZ <= 135)
        {
            //上向き
            dir = 2;
        }
        else if (angleZ >= -135 && angleZ <= -45)
        {
            //下向き
            dir = 0;
        }
        else
        {
            //左向き
            dir = 1;
        }

        if (dir != direction)
        {
            direction = dir;
            animator.SetInteger("Direction", direction);
        }



    }

    void FixedUpdate()
    {


        //移動速度を更新する
        // rbody.linearVelocity = new Vector2(axisHx * speed, axisHy * speed);
        rbody.linearVelocity = new Vector2(axisHx, axisHy).normalized * speed;

    }

    public void SetAxis(float hx, float hy)
    {
        axisHx = hx;
        axisHy = hy;

        if (axisHx == 0 && axisHy == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }


    public void CheckObject()
    {

        Vector2 size = col.bounds.size;
        Vector2 dir = Vector2.zero;

        switch (direction)
        {
            case 0: dir = Vector2.down; break;   // 下
            case 1: dir = Vector2.left; break;   // 左
            case 2: dir = Vector2.up; break;     // 上
            case 3: dir = Vector2.right; break;  // 右
        }

        Vector2 halfSizeOffset = dir * (((dir.x != 0 ? size.x : size.y) / 2f) + interactRange / 2f);
        Vector2 origin = (Vector2)col.bounds.center + halfSizeOffset;

        RaycastHit2D hit = Physics2D.BoxCast(
            origin,       // キャラの少し前
            size,         // 判定ボックスのサイズ
            0f,           // 回転角度
            dir,          // 向き
            0f,           // すでに前方にオフセットしてるので距離は0
            check         // LayerMask
        );



        if (hit.collider != null)
        {
            Debug.Log($"見つけた: {hit.collider.name}");
        }
        else
        {
            Debug.Log("何もない");
        }
    }

}