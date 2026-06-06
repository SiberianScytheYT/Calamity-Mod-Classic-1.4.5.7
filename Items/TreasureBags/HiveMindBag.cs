using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.HiveMind;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class HiveMindBag : ModItem
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
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.RottenChunk, 10, 20);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.DemoniteBar, 9, 14);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<TrueShadowScale>(), 30, 40);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ItemID.CursedFlame, Main.hardMode, 15, 30);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<PerfectDark>(w),
                DropHelper.WeightStack<LeechingDagger>(w),
                DropHelper.WeightStack<Shadethrower>(w),
                DropHelper.WeightStack<ShadowdropStaff>(w),
                DropHelper.WeightStack<ShaderainStaff>(w),
                DropHelper.WeightStack<DankStaff>(w),
                DropHelper.WeightStack<RotBall>(w, 50, 75)
            );

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<RottenBrain>());
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<FilthyGlove>(), 3);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<HiveMindMask>(), 7);
        }
    }
}
