using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Mounts;
using CalRD.Items.Pets;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.BrimstoneElemental;
using CalRD.World;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class BrimstoneWaifuBag : ModItem
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

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            player.TryGettingDevArmor(player.GetSource_FromThis());

            // Materials
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<EssenceofChaos>(), 5, 9);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<Bloodstone>(), CalamityWorld.downedProvidence, 25, 35);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<Brimlance>(w),
                DropHelper.WeightStack<SeethingDischarge>(w),
                DropHelper.WeightStack<DormantBrimseeker>(w)
            );

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Abaddon>());
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Gehenna>());
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<RoseStone>(), 10);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<Brimrose>(), CalamityWorld.revenge && CalamityWorld.downedProvidence);

            // Vanity
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<CharredRelic>(), CalamityWorld.revenge);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<BrimstoneWaifuMask>(), 7);
        }
    }
}
