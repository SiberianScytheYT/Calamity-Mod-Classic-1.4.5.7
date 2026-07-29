using CalRD.Items.Pets;
using CalRD.Items.DifficultyItems;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using Terraria;
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

        public override void RightClick(Player player)
        {
            // Weapons
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.CopperBroadsword);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.CopperBow);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.WoodenArrow, 100);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.AmethystStaff);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.ManaCrystal);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<SquirrelSquireStaff>());
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<ThrowingBrick>(), 150);

            // Tools / Utility
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.CopperHammer);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Bomb, 10);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.MiningPotion);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.SpelunkerPotion, 2);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.SwiftnessPotion, 3);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.GillsPotion, 2);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.ShinePotion);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.RecallPotion, 3);

            // Tiles / Placeables
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Torch, 25);
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.Chest, 3);

            // Difficulty items (doesn't drop in Normal)
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<Death>(), Main.expertMode);
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<DefiledRune>(), Main.expertMode);

            // Speedrun King Slime
            DropHelper.DropItem(player.GetSource_FromThis(), player, ItemID.SlimeCrown);

            // The Lad
            DropHelper.DropItemCondition(player.GetSource_FromThis(), player, ModContent.ItemType<JoyfulHeart>(), player.name == "Aleksh" || player.name == "Shark Lad");

            // Music box
            DropHelper.DropItem(player.GetSource_FromThis(), player, ModContent.ItemType<Placeables.MusicBoxes.CalamityMusicbox>());
        }
    }
}
