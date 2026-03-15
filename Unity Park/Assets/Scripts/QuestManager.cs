using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public bool iceCreamCollected = false;
    public bool pizzaCollected = false;

    private void Awake()
    {
        Instance = this;
    }

    public bool CanCollectIceCream()
    {
        return !iceCreamCollected;
    }

    public bool CanCollectPizza()
    {
        return iceCreamCollected && !pizzaCollected;
    }

    public void CollectIceCream()
    {
        iceCreamCollected = true;
        Debug.Log("Μαζεύτηκε το αντικείμενο από το παγωτατζίδικο");
    }

    public void CollectPizza()
    {
        pizzaCollected = true;
        Debug.Log("Μαζεύτηκε το αντικείμενο από την πιτσαρία");
    }
}