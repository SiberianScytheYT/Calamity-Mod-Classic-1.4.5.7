
using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.NPCs.Crabulon;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class CrabulonBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Treasure Bag");
/*
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
*/
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = 9;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            // Materials
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.GlowingMushroom, 25, 35);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.MushroomGrassSeeds, 5, 10);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<MycelialClaws>(w),
                DropHelper.WeightStack<Fungicide>(w),
                DropHelper.WeightStack<HyphaeRod>(w),
                DropHelper.WeightStack<Mycoroot>(w),
                DropHelper.WeightStack<Shroomerang>(w)
            );

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<FungalClump>());
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<MushroomPlasmaRoot>(), CalamityWorld.revenge && !player.Calamity().rageBoostOne);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<CrabulonMask>(), 7);
        }
    }
}
