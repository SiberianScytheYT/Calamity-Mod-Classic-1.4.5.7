using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Ravager;
using CalRD.World;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class RavagerBag : ModItem
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
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<FleshyGeodeT1>(), !CalamityWorld.downedProvidence);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<FleshyGeodeT2>(), CalamityWorld.downedProvidence);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<UltimusCleaver>(w),
                DropHelper.WeightStack<RealmRavager>(w),
                DropHelper.WeightStack<Hematemesis>(w),
                DropHelper.WeightStack<SpikecragStaff>(w),
                DropHelper.WeightStack<CraniumSmasher>(w)
            );

            DropHelper.DropItemFromSetChance(player.GetSource_FromThis(), player, 0.05f, ModContent.ItemType<CorpusAvertorMelee>(), ModContent.ItemType<CorpusAvertor>());

            // Equipment
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<BloodPact>(), 0.5f);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<FleshTotem>(), 0.5f);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<BloodflareCore>(), CalamityWorld.downedProvidence);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<InfernalBlood>(), CalamityWorld.revenge && !player.Calamity().rageBoostTwo);

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<RavagerMask>(), 7);
        }
    }
}
