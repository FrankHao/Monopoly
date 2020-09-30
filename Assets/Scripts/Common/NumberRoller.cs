using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class NumberRoller : MonoBehaviour
{
    Text numText;

    [SerializeField]
    float AnimateTime = 2f;
    [SerializeField]
    long desiredNumber = 0;
    [SerializeField]
    long initialNumber = 0;

    long currentNumber;
    long delta;
    System.Action OnComplete;

    static readonly float REFRESH_RATE = 1f / 30f;

    // Start is called before the first frame update
    void Start()
    {
        numText = GetComponent<Text>();
    }

    private void OnEnable()
    {
        currentNumber = initialNumber;
        delta = desiredNumber - initialNumber;
        if (delta < 0)
        {
            delta = -delta;
        }

        StartCoroutine(RollText());
    }

    public void Setup(long start, long end, float duration = 1f, System.Action onComplete = null)
    {
        AnimateTime = duration;
        initialNumber = start;
        desiredNumber = end;
        OnComplete = onComplete;
    }

    IEnumerator RollText()
    {
        long jump = delta / (long)(AnimateTime / REFRESH_RATE);

        bool running = true;

        while (running)
        {
            yield return new WaitForSeconds(REFRESH_RATE);

            if (currentNumber < desiredNumber)
            {
                currentNumber += jump;
                if (currentNumber > desiredNumber)
                {
                    currentNumber = desiredNumber;
                }
            }
            else if (currentNumber > desiredNumber)
            {
                currentNumber -= jump;
                if (currentNumber < desiredNumber)
                {
                    currentNumber = desiredNumber;
                }
            }
            else
            {
                running = false;
            }

            numText.text = currentNumber.ToString();
        }

        OnComplete?.Invoke();
    }
}
