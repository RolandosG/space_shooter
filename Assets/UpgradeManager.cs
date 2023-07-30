using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    // Reference to your PlayerData
    public PlayerData playerData;
    public int damage = 0;
    // References to UI elements for cost
    public TextMeshProUGUI scoreMultiplierCostText;
    public TextMeshProUGUI scoreMultiplierLevelText;
    // References to Buttons for text changes
    public Button scoreMultiplierBuyButton;
    public Button healthUpgradeBuyButton;
    public Button dashAbilityBuyButton;
    // ... Repeat for all your upgrade level text components

    public TextMeshProUGUI healthUpgradeCostText;
    public TextMeshProUGUI healthUpgradeLevelText;
    public TextMeshProUGUI dashAbilityCostText;
    public TextMeshProUGUI dashAbilityLevelText;
    private int dashAbilityCost = 2500;
    // Your upgrade cost calculation parameters
    private int baseCost = 10;
    private float costMultiplier = 1.07f;

    private void Start()
    {
        playerData = SaveManager.Instance.Load();
        dashAbilityCostText.text = "Cost: " + dashAbilityCost;
        UpdateCostTexts();
        UpdateLevelTexts();
    }

    public int GetUpgradeCost(int currentLevel)
    {
        return (int)(baseCost * Mathf.Pow(costMultiplier, currentLevel));
    }

    public void UpdateCostTexts()
    {
        scoreMultiplierCostText.text = "Cost: " + GetUpgradeCost(playerData.scoreMultiplierLevel);
      
        healthUpgradeCostText.text = "Cost: " + GetUpgradeCost(playerData.healthUpgradeLevel);
        // ... Repeat for all your upgrade cost text components
    }

    public void UpdateLevelTexts()
    {
        scoreMultiplierLevelText.text = playerData.scoreMultiplierLevel + "/100";
        dashAbilityLevelText.text = (playerData.hasDashAbility ? "1" : "0") + "/1";
        healthUpgradeLevelText.text = playerData.healthUpgradeLevel + "/100";
        // ... Repeat for all your upgrade level text components
        // If the upgrade is at its max level, change the text of the button
        if (playerData.scoreMultiplierLevel >= 100)
            scoreMultiplierBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
        if (playerData.healthUpgradeLevel >= 100)
            healthUpgradeBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
        if (playerData.hasDashAbility)
            dashAbilityBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD";
    }

    public void PurchaseScoreMultiplier()
    {
        PurchaseUpgrade(ref playerData.scoreMultiplierLevel, scoreMultiplierCostText, scoreMultiplierLevelText);
        SaveManager.Instance.Save(playerData);
    }

    public void PurchaseHealthUpgrade()
    {
        PurchaseUpgrade(ref playerData.healthUpgradeLevel, healthUpgradeCostText, healthUpgradeLevelText);
    }

    public void PurchaseDashAbility()
    {
        if (!playerData.hasDashAbility && playerData.currency >= dashAbilityCost)
        {
            playerData.currency -= dashAbilityCost;
            playerData.hasDashAbility = true;

            SaveManager.Instance.Save(playerData);

            Debug.Log("Dash Ability purchased.");
        }
        else if (playerData.hasDashAbility)
        {
            Debug.Log("Player already has Dash Ability.");
        }
        else
        {
            Debug.Log("Player does not have enough currency to purchase the Dash Ability.");
        }
    }

    private void PurchaseUpgrade(ref int upgradeLevel, TextMeshProUGUI costText, TextMeshProUGUI levelText)
    {
        // Calculate the cost of the upgrade
        int cost = GetUpgradeCost(upgradeLevel);

        // Check if the player has enough currency
        if (playerData.currency >= cost)
        {
            // Subtract the cost from the player's currency
            playerData.currency -= cost;

            // Increment the level of the upgrade
            upgradeLevel++;

            if (costText == healthUpgradeCostText) // Check if it's a health upgrade
            {
                Stats.instance.health += 10;  // Increase the player's health by 10
            }

            // Save the player data
            SaveManager.Instance.Save(playerData);

            // Update the cost and level text
            costText.text = "Cost: " + GetUpgradeCost(upgradeLevel);
            levelText.text = upgradeLevel + "/100";

            Debug.Log("Upgrade purchased. New level: " + upgradeLevel);
        }
        else
        {
            Debug.Log("Player does not have enough currency to purchase the upgrade.");
        }
    }
}
