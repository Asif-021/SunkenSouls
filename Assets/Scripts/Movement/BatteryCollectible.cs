//using UnityEngine;

//public class BatteryCollectible : MonoBehaviour
//{
//    public GameObject player;          // Drag and drop the Player GameObject here in the Inspector
//    public float healthAmount = 25f;   // Amount to replenish
//    public float collectionRadius = 1f; // Radius within which the battery is collected

//    void Update()
//    {
//        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= collectionRadius)
//        {
//            CollectBattery();
//        }
//    }

//    private void CollectBattery()
//    {
//        // Access the HealthDeplete component on the player and replenish health
//        SunkenSouls.HealthDeplete healthDeplete = player.GetComponent<SunkenSouls.HealthDeplete>();
//        if (healthDeplete != null)
//        {
//            healthDeplete.ReplenishHealth(healthAmount);
//        }

//        // Destroy the battery collectible
//        Destroy(gameObject);
//    }

//    void OnDrawGizmosSelected()
//    {
//        // Visualize the collection radius in the editor
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(transform.position, collectionRadius);
//    }
//}
