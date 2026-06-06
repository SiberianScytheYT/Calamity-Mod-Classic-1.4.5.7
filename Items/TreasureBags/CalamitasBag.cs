using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Calamitas;
using CalRD.NPCs.NormalNPCs;
using CalRD.World;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class CalamitasBag : ModItem
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
            player.TryGettingDevArmor(player.GetSource_FromThis());

            // Materials
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<CalamityDust>(), 14, 18);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<BlightedLens>(), 1, 3);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<EssenceofChaos>(), 5, 9);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<Bloodstone>(), CalamityWorld.downedProvidence, 35, 45);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<TheEyeofCalamitas>(w),
                DropHelper.WeightStack<Animosity>(w),
                DropHelper.WeightStack<CalamitasInferno>(w),
                DropHelper.WeightStack<BlightedEyeStaff>(w)
            );

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<CalamityRing>());
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<ChaosStone>(), 10);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<CalamitasMask>(), 7);
        }
    }
}
