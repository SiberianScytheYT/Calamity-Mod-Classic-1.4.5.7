using CalRD.World;
using CalRD.Items.Accessories;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.AquaticScourge;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Items.Armor.Vanity;

namespace CalRD.Items.TreasureBags
{
    public class AquaticScourgeBag : ModItem
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

        public override void PostUpdate() => CalamityUtils.ForceItemIntoWorld(Item);

        public override void RightClick(Player player)
        {
            // AS is available PHM, so this check is necessary to keep vanilla consistency
            if (Main.hardMode)
                player.TryGettingDevArmor(player.GetSource_FromThis());

            // Materials
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<VictoryShard>(), 15, 25);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Coral, 7, 11);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Seashell, 7, 11);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Starfish, 7, 11);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<SubmarineShocker>(w),
                DropHelper.WeightStack<Barinautical>(w),
                DropHelper.WeightStack<Downpour>(w),
                DropHelper.WeightStack<DeepseaStaff>(w),
                DropHelper.WeightStack<ScourgeoftheSeas>(w)
            );

            float searingChance = DropHelper.LegendaryDropRateFloat;
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<SeasSearing>(), CalamityWorld.revenge, searingChance);

            // Equipment
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<AquaticEmblem>());
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AeroStone>(), 8);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<CorrosiveSpine>(), CalamityWorld.revenge, 0.25f);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AquaticScourgeMask>(), 7);

            // Fishing
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<BleachedAnglingKit>());
        }
    }
}
