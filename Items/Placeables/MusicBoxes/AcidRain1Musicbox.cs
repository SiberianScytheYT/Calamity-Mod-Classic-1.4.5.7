using CalRD.Items.Materials;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ID;
using CalRD.Tiles.MusicBoxes;
using Terraria;

namespace CalRD.Items.Placeables.MusicBoxes
{
    public class AcidRain1Musicbox : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Music Box (Acidic Downpour)");
        }

        public override void SetDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<AcidRain1MusicboxTile>();
            Item.width = 24;
            Item.height = 24;
            Item.rare = 4;
            Item.value = 100000;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>());
            recipe.AddIngredient(ModContent.ItemType<Acidwood>(), 10);
            recipe.AddIngredient(ModContent.ItemType<CorrodedFossil>(), 10); 
            recipe.AddIngredient(ModContent.ItemType<SulfuricScale>(), 10);
            recipe.AddIngredient(ItemID.MusicBox);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}