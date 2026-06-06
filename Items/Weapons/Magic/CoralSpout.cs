using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class CoralSpout : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Coral Spout");
/*
            Tooltip.SetDefault("Casts coral water spouts that lay on the ground and damage enemies");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item17;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CoralSpike>();
            Item.shootSpeed = 16f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VictideBar>(), 2);
            recipe.AddIngredient(ItemID.Coral, 5);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
