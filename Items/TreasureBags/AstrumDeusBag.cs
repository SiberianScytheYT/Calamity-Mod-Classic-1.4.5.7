using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Pets;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.AstrumDeus;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class AstrumDeusBag : ModItem
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
            Item.expert = true;
            Item.rare = 9;
        }

        public override bool CanRightClick() => true;

        public override void PostUpdate() => CalamityUtils.ForceItemIntoWorld(Item);

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            itemLoot.Add(ModContent.ItemType<Stardust>(), 1, 60, 90);
            itemLoot.Add(ItemID.FallenStar, 1, 100, 180);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<TheMicrowave>(),
                ModContent.ItemType<StarSputter>(),
                ModContent.ItemType<Starfall>(),
                ModContent.ItemType<GodspawnHelixStaff>(),
                ModContent.ItemType<RegulusRiot>()
            }));

            itemLoot.Add(ModContent.ItemType<Quasar>(), DropHelper.RareVariantDropRateInt);

            // Equipment
            itemLoot.AddRIV(ModContent.ItemType<AstralBulwark>(), ModContent.ItemType<HideofAstrumDeus>(), 1, DropHelper.RareVariantDropRateInt);
            itemLoot.Add(ModContent.ItemType<ChromaticOrb>(), 5);

            // Vanity
            itemLoot.Add(ModContent.ItemType<AstrumDeusMask>(), 7);
        }
    }
}
