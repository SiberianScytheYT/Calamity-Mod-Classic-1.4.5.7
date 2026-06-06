using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
	public class EventHorizon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Event Horizon");
/*
            Tooltip.SetDefault("Nothing, not even light, can return.\n" +
			"Fires a ring of stars to home in on nearby enemies\n" +
			"Stars spawn black holes on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;

            Item.damage = 369;
            Item.knockBack = 3.5f;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;

            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Violet;
            Item.value = Item.buyPrice(2, 50, 0, 0);

            Item.UseSound = SoundID.Item84;
            Item.shoot = ModContent.ProjectileType<EventHorizonStar>();
            Item.shootSpeed = 25f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (float i = 0; i < 8; i++)
            {
                float angle = MathHelper.TwoPi / 8f * i;
                Projectile.NewProjectile(source, player.Center, angle.ToRotationVector2() * 8f, type, damage, Item.knockBack, player.whoAmI, angle, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Starfall>());
            recipe.AddIngredient(ModContent.ItemType<NuclearFury>());
            recipe.AddIngredient(ModContent.ItemType<RelicofRuin>());
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 15);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
