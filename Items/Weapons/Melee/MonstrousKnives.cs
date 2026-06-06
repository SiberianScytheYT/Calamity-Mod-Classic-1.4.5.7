using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
	public class MonstrousKnives : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Monstrous Knives");
/*
            Tooltip.SetDefault("Throws a spread of knives that can heal the user");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.damage = 4;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 21;
            Item.useStyle = 1;
            Item.useTime = 21;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item39;
            Item.autoReuse = true;
            Item.height = 20;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
            Item.shoot = ModContent.ProjectileType<MonstrousKnife>();
            Item.shootSpeed = 15f;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float speed = Item.shootSpeed;
            Vector2 playerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float xDist = Main.mouseX + Main.screenPosition.X - playerPos.X;
            float yDist = Main.mouseY + Main.screenPosition.Y - playerPos.Y;
            if (player.gravDir == -1f)
            {
                yDist = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - playerPos.Y;
            }
			Vector2 vector = new Vector2(xDist, yDist);
			float speedMult = vector.Length();
            if ((float.IsNaN(xDist) && float.IsNaN(yDist)) || (xDist == 0f && yDist == 0f))
            {
                xDist = player.direction;
                yDist = 0f;
                speedMult = speed;
            }
            else
            {
                speedMult = speed / speedMult;
            }
            xDist *= speedMult;
            yDist *= speedMult;
            int knifeAmt = 3;
            if (Main.rand.NextBool(2))
            {
                knifeAmt++;
            }
            if (Main.rand.NextBool(4))
            {
                knifeAmt++;
            }
            if (Main.rand.NextBool(8))
            {
                knifeAmt++;
            }
            if (Main.rand.NextBool(16))
            {
                knifeAmt++;
            }
            for (int i = 0; i < knifeAmt; i++)
            {
                float xVec = xDist;
                float yVec = yDist;
                float spreadMult = 0.05f * i;
                xVec += Main.rand.NextFloat(-25f, 25f) * spreadMult;
                yVec += Main.rand.NextFloat(-25f, 25f) * spreadMult;
				Vector2 directionToShoot = new Vector2(xVec, yVec);
				speedMult = directionToShoot.Length();
                speedMult = speed / speedMult;
                xVec *= speedMult;
                yVec *= speedMult;
                directionToShoot = new Vector2(xVec, yVec);
                Projectile.NewProjectile(source, playerPos, directionToShoot, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ThrowingKnife, 200);
            recipe.AddIngredient(ItemID.LifeCrystal);
            recipe.AddIngredient(ItemID.LesserHealingPotion, 5); //I considered making a recipe group for any healing potion but Merchant sells these
            recipe.AddIngredient(ModContent.ItemType<VictoryShard>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
