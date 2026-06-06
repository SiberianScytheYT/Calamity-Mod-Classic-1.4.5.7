using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Leviathan;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class LeviathanBag : ModItem
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

        public override void PostUpdate() => CalamityUtils.ForceItemIntoWorld(Item);

        public override void RightClick(Player player)
        {
            // siren & levi are available PHM, so this check is necessary to keep vanilla consistency
            if (Main.hardMode)
                player.TryGettingDevArmor(player.GetSource_FromThis());

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<Greentide>(w),
                DropHelper.WeightStack<Leviatitan>(w),
                DropHelper.WeightStack<SirensSong>(w),
                DropHelper.WeightStack<Atlantis>(w),
                DropHelper.WeightStack<GastricBelcherStaff>(w),
                DropHelper.WeightStack<BrackishFlask>(w),
                DropHelper.WeightStack<LeviathanTeeth>(w)
            );

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<LeviathanAmbergris>());
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<LureofEnthrallment>(), 3);
            float communityChance = DropHelper.LegendaryDropRateFloat;
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<TheCommunity>(), CalamityWorld.revenge, communityChance);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<LeviathanMask>(), 7);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AnahitaMask>(), 7);

            // Fishing
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.HotlineFishingHook, 10);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.BottomlessBucket, 10);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.SuperAbsorbantSponge, 10);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.FishingPotion, 5, 5, 8);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.SonarPotion, 5, 5, 8);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.CratePotion, 5, 5, 8);
        }
    }
}
