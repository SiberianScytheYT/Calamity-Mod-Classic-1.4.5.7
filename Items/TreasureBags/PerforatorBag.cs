using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Pets;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Perforator;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class PerforatorBag : ModItem
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
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Vertebrae, 10, 20);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.CrimtaneBar, 9, 14);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<BloodSample>(), 30, 40);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ItemID.Ichor, Main.hardMode, 15, 30);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<VeinBurster>(w),
                DropHelper.WeightStack<BloodyRupture>(w),
                DropHelper.WeightStack<SausageMaker>(w),
                DropHelper.WeightStack<Aorta>(w),
                DropHelper.WeightStack<Eviscerator>(w),
                DropHelper.WeightStack<BloodBath>(w),
                DropHelper.WeightStack<BloodClotStaff>(w),
                DropHelper.WeightStack<ToothBall>(w, 50, 75)
            );

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<BloodyWormTooth>());
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<BloodstainedGlove>(), 3);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<PerforatorMask>(), 7);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<BloodyVein>(), 10);
        }
    }
}
