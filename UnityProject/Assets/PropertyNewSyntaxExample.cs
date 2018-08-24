using UnityEngine;

public class PropertyNewSyntaxExample : MonoBehaviour
{
    public string AutoPropertyInitializer { get; } = $"{nameof(AutoPropertyInitializer)} working!";

    public int Health { get; set; } = 100;
    public string PlayerHealthUiText => $"Player Health: {Health}";

    private readonly int[] scores = { 80, 20, 45, 15, 38, 60 };
    public int TestExpressionBodyReadyOnlyProperty => scores[0];
}
