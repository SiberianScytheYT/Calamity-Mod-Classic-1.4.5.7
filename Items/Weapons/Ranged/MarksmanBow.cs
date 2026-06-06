using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class MarksmanBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Marksman Bow");
/*
            Tooltip.SetDefault("Fires three arrows at a time\n" +
			"Wooden arrows are converted into Jester's arrows");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.DamageType = DamageClass.Ranged;
            Item.crit += 20;
            Item.width = 36;
            Item.height = 110;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.JestersArrow;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Arrow;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-4, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			//Convert wooden arrows to Jester's Arrows
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ProjectileID.JestersArrow;

            for (int i = 0; i < 3; i++)
            {
                float SpeedX = velocity.X + Main.rand.NextFloat(-10f, 10f) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.NextFloat(-10f, 10f) * 0.05f;
                int arrow = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[arrow].noDropItem = true;
				Main.projectile[arrow].extraUpdates += Main.rand.Next(3); //0 to 2 extra updates
				if (type == ProjectileID.JestersArrow)
				{
					Main.projectile[arrow].localNPCHitCooldown = 10;
					Main.projectile[arrow].usesLocalNPCImmunity = true;
				}
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Ectoplasm, 31);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
