using CalRD.Projectiles.Magic;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class RecitationoftheBeast : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Recitation of the Beast");
/*
            Tooltip.SetDefault("A thousand years sealed in the demon's realm will teach you a thing or two\n" +
							   "Summons beast scythes around the player in a small circle,\n" +
                               "before firing toward the cursor and home in to nearby enemies");
*/
        }

        public override void SetDefaults()
        {
			Item.mana = 24;
            Item.width = 38;
            Item.height = 34;
            Item.damage = 300;
            Item.crit += 20;
            Item.noMelee = true;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 18;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<BeastScythe>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Magic;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			float spread = 60f * 0.0174f;
			double startAngle = Math.Atan2(velocity.X, velocity.Y) - spread / 2;
			double deltaAngle = spread / 6f;
			double offsetAngle;
			int i;
			for (i = 0; i < 3; i++)
			{
				offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, (float)(Math.Sin(offsetAngle) * 2f), (float)(Math.Cos(offsetAngle) * 2f), type, damage, Item.knockBack, Main.myPlayer, 0f, 0f);
				Projectile.NewProjectile(source, player.Center.X, player.Center.Y, (float)(-Math.Sin(offsetAngle) * 2f), (float)(-Math.Cos(offsetAngle) * 2f), type, damage, Item.knockBack, Main.myPlayer, 0f, 0f);
			}
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemonScythe);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 3);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 8);
            recipe.AddTile(TileID.Bookcases);
            recipe.Register();
        }
    }
}
