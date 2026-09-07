using CalRD.Items.Pets;
using CalRD.Items.DifficultyItems;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class StarterBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Starter Bag");
/*
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
*/
        }

        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = 1;
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Weapons
            itemLoot.Add(ItemID.CopperBroadsword);
            itemLoot.Add(ItemID.CopperBow);
            itemLoot.Add(ItemID.WoodenArrow, 1, 100, 100);
            itemLoot.Add(ItemID.AmethystStaff);
            itemLoot.Add(ItemID.ManaCrystal);
            itemLoot.Add(ModContent.ItemType<SquirrelSquireStaff>());
            itemLoot.Add(ModContent.ItemType<ThrowingBrick>(), 1, 150, 150);

            // Tools / Utility
            itemLoot.Add(ItemID.CopperHammer);
            itemLoot.Add(ItemID.Bomb, 1, 10, 10);
            itemLoot.Add(ItemID.MiningPotion);
            itemLoot.Add(ItemID.SpelunkerPotion, 1, 2, 2);
            itemLoot.Add(ItemID.SwiftnessPotion, 1, 3, 3);
            itemLoot.Add(ItemID.GillsPotion, 1, 2, 2);
            itemLoot.Add(ItemID.ShinePotion);
            itemLoot.Add(ItemID.RecallPotion, 1, 3, 3);

            // Tiles / Placeables
            itemLoot.Add(ItemID.Torch, 1, 25, 25);
            itemLoot.Add(ItemID.Chest, 1, 3, 3);

            // Difficulty items (doesn't drop in Normal)
            itemLoot.AddIf(() => Main.expertMode, ModContent.ItemType<Death>());
            itemLoot.AddIf(() => Main.expertMode, ModContent.ItemType<DefiledRune>());

            // Speedrun King Slime
            itemLoot.Add(ItemID.SlimeCrown);

            // The Lad
            itemLoot.AddIf(info => info.player.name == "Aleksh" || info.player.name == "Shark Lad", ModContent.ItemType<JoyfulHeart>());

            // Music box
            itemLoot.Add(ModContent.ItemType<Placeables.MusicBoxes.CalamityMusicbox>());
        }
    }
}
