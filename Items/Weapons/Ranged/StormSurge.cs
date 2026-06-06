using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class StormSurge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Storm Surge");
/*
            Tooltip.SetDefault("Fear the storm\n" +
                "Does not consume ammo");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 58;
            Item.height = 22;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item122;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<StormSurgeTornado>();
            Item.shootSpeed = 12f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StormlionMandible>());
            recipe.AddIngredient(ModContent.ItemType<VictideBar>(), 2);
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
