using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeroKnight : Unit {

    [SerializeField] private float m_speed = 4.0f;
    [SerializeField] private float m_jumpForce = 7.5f;
    [SerializeField] private float m_rollForce = 6.0f;
    [SerializeField] private bool m_noBlood = false;
    [SerializeField] private GameObject m_slideDust;

    [SerializeField] private int maxLives = 5;
    [SerializeField] private new int damage = 1;
    [SerializeField] private float radiusAttak = 1f;
    [SerializeField] private new bool immortality = false;
    [SerializeField] private float forceRepulsion = 6.5f;
                     private new int lives;

    [HideInInspector] public bool m_dead = false;
    [HideInInspector] public int score = 0;
    
    [SerializeField] private Image[] hearts;

    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart;
    [SerializeField] private Button reset;
    [SerializeField] private Text die;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private bool                haveKey = false;
    private Collider2D          door;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private bool                win = false;

    private Pause pause;
    [SerializeField] private Text winText;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip swingOfSword;
    [SerializeField] private AudioClip potionSound;
    [SerializeField] private AudioClip gettingHit;
    [SerializeField] private AudioClip keySound;
    [SerializeField] private AudioClip doorSound;
    [SerializeField] private AudioClip chestSound;

    private void Awake () {
        pause = GameObject.Find("_Pause").GetComponent<Pause>();
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
    }

    private void Start() {
        lives = maxLives;
        score = 0;
    }

    private void Update () {
        if (win && Input.GetKeyDown("space")) {
            SceneManager.LoadSceneAsync(0);
        }

        if (!pause.isPause && !win) {
            if (door != null) 
                if (door.transform.position.y > -7) door.transform.position -= new Vector3(0, 1.5f, 0) * Time.deltaTime;
            if (!m_dead) {
                if (transform.position.y < -40)
                    Die();

                // Увеличить таймер, который контролирует комбо
                m_timeSinceAttack += Time.deltaTime;

                //Проверка того, что персонаж на земле
                if (!m_grounded && m_groundSensor.State()) {
                    m_grounded = true;
                    m_animator.SetBool("Grounded", m_grounded);
                }

                //Проверка того, что персонаж только что начал падать
                if (m_grounded && !m_groundSensor.State()) {
                    m_grounded = false;
                    m_animator.SetBool("Grounded", m_grounded);
                }

                //Управление персонажа по горизонтальной оси
                float inputX = Input.GetAxis("Horizontal");

                //Направление персонажа
                if (inputX > 0) {
                    GetComponent<SpriteRenderer>().flipX = false;
                    m_facingDirection = 1;
                }

                else if (inputX < 0) {
                    GetComponent<SpriteRenderer>().flipX = true;
                    m_facingDirection = -1;
                }

                //Передвижение
                if (!m_rolling) m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

                //Ускорение падения
                m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

                //Атака
                if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling) {
                    Fight2D.Action(transform.position, radiusAttak, 9, damage, true);
                    AudioSource.PlayClipAtPoint(swingOfSword, transform.position);

                    m_currentAttack++;

                    //Сброс комбо
                    if (m_currentAttack > 3)
                        m_currentAttack = 1;                    
                    if (m_timeSinceAttack > 1.0f)
                        m_currentAttack = 1;

                    //Анимация атаки
                    m_animator.SetTrigger("Attack" + m_currentAttack);

                    //Сброс таймера
                    m_timeSinceAttack = 0.0f;
                }

                //Перекат
                else if (Input.GetKeyDown("left shift") && !m_rolling) {
                    m_rolling = true;
                    m_animator.SetTrigger("Roll");
                    m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
                }

                //Прыжок
                else if (Input.GetKeyDown("space") && m_grounded && !m_rolling) {
                    m_animator.SetTrigger("Jump");
                    m_grounded = false;
                    m_animator.SetBool("Grounded", m_grounded);
                    m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                    m_groundSensor.Disable(0.2f);
                }

                //Бег
                else if (Mathf.Abs(inputX) > Mathf.Epsilon) {
                    //Сброс таймера
                    m_delayToIdle = 0.05f;
                    m_animator.SetInteger("AnimState", 1);
                }

                //Idle
                else {
                    m_delayToIdle -= Time.deltaTime;
                    if (m_delayToIdle < 0)
                        m_animator.SetInteger("AnimState", 0);
                }
            }
            else {
                reset.gameObject.SetActive(true);
                die.gameObject.SetActive(true);
            }
        }
    }

    // Animation Events
    // Called in end of roll animation.
    void AE_ResetRoll()
    {
        m_rolling = false;
    }
    private void HealthCheck(){
        if (lives > maxLives)
            lives = maxLives;
        for (int i = 0; i < hearts.Length; i++){
            if (i < lives)
                hearts[i].sprite = aliveHeart;
            else
                hearts[i].sprite = deadHeart;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        //проверяем слой объекта с которым столкнулись
        if (collision.gameObject.layer == 9)
            //9 слой - врагов
        {
            ReceiveDamage(collision.gameObject.GetComponent<Unit>().GetDamege());
            Vector2 dir = (transform.position - collision.transform.position).normalized;
            if (dir.y < 0)
                dir.y= 0.5f;
            m_body2d.AddForce(dir * forceRepulsion, ForceMode2D.Impulse);
        }
    }

    public override void Die() {
        m_animator.SetBool("noBlood", m_noBlood);
        m_animator.SetTrigger("Death");
        m_dead = true;

        for(int i = 0; i < hearts.Length && hearts[i].sprite == aliveHeart; i++) {
            hearts[i].sprite = deadHeart;
        }
    }

    //Поднимаем монетку или зелье
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.tag == "Coin") {
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
            Destroy(collision.gameObject);
            score += 5;
        }
        else if (collision.transform.tag == "Potion") {
            Destroy(collision.gameObject);
            AudioSource.PlayClipAtPoint(potionSound, transform.position);
            lives = 5;
            HealthCheck();
        }
        else if (collision.transform.tag == "Key") {
            haveKey = true;
            Destroy(collision.gameObject);
            AudioSource.PlayClipAtPoint(keySound, transform.position);
        }
        else if (collision.transform.tag == "Door" && haveKey) {
            door = collision;
            AudioSource.PlayClipAtPoint(doorSound, transform.position);
        }
        else if (collision.transform.tag == "Chest") {
            AudioSource.PlayClipAtPoint(chestSound, transform.position);
            win = true;
            winText.gameObject.SetActive(true);
        }
    }

    public override void ReceiveDamage(int damage) {
        if (!m_rolling) m_animator.SetTrigger("Hurt");
        AudioSource.PlayClipAtPoint(gettingHit, transform.position);
        if (!immortality)
            lives -= damage;
        HealthCheck();
        if (lives < 1)
            Die();
        Debug.Log(transform.name +": " +  lives);
    }
}
