using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.DesertScourge;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class DesertScourgeBag : ModItem
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
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<VictoryShard>(), 10, 16);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Coral, 7, 11);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Seashell, 7, 11);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Starfish, 7, 11);

            // Weapons
            // Set up the base drop set, which includes Scourge of the Desert at its normal drop chance.
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.WeightedItemStack[] weapons =
            {
                DropHelper.WeightStack<AquaticDischarge>(w),
                DropHelper.WeightStack<Barinade>(w),
                DropHelper.WeightStack<StormSpray>(w),
                DropHelper.WeightStack<SeaboundStaff>(w),
                DropHelper.WeightStack<ScourgeoftheDesert>(w),
            };

            // If the RIV roll for Dune Hopper succeeds, REPLACE Scourge of the Desert with a guaranteed Dune Hopper.
            float duneHopperChance = DropHelper.RareVariantDropRateFloat;
            if (Main.rand.NextFloat() < duneHopperChance)
                weapons[4] = DropHelper.WeightStack<DuneHopper>();

            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player, weapons);

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<OceanCrest>());
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AeroStone>(), 9);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<SandCloak>(), 9);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<DeepDiver>(), DropHelper.RareVariantDropRateInt);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<DesertScourgeMask>(), 7);

            // Fishing
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<SandyAnglingKit>());
        }
    }
}
