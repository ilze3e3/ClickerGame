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

    // Text
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
        upgrades.Add("+1_ToClick", new UpgradeType(10, 1, "Click", 10));
        upgrades.Add("+5_ToClick", new UpgradeType(100, 5, "Click", 10));
        upgrades.Add("+10_ToClick", new UpgradeType(500, 10, "Click", 10));
        upgrades.Add("+100_ToClick", new UpgradeType(2000, 100, "Click", 10));
        upgrades.Add("+1_ToIdle", new UpgradeType(1000, 1, "Idle", 10));
        upgrades.Add("+5_ToIdle", new UpgradeType(5000, 5, "Idle", 10));
        upgrades.Add("+10_ToIdle", new UpgradeType(10000, 10, "Idle", 10));
        upgrades.Add("+100_ToIdle", new UpgradeType(50000, 100, "Idle", 10));

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


        // Configure text here
        enableIdlePrice.text = "750";
        add1_ClickPrice.text = upgrades["+1_ToClick"].GetCost().ToString();
        add5_ClickPrice.text = upgrades["+5_ToClick"].GetCost().ToString();
        add10_ClickPrice.text = upgrades["+10_ToClick"].GetCost().ToString();
        add100_ClickPrice.text = upgrades["+100_ToClick"].GetCost().ToString();
        add1_IdlePrice.text = upgrades["+1_ToIdle"].GetCost().ToString();
        add5_IdlePrice.text = upgrades["+5_ToIdle"].GetCost().ToString();
        add10_IdlePrice.text = upgrades["+10_ToIdle"].GetCost().ToString();
        add100_IdlePrice.text = upgrades["+100_ToIdle"].GetCost().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdleActive)
        {
            currency += addPerSecond * Time.deltaTime;
        }
        cText.text = currency.ToString("0");
    }

    public void click()
    {
        currency += addPerClick;
    }

    public void BuyIdleUpgrade()
    {
        float cost = 750.0f;

        if (currency < cost)
        {
            // TODO: Make Popup appear that says not enough funds
            return; 
        }
        else
        {
            isIdleActive = true;
            currency -= cost;
            enableIdle.interactable = false;
            enableIdle.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
            enableIdlePrice.text = "Sold out";
        }
        return;
    }
    public void BuyRaiseValueUpgrade(string command)
    {
        if (currency < upgrades[command].GetCost())
        {
            // TODO: Make Popup appear that says not enough funds
        }
        else
        {
            currency -= upgrades[command].GetCost();

            if (upgrades[command].GetDestination() == "Click")
            {
                addPerClick += upgrades[command].UseUpgrade();
            }
            else if (upgrades[command].GetDestination() == "Idle")
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
                updatePriceAndButton(add1_ClickPrice, add_1_ToClick, command);
                break;
            case "+5_ToClick":
                updatePriceAndButton(add5_ClickPrice, add_5_ToClick, command);
                break;
            case "+10_ToClick":
                updatePriceAndButton(add10_ClickPrice, add_10_ToClick, command);
                break;
            case "+100_ToClick":
                updatePriceAndButton(add100_ClickPrice, add_100_ToClick, command);
                break;
            case "+1_ToIdle":
                updatePriceAndButton(add1_IdlePrice, add_10_ToClick, command);
                break;
            case "+5_ToIdle":
                updatePriceAndButton(add1_ClickPrice, add_1_ToClick, command);
                break;
            case "+10_ToIdle":
                updatePriceAndButton(add1_ClickPrice, add_1_ToClick, command);
                break;
            case "+100_ToIdle":
                updatePriceAndButton(add1_ClickPrice, add_1_ToClick, command);
                break;
            default:
                Debug.Log("in Line 102 of ClickerController ran into default");
                break;

        }
        
    }

    private void updatePriceAndButton(TextMeshProUGUI text, Button btn, string command)
    {
        if (upgrades[command].GetNumberOfUpgradesAllowed() == 0)
        {
            text.text = "Sold Out";
        }
        else
        {
            text.text = upgrades[command].GetCost().ToString();
            btn.interactable = false;
            btn.GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        }
    }
}
