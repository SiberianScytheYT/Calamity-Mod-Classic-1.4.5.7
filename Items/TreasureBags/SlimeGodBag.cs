using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.SlimeGod;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class SlimeGodBag : ModItem
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
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Gel, 30, 60);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<PurifiedGel>(), 35, 55);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<OverloadedBlaster>(w),
                DropHelper.WeightStack<AbyssalTome>(w),
                DropHelper.WeightStack<EldritchTome>(w),
                DropHelper.WeightStack<CorroslimeStaff>(w),
                DropHelper.WeightStack<CrimslimeStaff>(w)
            );

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<ManaOverloader>());
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<ElectrolyteGelPack>(), CalamityWorld.revenge && !player.Calamity().adrenalineBoostOne);

            // Vanity
            DropHelper.DropItemFromSetChance(player.GetSource_FromThis(), player, 0.142857f, ModContent.ItemType<SlimeGodMask>(), ModContent.ItemType<SlimeGodMask2>());

            // Other
        }
    }
}
