using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class DraedonsHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Draedon's Heart");
/*
            Tooltip.SetDefault("Gives 10% increased damage while you have the absolute rage buff\n" +
                "Increases your chance of getting the absolute rage buff\n" +
                "Boosts your damage by 5% and max movement speed and acceleration by 5%\n" +
                "Rage mode does more damage\n" +
                "You gain rage over time\n" +
				"Converts certain debuffs into buffs and extends their durations\n" +
				"Debuffs affected: Darkness, Blackout, Confused, Slow, Weak, Broken Armor,\n" +
				"Armor Crunch, War Cleave, Chilled, Ichor and Obstructed\n" +
                "Receiving a hit causes you to only lose half of your max adrenaline rather than all of it\n" +
                "Standing still regenerates your life quickly and boosts your defense by 25");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity14BuyPrice;
            Item.accessory = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.laudanum = true;
            modPlayer.stressPills = true;
            modPlayer.draedonsHeart = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<HeartofDarkness>());
            recipe.AddIngredient(ModContent.ItemType<StressPills>());
            recipe.AddIngredient(ModContent.ItemType<Laudanum>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>());
            recipe.AddIngredient(ItemID.Nanites, 250);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
