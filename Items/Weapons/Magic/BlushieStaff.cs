using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class BlushieStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Staff of Blushie");
/*
            Tooltip.SetDefault("Hold your mouse, wait, wait, wait, and put your trust in the power of blue magic");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.channel = true;
            Item.noMelee = true;
            Item.damage = 1;
            Item.knockBack = 1f;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.DamageType = DamageClass.Magic;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<BlushieStaffProj>();
            Item.mana = 200;
            Item.shootSpeed = 0f;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Phantoplasm>(), 100);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 50);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
