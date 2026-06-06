using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
    public class Onyxia : ModItem
    {
        const int NotConsumeAmmo = 50;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Onyxia");
/*
            Tooltip.SetDefault(NotConsumeAmmo.ToString() + "% chance to not consume ammo\n" +
                "Fires a storm of bullets and onyx shards");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 84;
            Item.height = 34;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.value = Item.buyPrice(2, 50, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item36;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder; // wait why
            Item.shootSpeed = 28f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-11, 3);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Fire the Onyx Shard that is characteristic of the Onyx Blaster
            // The shard deals triple damage and double knockback
            int shardDamage = (int)(2.5 * damage);
            float shardKB = 2f * Item.knockBack;
            float shardVelocityX = (velocity.X + (float)Main.rand.Next(-25, 26) * 0.05f) * 0.9f;
            float shardVelocityY = (velocity.Y + (float)Main.rand.Next(-25, 26) * 0.05f) * 0.9f;
            Projectile.NewProjectile(source, position.X, position.Y, shardVelocityX, shardVelocityY, ProjectileID.BlackBolt, shardDamage, shardKB, player.whoAmI, 0f, 0f);

            // Fire three symmetric pairs of bullets alongside it
            Vector2 baseVelocity = new Vector2(velocity.X, velocity.Y);
            for (int i = 0; i < 3; i++)
            {
                float randAngle = Main.rand.NextFloat(0.035f);
                float randVelMultiplier = Main.rand.NextFloat(0.92f, 1.08f);
                Vector2 left = baseVelocity.RotatedBy(-randAngle) * randVelMultiplier;
                Vector2 right = baseVelocity.RotatedBy(randAngle) * randVelMultiplier;
                Projectile.NewProjectile(source, position.X, position.Y, left.X, left.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Projectile.NewProjectile(source, position.X, position.Y, right.X, right.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < NotConsumeAmmo)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<OnyxChainBlaster>());
            r.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            r.AddIngredient(ModContent.ItemType<DarksunFragment>(), 10);
            r.AddTile(ModContent.TileType<DraedonsForge>());
            r.Register();
        }
    }
}
