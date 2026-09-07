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

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            itemLoot.Add(ModContent.ItemType<VictoryShard>(), 1, 10, 16);
            itemLoot.Add(ItemID.Coral, 1, 7, 11);
            itemLoot.Add(ItemID.Seashell, 1, 7, 11);
            itemLoot.Add(ItemID.Starfish, 1, 7, 11);

            // Weapons
            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<AquaticDischarge>(),
                ModContent.ItemType<Barinade>(),
                ModContent.ItemType<StormSpray>(),
                ModContent.ItemType<SeaboundStaff>(),
                ModContent.ItemType<ScourgeoftheDesert>()
            }));
            
            // If the RIV roll for Dune Hopper succeeds, REPLACE Scourge of the Desert with a guaranteed Dune Hopper.
            itemLoot.AddRIV(ModContent.ItemType<ScourgeoftheDesert>(), ModContent.ItemType<DuneHopper>(), 1, 1);

            // Equipment
            itemLoot.Add(ModContent.ItemType<OceanCrest>());
            itemLoot.Add(ModContent.ItemType<AeroStone>(), 9);
            itemLoot.Add(ModContent.ItemType<SandCloak>(), 9);
            itemLoot.Add(ModContent.ItemType<DeepDiver>(), DropHelper.RareVariantDropRateInt);

            // Vanity
            itemLoot.Add(ModContent.ItemType<DesertScourgeMask>(), 7);

            // Fishing
            itemLoot.Add(ModContent.ItemType<SandyAnglingKit>());
        }
    }
}
