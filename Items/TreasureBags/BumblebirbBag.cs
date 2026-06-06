using CalRD.Items.Materials;
using CalRD.Items.Mounts;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Accessories;
using CalRD.NPCs.Bumblebirb;
using CalRD.World;
using Terraria;
using Terraria.ModLoader;
using CalRD.Items.Armor.Vanity;

namespace CalRD.Items.TreasureBags
{
    public class BumblebirbBag : ModItem
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

        public override void RightClick(Player player)
        {
            player.TryGettingDevArmor(player.GetSource_FromThis());

            // Materials
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<EffulgentFeather>(), 15, 21);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<GildedProboscis>(w),
                DropHelper.WeightStack<GoldenEagle>(w),
                DropHelper.WeightStack<RougeSlash>(w)
            );

            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<Swordsplosion>(), DropHelper.RareVariantDropRateInt);

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<DynamoStemCells>());
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<BirdSeed>(), 3);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<RedLightningContainer>(), CalamityWorld.revenge && !player.Calamity().rageBoostThree);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<BumblefuckMask>(), 7);
        }
    }
}
