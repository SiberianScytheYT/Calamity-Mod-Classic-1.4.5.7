using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.OldDuke;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class OldDukeBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Treasure Bag");
/*
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
*/
            ItemID.Sets.BossBag[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.expert = true;
            Item.rare = 10;
        }

        public override bool CanRightClick() => true;

        public override void PostUpdate() => CalamityUtils.ForceItemIntoWorld(Item);

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Money
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<OldDuke>()));
            
            // Weapons
            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<InsidiousImpaler>(),
                ModContent.ItemType<FetidEmesis>(),
                ModContent.ItemType<SepticSkewer>(),
                ModContent.ItemType<VitriolicViper>(),
                ModContent.ItemType<CadaverousCarrion>(),
                ModContent.ItemType<ToxicantTwister>()
            }));

            // Equipment
            itemLoot.Add(ModContent.ItemType<DukeScales>(), 10);
            itemLoot.Add(ModContent.ItemType<MutatedTruffle>());

            // Vanity
            itemLoot.Add(ModContent.ItemType<OldDukeMask>(), 7);
        }
    }
}
