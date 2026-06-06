using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Pets;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.PlaguebringerGoliath;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class PlaguebringerGoliathBag : ModItem
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

        public override void RightClick(Player player)
        {
            player.TryGettingDevArmor(player.GetSource_FromThis());

            // Materials
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<PlagueCellCluster>(), 13, 17);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<InfectedArmorPlating>(), 16, 20);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Stinger, 4, 8);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<VirulentKatana>(w), // Virulence
                DropHelper.WeightStack<DiseasedPike>(w),
                DropHelper.WeightStack<ThePlaguebringer>(w), // Pandemic
                DropHelper.WeightStack<Malevolence>(w),
                DropHelper.WeightStack<PestilentDefiler>(w),
                DropHelper.WeightStack<TheHive>(w),
                DropHelper.WeightStack<MepheticSprayer>(w), // Blight Spewer
                DropHelper.WeightStack<PlagueStaff>(w),
                DropHelper.WeightStack<FuelCellBundle>(w),
                DropHelper.WeightStack<InfectedRemote>(w),
                DropHelper.WeightStack<TheSyringe>(w)
            );

            float malachiteChance = DropHelper.LegendaryDropRateFloat;
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<Malachite>(), CalamityWorld.revenge, malachiteChance);

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<ToxicHeart>());
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<BloomStone>(), 10);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<PlaguebringerGoliathMask>(), 7);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<PlagueCaller>(), 10);
        }
    }
}
