using System;
using UnityEngine;
using UnityEngine.UI;

public class JailQuestionUI : MonoBehaviour
{
    [SerializeField] Button usecardButton = null;
    [SerializeField] Button rollDiceButton = null;
    [SerializeField] Button payButton = null;
    [SerializeField] Button stayButton = null;

    public Action OnUseCard;
    public Action OnRollDice;
    public Action OnPayBail;
    public Action OnStay;


    // Start is called before the first frame update
    void Start()
    {
        usecardButton.onClick.AddListener(useCard);
        rollDiceButton.onClick.AddListener(rollDice);
        payButton.onClick.AddListener(payCash);
        stayButton.onClick.AddListener(stay);
    }

    private void Update()
    {
        usecardButton.enabled = (OnUseCard != null);
        rollDiceButton.enabled = (OnRollDice != null);
        payButton.enabled = (OnPayBail != null);
        stayButton.enabled = (OnStay != null);
    }

    void useCard()
    {
        OnUseCard();
    }

    void rollDice()
    {
        OnRollDice();
    }

    void payCash()
    {
        OnPayBail();
    }

    void stay()
    {
        OnStay();
    }
}
