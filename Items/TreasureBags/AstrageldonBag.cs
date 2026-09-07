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
using Terraria.GameContent.ItemDropRules;
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

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            itemLoot.Add(ModContent.ItemType<AstralJelly>(), 1, 12, 16);
            itemLoot.Add(ModContent.ItemType<Stardust>(), 1, 30, 40);
            itemLoot.Add(ItemID.FallenStar, 1, 30, 50);

            // Weapons
            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<Nebulash>(),
                ModContent.ItemType<AuroraBlazer>(),
                ModContent.ItemType<AlulaAustralis>(),
                ModContent.ItemType<BorealisBomber>(),
                ModContent.ItemType<AuroradicalThrow>()
            }));

            int leonidChance = DropHelper.LegendaryDropRateInt;
            itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<LeonidProgenitor>(),  leonidChance);

            // Equipment
            itemLoot.AddIf(() => CalamityWorld.revenge && NPC.downedMoonlord, ModContent.ItemType<SquishyBeanMount>());
            itemLoot.Add(ModContent.ItemType<GravistarSabaton>());

            // Vanity
            itemLoot.Add(ModContent.ItemType<AureusMask>(), 7);

            // Other
            itemLoot.AddIf(info => CalamityWorld.revenge && !info.player.Calamity().adrenalineBoostTwo, ModContent.ItemType<StarlightFuelCell>());
            itemLoot.Add(ItemID.HallowedKey, 5);
        }
    }
}
