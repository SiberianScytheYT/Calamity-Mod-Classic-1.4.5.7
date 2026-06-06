using CalRD.Items.Materials;
using CalRD.Items.Placeables.Ores;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Mounts
{
    public class GazeOfCrysthamyr : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gaze of Crysthamyr");
/*
            Tooltip.SetDefault("Summons a shadow dragon");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = 10;
            Item.value = Item.buyPrice(3, 0, 0, 0);
            Item.UseSound = SoundID.NPCHit56;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<Crysthamyr>();
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DD2PetDragon);
            recipe.AddIngredient(ItemID.SoulofNight, 100);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 50);
            recipe.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 25);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
