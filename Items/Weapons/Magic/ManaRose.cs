using CalRD.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Magic
{
    public class ManaRose : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mana Rose");
/*
            Tooltip.SetDefault("Casts a mana flower that explodes into petals");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ManaBolt>();
            Item.shootSpeed = 10f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(10, 10);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.NaturesGift);
            recipe.AddIngredient(ItemID.JungleRose);
            recipe.AddIngredient(ItemID.Moonglow, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
