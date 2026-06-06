using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.Mounts;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Potions;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Summon;
using CalRD.Items.Weapons.Rogue;
using CalRD.NPCs.AstrumAureus;
using CalRD.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class AstrageldonBag : ModItem
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
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<AstralJelly>(), 12, 16);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Stardust>(), 30, 40);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.FallenStar, 30, 50);

            // Weapons
            float w = DropHelper.BagWeaponDropRateFloat;
            DropHelper.DropEntireWeightedSet(player.GetSource_FromThis(), player,
                DropHelper.WeightStack<Nebulash>(w),
                DropHelper.WeightStack<AuroraBlazer>(w),
                DropHelper.WeightStack<AlulaAustralis>(w),
                DropHelper.WeightStack<BorealisBomber>(w),
                DropHelper.WeightStack<AuroradicalThrow>(w)
            );

            float leonidChance = DropHelper.LegendaryDropRateFloat;
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<LeonidProgenitor>(), CalamityWorld.revenge, leonidChance);

            // Equipment
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<SquishyBeanMount>(), CalamityWorld.revenge && NPC.downedMoonlord);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<GravistarSabaton>());

            // Vanity
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ModContent.ItemType<AureusMask>(), 7);

            // Other
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<StarlightFuelCell>(), CalamityWorld.revenge && !player.Calamity().adrenalineBoostTwo);
            DropHelper.DropItemChance(player.GetSource_FromThis(), player, ItemID.HallowedKey, 5);
        }
    }
}
