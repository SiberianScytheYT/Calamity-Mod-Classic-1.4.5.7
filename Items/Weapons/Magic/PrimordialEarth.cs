using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class PrimordialEarth : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Primordial Earth");
/*
            Tooltip.SetDefault("Casts a large blast of dust");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 60;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 19;
            Item.width = 30;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SupremeDustProjectile>();
            Item.shootSpeed = 4f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DeathValley>());
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 3);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
