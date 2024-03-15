using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Import DOTween namespace

public class HealthBarFade : MonoBehaviour
{
    public Image barImage;
    public Image damagedBarImage;

    public float shakeDuration = 0.3f;
    public Vector3 shakeStrength = new Vector3(0.1f, 0.1f, 0);
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;

    public float damagedHealthFadeTimerMax = 1f;

    private Color damagedColor;
    private float damagedHealthFadeTimer;
    private bool isActive = false;

    // Reference to the parent object containing the health bar
    private GameObject healthBarParent;

    // Add reference to the enemy script
    public Enemy myEnemy;

    private void Awake()
    {
        if (barImage == null)
            barImage = transform.Find("Bar").GetComponent<Image>();

        if (damagedBarImage == null)
            damagedBarImage = transform.Find("damagedBar").GetComponent<Image>();

        damagedColor = damagedBarImage.color;
        damagedColor.a = 0f;
        damagedBarImage.color = damagedColor;

        // Get the parent object
        healthBarParent = transform.parent.gameObject;
    }

    private void Update()
    {
        if (damagedColor.a > 0)
        {
            damagedHealthFadeTimer -= Time.deltaTime;
            if (damagedHealthFadeTimer < 0)
            {
                float fadeAmount = 5f;
                damagedColor.a -= fadeAmount * Time.deltaTime;
                damagedBarImage.color = damagedColor;
            }
        }
    }

    private void Start()
    {
        Deactivate(); // Start with health bar deactivated
    }

    // Method to set the specific Enemy instance
    public void SetEnemyInstance(Enemy enemy)
    {
        myEnemy = enemy;

        // Subscribe to the enemy's death event
        if (myEnemy != null)
        {
            myEnemy.onDeath.AddListener(Deactivate);
        }
    }

    // Method to unsubscribe from the enemy's death event
    private void OnDestroy()
    {
        if (myEnemy != null)
        {
            myEnemy.onDeath.RemoveListener(Deactivate);
        }
    }

    private void OnEnable()
    {
        // Subscribe to the enemy's damage delegate
        if (myEnemy != null)
        {
            myEnemy.damageDelegate += MyMethod;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the enemy's damage delegate
        if (myEnemy != null)
        {
            myEnemy.damageDelegate -= MyMethod;
        }
    }

    public void Activate()
    {
        // Activate the health bar
        isActive = true;
        healthBarParent.SetActive(true);
        SetHealth(1f); // Set health to 100%
    }

    public void Deactivate()
    {
        // Deactivate the health bar
        isActive = false;
        healthBarParent.SetActive(false);
    }

    public void SetHealth(float healthNormalized)
    {
        // Ensure health value is clamped between 0 and 1
        //healthNormalized = Mathf.Clamp01(healthNormalized);
        healthNormalized = myEnemy.currentHealth / myEnemy.maxHealth;
        barImage.fillAmount = healthNormalized;
    }

    // Method to handle enemy damage event
    private void MyMethod()
    {
        if (damagedColor.a < 0)
        {
            // Set the damaged health bar color and timer
            damagedBarImage.fillAmount = barImage.fillAmount;
            damagedColor.a = 1f;
            damagedBarImage.color = damagedColor;
            damagedHealthFadeTimer = damagedHealthFadeTimerMax;

            // Shake the enemy's portrait
            if (myEnemy != null)
            {
                myEnemy.transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);
            }
        }
        else // If double hit
        {
            damagedColor.a = 1f;
            damagedBarImage.color = damagedColor;
            damagedHealthFadeTimer = damagedHealthFadeTimerMax;

            // Shake the enemy's portrait
            if (myEnemy != null)
            {
                myEnemy.transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness);
            }
        }
    }
}