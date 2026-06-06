using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Tumbleweed : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tumbleweed");
/*
            Tooltip.SetDefault("Releases a rolling tumbleweed on hit");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 125;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 30;
            Item.height = 10;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 8f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.rare = 7;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<TumbleweedFlail>();
            Item.shootSpeed = 12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Sunfury);
            recipe.AddIngredient(ModContent.ItemType<GrandScale>());
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
