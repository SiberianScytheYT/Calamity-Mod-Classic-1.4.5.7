using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.OldDuke;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class OldDukeBag : ModItem
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
            Item.rare = 10;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void PostUpdate() => CalamityUtils.ForceItemIntoWorld(Item);

        public override void RightClick(Player player)
        {
            player.TryGettingDevArmor(player.GetSource_FromThis());

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<InsidiousImpaler>(w),
                DropHelper.WeightStack<FetidEmesis>(w),
                DropHelper.WeightStack<SepticSkewer>(w),
                DropHelper.WeightStack<VitriolicViper>(w),
                DropHelper.WeightStack<CadaverousCarrion>(w),
                DropHelper.WeightStack<ToxicantTwister>(w)
            );

            // Equipment
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<DukeScales>(), 10);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<MutatedTruffle>());

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<OldDukeMask>(), 7);
        }
    }
}
