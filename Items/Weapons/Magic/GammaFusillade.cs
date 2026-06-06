using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class GammaFusillade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Biofusillade");
/*
            Tooltip.SetDefault("Unleashes a concentrated beam of life energy");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 110;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 3;
            Item.useAnimation = 3;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(1, 20, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GammaLaser>();
            Item.shootSpeed = 20f;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 8);
            recipe.AddIngredient(ItemID.SpellTome);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
