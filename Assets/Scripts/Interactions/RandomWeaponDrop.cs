using RPG.Combat;
using UnityEngine;

namespace RPG.Interactions
{
    [CreateAssetMenu(fileName = "LootTable", menuName = "Loot/Make New LootTable", order = 0)]
    public class RandomWeaponDrop : ScriptableObject
    {
        [SerializeField] WeaponPickup[] lootTable = null;

        public void GenerateLoot(Transform[] dropLocations, Transform pickupParentObject)
        {
            if (lootTable.Length == 0) return;
            if (dropLocations.Length == 0) return;
            WeaponPickup weapon = lootTable[Random.Range(0, lootTable.Length)];
            Transform dropLocation = dropLocations[Random.Range(0, dropLocations.Length)];
            Spawn(weapon, dropLocation, pickupParentObject);
        }

        private void Spawn(WeaponPickup weapon, Transform dropLocation, Transform parent)
        {
            if (parent != null)
            {
                Instantiate(weapon, dropLocation).transform.parent = parent;
            }
            else
            {
                Instantiate(weapon, dropLocation);
            }
        }
    }
}