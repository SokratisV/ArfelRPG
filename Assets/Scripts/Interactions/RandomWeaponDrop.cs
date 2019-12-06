using RPG.Combat;
using UnityEngine;

namespace RPG.Interactions
{
    [CreateAssetMenu(fileName = "LootTable", menuName = "Loot/Make New LootTable", order = 0)]
    public class RandomWeaponDrop : ScriptableObject
    {
        [SerializeField] WeaponPickup[] lootTable = null;
        Transform pickupManager;
        private void Start()
        {
            pickupManager = GameObject.FindWithTag("PickupManager").transform;
        }
        public void GenerateLoot(Transform[] dropLocations)
        {
            if (lootTable.Length == 0) return;
            if (dropLocations.Length == 0) return;
            WeaponPickup weapon = lootTable[Random.Range(0, lootTable.Length)];
            Transform dropLocation = dropLocations[Random.Range(0, dropLocations.Length)];
            Spawn(weapon, dropLocation);
        }

        private void Spawn(WeaponPickup weapon, Transform dropLocation)
        {
            Instantiate(weapon, dropLocation).transform.parent = pickupManager;
        }
    }
}