using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Pets;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Yharon;
using CalRD.World;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class YharonBag : ModItem
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
        
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Weapons
            itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<DragonRage>(),
                ModContent.ItemType<TheBurningSky>(),
                ModContent.ItemType<DragonsBreath>(),
                ModContent.ItemType<ChickenCannon>(),
                ModContent.ItemType<PhoenixFlameBarrage>(),
                ModContent.ItemType<AngryChickenStaff>(), // Yharon Kindle Staff
                ModContent.ItemType<ProfanedTrident>(), // Infernal Spear
                ModContent.ItemType<FinalDawn>()
            }));

            // Equipment
            itemLoot.Add(ModContent.ItemType<YharimsGift>());

            // Vanity
            itemLoot.Add(ModContent.ItemType<YharonMask>(), 7);
            itemLoot.Add(ModContent.ItemType<ForgottenDragonEgg>(), 10);
            itemLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<FoxDrive>());
        }
    }
}
