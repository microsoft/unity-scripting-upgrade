using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// You need to put Newtonsoft.Json.dll in your Assets/Plugins directory
/// for this to work!
/// Download it from NuGet: https://www.nuget.org/packages/Newtonsoft.Json
/// </summary>
public class JsonDotNetExample : MonoBehaviour
{
    public class Enemy
    {
        public string Name { get; set; }
        public int AttackDamage { get; set; }
        public int MaxHealth { get; set; }
    }
    private void Start()
    {
        string json = @"{
        'Name': 'Ninja',
        'AttackDamage': '40'
        }";
        var enemy = JsonConvert.DeserializeObject<Enemy>(json);
        Debug.Log($"{enemy.Name} deals {enemy.AttackDamage} damage.");
        // Output: Ninja deals 40 damage.
    }
}


