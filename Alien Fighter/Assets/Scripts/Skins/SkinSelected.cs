using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelected : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private Image buttonSprite;
    [SerializeField] private Text currentGoldUI;
    [SerializeField] private Text skinCostUI;

    [Header("Images")]
    [SerializeField] private Sprite buySprite;
    [SerializeField] private Sprite acceptSprite;
    [SerializeField] private Sprite chooseSprite;

    [Header("Skins")]
    [SerializeField] private Sprite[] allSkins;
    [SerializeField] private int[] allSkinsCost;

    [Header("Clear Buy")]
    [SerializeField] private GameObject buyClearPanel;
    [SerializeField] private GameObject afterBuyClearPanel;
    [SerializeField] private Text countOfClearUI;

    private int currentSkinChoose;
    private int currentSkinSelected;

    private static Text currentGoldTextTempToUpdate;
    private static int currentGold;

    private void Awake()
    {
        StartScheckKeys();
        currentGoldTextTempToUpdate = currentGoldUI;
    }

    private void OnEnable()
    {
        UpdateShopUI();
        UpdateGoldUI();
        UpdateClearUI();
    }

    private void StartScheckKeys()
    {
        if (!PlayerPrefs.HasKey("SelectedSkin"))
        {
            PlayerPrefs.SetInt("SelectedSkin", 0);
        }
        if (!PlayerPrefs.HasKey("Gold"))
        {
            PlayerPrefs.SetInt("Gold", 0);
        }

        /* Set State of all skins */
        if (!PlayerPrefs.HasKey("Skin00"))
        {
            PlayerPrefs.SetInt("Skin00", 1);
        }
        if (!PlayerPrefs.HasKey("Skin01"))
        {
            PlayerPrefs.SetInt("Skin01", 0);
        }
        if (!PlayerPrefs.HasKey("Skin02"))
        {
            PlayerPrefs.SetInt("Skin02", 0);
        }
        if (!PlayerPrefs.HasKey("Skin03"))
        {
            PlayerPrefs.SetInt("Skin03", 0);
        }
    }

    private void Update()
    {
        if (skinCostUI.color.Equals(Color.red))
        {
            skinCostUI.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 1));
        }
    }

    public static void UpdateGoldUI()
    {
        currentGold = PlayerPrefs.GetInt("Gold");
        currentGoldTextTempToUpdate.text = currentGold.ToString();
    }

    private void UpdateShopUI()
    {
        currentSkinChoose = PlayerPrefs.GetInt("SelectedSkin");
        currentSkinSelected = currentSkinChoose;
        spriteRenderer.sprite = allSkins[currentSkinChoose];
        currentGold = PlayerPrefs.GetInt("Gold");

        currentGoldUI.text = currentGold.ToString();
        skinCostUI.text = "";
        skinCostUI.color = Color.white;
        buttonSprite.sprite = chooseSprite;
    }

    public void SetSkin()
    {
        if (HasSkin())
        {
            PlayerPrefs.SetInt("SelectedSkin", currentSkinChoose);
            currentSkinSelected = currentSkinChoose;
            skinCostUI.text = "";
            skinCostUI.color = Color.white;
            buttonSprite.sprite = chooseSprite;
        }
        else
        {
            if (currentGold >= allSkinsCost[currentSkinChoose])
            {
                BuySkin();

                currentGold -= allSkinsCost[currentSkinChoose];
                PlayerPrefs.SetInt("Gold", currentGold);

                currentGoldUI.text = currentGold.ToString();
                skinCostUI.text = "";
                skinCostUI.color = Color.white;
                buttonSprite.sprite = acceptSprite;
            }
            else
            {
                skinCostUI.text = allSkinsCost[currentSkinChoose].ToString();
                skinCostUI.color = Color.red;
            }
        }
    }

    private void BuySkin()
    {
        switch (currentSkinChoose)
        {
            case 0:
                PlayerPrefs.SetInt("Skin00", 1);
                break;

            case 1:
                PlayerPrefs.SetInt("Skin01", 1);
                break;

            case 2:
                PlayerPrefs.SetInt("Skin02", 1);
                break;

            case 3:
                PlayerPrefs.SetInt("Skin03", 1);
                break;
        }
    }

    public void SkinLeft()
    {
        currentSkinChoose--;
        if (currentSkinChoose < 0)
        {
            currentSkinChoose = allSkins.Length - 1;
        }

        if (HasSkin())
        {
            if (currentSkinChoose == currentSkinSelected)
            {
                skinCostUI.text = "";
                buttonSprite.sprite = chooseSprite;
                skinCostUI.color = Color.green;
            }
            else
            {

                skinCostUI.text = "";
                buttonSprite.sprite = acceptSprite;
                skinCostUI.color = Color.white;
            }
        }
        else
        {
            skinCostUI.text = allSkinsCost[currentSkinChoose].ToString();
            buttonSprite.sprite = buySprite;
            skinCostUI.color = Color.white;
        }

        spriteRenderer.sprite = allSkins[currentSkinChoose];
    }

    public void SkinRight()
    {
        currentSkinChoose++;
        if (currentSkinChoose > allSkins.Length - 1)
        {
            currentSkinChoose = 0;
        }

        if (HasSkin())
        {
            if (currentSkinChoose == currentSkinSelected)
            {
                skinCostUI.text = "";
                skinCostUI.color = Color.white;
                buttonSprite.sprite = chooseSprite;
            }
            else
            {
                skinCostUI.text = "";
                skinCostUI.color = Color.white;
                buttonSprite.sprite = acceptSprite;
            }
        }
        else
        {
            skinCostUI.text = allSkinsCost[currentSkinChoose].ToString();
            skinCostUI.color = Color.white;
            buttonSprite.sprite = buySprite;
        }

        spriteRenderer.sprite = allSkins[currentSkinChoose];
    }

    private bool HasSkin()
    {
        switch (currentSkinChoose)
        {
            case 0:
                return PlayerPrefs.GetInt("Skin00") == 1 ? true : false;

            case 1:
                return PlayerPrefs.GetInt("Skin01") == 1 ? true : false;

            case 2:
                return PlayerPrefs.GetInt("Skin02") == 1 ? true : false;

            case 3:
                return PlayerPrefs.GetInt("Skin03") == 1 ? true : false;

            default:
                return false;
        }
    }

    #region Clear Buy
    private void UpdateClearUI()
    {
        countOfClearUI.text = ScoreController.s_chargeOfClear.ToString();
    }

    public void OpenBuyCleaner()
    {
        buyClearPanel.SetActive(true);
    }

    public void CloseBuyCleaner()
    {
        buyClearPanel.SetActive(false);
    }

    public void BuyCleaner()
    {
        ///<summary>
        ///cost - it is cost of one Clear
        ///</summary>
        int cost = 250;
        if (currentGold >= cost) // check that player have 100 or more gold to buy
        {
            afterBuyClearPanel.SetActive(true);

            currentGold -= cost;
            PlayerPrefs.SetInt("Gold", currentGold);

            ScoreController.AddClearCharge();

            currentGoldUI.text = currentGold.ToString();
            UpdateClearUI();
        }
    }

    public void CloseAfterBuyClearPanel()
    {
        afterBuyClearPanel.SetActive(false);
    }
    #endregion
}
