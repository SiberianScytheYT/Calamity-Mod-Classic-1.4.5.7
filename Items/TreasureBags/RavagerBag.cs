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
using Terraria.GameContent.ItemDropRules;
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

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            itemLoot.AddIf(() => !CalamityWorld.downedProvidence, ModContent.ItemType<FleshyGeodeT1>());
            itemLoot.AddIf(() => CalamityWorld.downedProvidence, ModContent.ItemType<FleshyGeodeT2>());

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<UltimusCleaver>(),
                ModContent.ItemType<RealmRavager>(),
                ModContent.ItemType<Hematemesis>(),
                ModContent.ItemType<SpikecragStaff>(),
                ModContent.ItemType<CraniumSmasher>()
            }));

            itemLoot.Add(ItemDropRule.OneFromOptions(20, ModContent.ItemType<CorpusAvertorMelee>(), ModContent.ItemType<CorpusAvertor>()));

            // Equipment
            itemLoot.Add(ModContent.ItemType<BloodPact>(), 2);
            itemLoot.Add(ModContent.ItemType<FleshTotem>(), 2);
            itemLoot.AddIf(() => CalamityWorld.downedProvidence, ModContent.ItemType<BloodflareCore>());
            itemLoot.AddIf((info) => CalamityWorld.revenge && !info.player.Calamity().rageBoostTwo, ModContent.ItemType<InfernalBlood>());

            // Vanity
            itemLoot.Add(ModContent.ItemType<RavagerMask>(), 7);
        }
    }
}
