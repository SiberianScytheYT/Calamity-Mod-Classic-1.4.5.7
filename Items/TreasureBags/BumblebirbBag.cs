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
using Terraria.GameContent.ItemDropRules;

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

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            itemLoot.Add(ModContent.ItemType<EffulgentFeather>(), 1, 15, 21);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<GildedProboscis>(),
                ModContent.ItemType<GoldenEagle>(),
                ModContent.ItemType<RougeSlash>()
            }));

            itemLoot.Add(ModContent.ItemType<Swordsplosion>(), DropHelper.RareVariantDropRateInt);

            // Equipment
            itemLoot.Add(ModContent.ItemType<DynamoStemCells>());
            itemLoot.Add(ModContent.ItemType<BirdSeed>(), 3);
            
            itemLoot.AddIf(info => CalamityWorld.revenge && !info.player.Calamity().rageBoostThree, ModContent.ItemType<RedLightningContainer>());

            // Vanity
            itemLoot.Add(ModContent.ItemType<BumblefuckMask>(), 7);
        }
    }
}
