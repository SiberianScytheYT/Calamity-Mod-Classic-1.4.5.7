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

        public override void RightClick(Player player)
        {
            player.TryGettingDevArmor(player.GetSource_FromThis());

            // Materials
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Stardust>(), 60, 90);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.FallenStar, 100, 180);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<TheMicrowave>(w),
                DropHelper.WeightStack<StarSputter>(w),
                DropHelper.WeightStack<Starfall>(w),
                DropHelper.WeightStack<GodspawnHelixStaff>(w),
                DropHelper.WeightStack<RegulusRiot>(w)
            );

            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<Quasar>(), DropHelper.RareVariantDropRateInt);

            // Equipment
            DropHelper.DropItemRIV(player.GetSource_FromThis(), player, ModContent.ItemType<AstralBulwark>(), ModContent.ItemType<HideofAstrumDeus>(), 1f, DropHelper.RareVariantDropRateFloat);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<ChromaticOrb>(), 5);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AstrumDeusMask>(), 7);
        }
    }
}
