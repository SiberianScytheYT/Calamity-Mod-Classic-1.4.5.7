
using CalRD.Items.Accessories;
using CalRD.Items.Accessories.Wings;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Cryogen;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class CryogenBag : ModItem
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
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<CryoBar>(), 20, 40);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<EssenceofEleum>(), 5, 9);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.FrostCore);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<Avalanche>(w),
                DropHelper.WeightStack<GlacialCrusher>(w),
                DropHelper.WeightStack<EffluviumBow>(w),
                DropHelper.WeightStack<BittercoldStaff>(w),
                DropHelper.WeightStack<SnowstormStaff>(w),
                DropHelper.WeightStack<Icebreaker>(w)
            );

            float divinityChance = DropHelper.LegendaryDropRateFloat;
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<ColdDivinity>(), CalamityWorld.revenge, divinityChance);

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<SoulofCryogen>());
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<FrostFlare>(), CalamityWorld.revenge);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<CryoStone>(), 10);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<Regenator>(), DropHelper.RareVariantDropRateInt);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<CryogenMask>(), 7);

            // Other
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.FrozenKey, 5);
        }
    }
}
