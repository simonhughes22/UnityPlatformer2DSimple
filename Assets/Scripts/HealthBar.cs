using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image[] hearts;
    [SerializeField] Image heartImage;    

    [SerializeField] float xOffsetPct = 0.05f;
    [SerializeField] float yOffsetPct = 0.075f;
    [SerializeField] int spacer = 10; 

    [SerializeField]
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        float xOffset = canvasScaler.referenceResolution.x * xOffsetPct;
        float yOffset = canvasScaler.referenceResolution.y * yOffsetPct;

        float heartWidth = heartImage.preferredWidth * heartImage.transform.localScale.x;
        
        // no need to compare to screen as offset from left
        float anchorX = xOffset;
        // need to substract from height as offset from top
        float anchorY = Screen.height - yOffset;

        hearts = new Image[playerController.maxHealth];        

        for (int i = 0; i < playerController.maxHealth; i++)
        {
            Image heart = Instantiate(heartImage, this.gameObject.transform, false);
            heart.transform.position = new Vector3(anchorX + ((heartWidth + spacer) * i),
                anchorY, transform.position.z);
            hearts[i] = heart;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerController.maxHealth; i++)
        {
            Image heart = hearts[i];
            if (i < playerController.currentHealth)
            {
                heart.enabled = true;
            }
            else
            {
                heart.enabled = false;
            }
        }
    }
}
