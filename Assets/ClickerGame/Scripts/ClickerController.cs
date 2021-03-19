using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickerController : MonoBehaviour
{
    private Dictionary <string, UpgradeType> upgrades;
    private float currency = 0;
    private float addPerClick = 1;
    private float addPerSecond = 1;
    private bool isIdleActive = false;

    // Cost
    private float enableIdleCost = 10.0f;

    // Not Enough Funds popup
    public Image notEnoughFundsPopup;
    float disableFundsPopupTime;

    // Cost Text
    public TextMeshProUGUI cText;
    public TextMeshProUGUI enableIdlePrice;
    public TextMeshProUGUI add1_ClickPrice;
    public TextMeshProUGUI add5_ClickPrice;
    public TextMeshProUGUI add10_ClickPrice;
    public TextMeshProUGUI add100_ClickPrice;
    public TextMeshProUGUI add1_IdlePrice;
    public TextMeshProUGUI add5_IdlePrice;
    public TextMeshProUGUI add10_IdlePrice;
    public TextMeshProUGUI add100_IdlePrice;
    
    // Quantity Text

    public TextMeshProUGUI add_1_ClickQuantity;
    public TextMeshProUGUI add_5_ClickQuantity;
    public TextMeshProUGUI add_10_ClickQuantity;
    public TextMeshProUGUI add_100_ClickQuantity;
    public TextMeshProUGUI enableIdleQuantity;
    public TextMeshProUGUI add_1_IdleQuantity;
    public TextMeshProUGUI add_5_IdleQuantity;
    public TextMeshProUGUI add_10_IdleQuantity;
    public TextMeshProUGUI add_100_IdleQuantity;

    // Buttons
    public Button clicker;
    public Button enableIdle;
    public Button add_1_ToClick;
    public Button add_5_ToClick;
    public Button add_10_ToClick;
    public Button add_100_ToClick;
    public Button add_1_ToIdle;
    public Button add_5_ToIdle;
    public Button add_10_ToIdle;
    public Button add_100_ToIdle;
   
    // Start is called before the first frame update
    void Start()
    {
        upgrades = new Dictionary<string, UpgradeType>();

        // Add Upgrades Here
        upgrades.Add("+1_ToClick", new UpgradeType(1, 1, "Click", 10));
        upgrades.Add("+5_ToClick", new UpgradeType(10, 5, "Click", 10));
        upgrades.Add("+10_ToClick", new UpgradeType(50, 10, "Click", 10));
        upgrades.Add("+100_ToClick", new UpgradeType(200, 100, "Click", 10));
        upgrades.Add("+1_ToIdle", new UpgradeType(10, 1, "Idle", 10));
        upgrades.Add("+5_ToIdle", new UpgradeType(50, 5, "Idle", 10));
        upgrades.Add("+10_ToIdle", new UpgradeType(100, 10, "Idle", 10));
        upgrades.Add("+100_ToIdle", new UpgradeType(500, 100, "Idle", 10));

        // Add listeners to button here
        clicker.GetComponent<Button>().onClick.AddListener(click);

        enableIdle.GetComponent<Button>().onClick.AddListener(BuyIdleUpgrade);

        add_1_ToClick.GetComponent<Button>().onClick.AddListener(delegate { BuyRaiseValueUpgrade("+1_ToClick"); });
        add_5_ToClick.GetComponent<Button>().onClick.AddListener(delegate { BuyRaiseValueUpgrade("+5_ToClick"); });
        add_10_ToClick.GetComponent<Button>().onClick.AddListener(delegate { BuyRaiseValueUpgrade("+10_ToClick"); });
        add_100_ToClick.GetComponent<Button>().onClick.AddListener(delegate { BuyRaiseValueUpgrade("+100_ToClick"); });
        add_1_ToIdle.GetComponent<Button>().onClick.AddListener(delegate { BuyRaiseValueUpgrade("+1_ToIdle"); });
        add_5_ToIdle.GetComponent<Button>().onClick.AddListener(delegate { BuyRaiseValueUpgrade("+5_ToIdle"); });
        add_10_ToIdle.GetComponent<Button>().onClick.AddListener(delegate { BuyRaiseValueUpgrade("+10_ToIdle"); });
        add_100_ToIdle.GetComponent<Button>().onClick.AddListener(delegate { BuyRaiseValueUpgrade("+100_ToIdle"); });

        //Disable Idle upgrades until bought

        add_1_ToIdle.interactable = false;
        add_5_ToIdle.interactable = false;
        add_10_ToIdle.interactable = false;
        add_100_ToIdle.interactable = false;
        // Configure text here
        enableIdleQuantity.text = "x1";
        enableIdlePrice.text = enableIdleCost.ToString();

        add1_ClickPrice.text = upgrades["+1_ToClick"].GetCost().ToString();
        add_1_ClickQuantity.text = "x" + upgrades["+1_ToClick"].GetNumberOfUpgradesAllowed().ToString();

        add5_ClickPrice.text = upgrades["+5_ToClick"].GetCost().ToString();
        add_5_ClickQuantity.text = "x" + upgrades["+5_ToClick"].GetNumberOfUpgradesAllowed().ToString();

        add10_ClickPrice.text = upgrades["+10_ToClick"].GetCost().ToString();
        add_10_ClickQuantity.text = "x" + upgrades["+10_ToClick"].GetNumberOfUpgradesAllowed().ToString();

        add100_ClickPrice.text = upgrades["+100_ToClick"].GetCost().ToString();
        add_100_ClickQuantity.text = "x" + upgrades["+100_ToClick"].GetNumberOfUpgradesAllowed().ToString();

        add1_IdlePrice.text = upgrades["+1_ToIdle"].GetCost().ToString();
        add_1_IdleQuantity.text = "x" + upgrades["+1_ToIdle"].GetNumberOfUpgradesAllowed().ToString();

        add5_IdlePrice.text = upgrades["+5_ToIdle"].GetCost().ToString();
        add_5_IdleQuantity.text = "x" + upgrades["+5_ToIdle"].GetNumberOfUpgradesAllowed().ToString();
        
        add10_IdlePrice.text = upgrades["+10_ToIdle"].GetCost().ToString();
        add_10_IdleQuantity.text = "x" + upgrades["+10_ToIdle"].GetNumberOfUpgradesAllowed().ToString();

        add100_IdlePrice.text = upgrades["+100_ToIdle"].GetCost().ToString();
        add_100_IdleQuantity.text = "x" + upgrades["+100_ToIdle"].GetNumberOfUpgradesAllowed().ToString();

        // If need to add "addPerSecond" per second than adding faster than a second
        //StartCoroutine(AddCurrencyPerSecond(1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdleActive)
        {
            currency += addPerSecond * Time.deltaTime;
        }
        cText.text = currency.ToString("0");

        if(Time.time > disableFundsPopupTime)
        {
          notEnoughFundsPopup.gameObject.SetActive(false);
        }

    }
    
    // Coroutine to add set value per second
    public IEnumerator AddCurrencyPerSecond(float seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            if (isIdleActive)
            {
                currency += addPerSecond;
            }
        }

    }

    // Deals with adding increment per click
    public void click()
    {
        currency += addPerClick;
    }

    //Allows player to buy the automatic clicker upgrade
    public void BuyIdleUpgrade()
    {
       

        if (currency < enableIdleCost) // If player does not have enough points to buy idle clicker then make popup appear
        {
            // TODO: Make Popup appear that says not enough funds
            notEnoughFundsPopup.transform.position = Input.mousePosition;
            notEnoughFundsPopup.gameObject.SetActive(true);
            disableFundsPopupTime = Time.time + 1f;
            return; 
        }
        else // Enable Idle clicker otherwise and update button accordingly
        {
            isIdleActive = true;
            add_1_ToIdle.interactable = true;
            add_5_ToIdle.interactable = true;
            add_10_ToIdle.interactable = true;
            add_100_ToIdle.interactable = true;
            currency -= enableIdleCost;
            enableIdle.interactable = false;
            enableIdle.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
            enableIdlePrice.text = "Sold out";
            enableIdleQuantity.text = "x0";
        }
        return;
    }

    // Allows the player to buy upgrades to the incremental clicker whether for manual clicks or automatic clicks.
    public void BuyRaiseValueUpgrade(string command)
    {
        if (command.Contains("Idle") && !isIdleActive) // If Idle clicker isn't activated, disable purchases of Idle Upgrades
        {
            return;
        }
        if (currency < upgrades[command].GetCost()) // If player does not have enough points to buy an upgrade then make the popup appear
        {
            // TODO: Make Popup appear that says not enough funds
            //Debug.Log("Not enough money for:" + command);
            notEnoughFundsPopup.transform.position = Input.mousePosition;
            notEnoughFundsPopup.gameObject.SetActive(true);
            disableFundsPopupTime = Time.time + 1f;
        }
        else // Take away cost of upgrade and update the dictionary
        {
            currency -= upgrades[command].GetCost();

            if (upgrades[command].GetDestination() == "Click") // If it is a button that corresponds to a click upgrade then add to the click increment
            {
                addPerClick += upgrades[command].UseUpgrade();
            }
            else if (upgrades[command].GetDestination() == "Idle") // If it is a buttont that corresponds to an idle upgrade then add to persecond increment.
            {
                addPerSecond += upgrades[command].UseUpgrade();
            }
            else
            {
                Debug.Log("in line 98 of Clicker Controller, Destination returning something unknown");
            }
        }
        switch(command)
        {
            case "+1_ToClick":
                updatePriceAndButton(add_1_ClickQuantity, add1_ClickPrice, add_1_ToClick, command);
                break;
            case "+5_ToClick":
                updatePriceAndButton(add_5_ClickQuantity, add5_ClickPrice, add_5_ToClick, command);
                break;
            case "+10_ToClick":
                updatePriceAndButton(add_10_ClickQuantity, add10_ClickPrice, add_10_ToClick, command);
                break;
            case "+100_ToClick":
                updatePriceAndButton(add_100_ClickQuantity, add100_ClickPrice, add_100_ToClick, command);
                break;
            case "+1_ToIdle":
                updatePriceAndButton(add_1_IdleQuantity, add1_IdlePrice, add_10_ToClick, command);
                break;
            case "+5_ToIdle":
                updatePriceAndButton(add_5_IdleQuantity, add5_IdlePrice, add_5_ToIdle, command);
                break;
            case "+10_ToIdle":
                updatePriceAndButton(add_10_IdleQuantity, add10_IdlePrice, add_10_ToIdle, command);
                break;
            case "+100_ToIdle":
                updatePriceAndButton(add_100_IdleQuantity, add100_IdlePrice, add_100_ToIdle, command);
                break;
            default:
                Debug.Log("in Line 102 of ClickerController ran into default");
                break;

        }
        
    }
    // Updates the price and button of the upgrades accordingly
    private void updatePriceAndButton(TextMeshProUGUI quantity_text, TextMeshProUGUI price_text, Button btn, string command)
    {
       
        if (upgrades[command].GetNumberOfUpgradesAllowed() == 0)
        {
            price_text.text = "Sold Out";
            btn.interactable = false;
            btn.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        }
        else
        {
            price_text.text = upgrades[command].GetCost().ToString();
            
        }
        quantity_text.text = "x" + upgrades[command].GetNumberOfUpgradesAllowed().ToString();
    }
}
