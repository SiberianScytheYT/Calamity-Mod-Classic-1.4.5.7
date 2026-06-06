using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class Judgement : ModItem
    {
        public const int HitsPerFlash = 300;
        public const int FlashBaseDamage = 80000;
        
        private const float SpawnAngleSpread = 0.4f * MathHelper.Pi;
        private const float SpeedRandomness = 0.08f;
        private const float Inaccuracy = 0.04f;
        private const float MinSpawnDist = 40;
        private const float MaxSpawnDist = 140;

        public static Color GetLightColor(float deviation) => new Color(1f, 0.5f + 0.35f * MathHelper.Clamp(deviation, 0f, 1f), 1f);
        public static Color GetSyncedLightColor() => GetLightColor(Main.DiscoG / 255f);
        public static Color GetRandomLightColor() => GetLightColor(Main.rand.NextFloat());

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Dance of Light");
/*
            Tooltip.SetDefault("Barrages enemies with a hailstorm of Light Blades\n'And in a flash of light, nothing remains'");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 42;
            Item.damage = 2077;
            Item.crit += 20;
            Item.knockBack = 4f;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 6;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 2;
            Item.useAnimation = 8;
            Item.reuseDelay = 5;
            Item.UseSound = SoundID.Item105;
            Item.autoReuse = true;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<LightBlade>();
            Item.shootSpeed = 14f;

            Item.value = Item.buyPrice(platinum: 5);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override Vector2? HoldoutOffset()
        {
            return Vector2.Zero;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile[] pair = new Projectile[2];
            for(int i = 0; i < 2; ++i)
            {
                // Pick a random direction somewhere behind the player
                float shootAngle = (float)Math.Atan2(velocity.Y, velocity.X);
                Vector2 offset = Main.rand.NextVector2Unit(MathHelper.Pi - SpawnAngleSpread, 2f * SpawnAngleSpread).RotatedBy(shootAngle);
                // Set how far away this sword is spawning
                offset *= Main.rand.NextFloat(MinSpawnDist, MaxSpawnDist);

                Vector2 spawnPos = position + offset;
                float randSpeed = Main.rand.NextFloat(1f - SpeedRandomness, 1f + SpeedRandomness);
                float randAngle = Main.rand.NextFloat(-Inaccuracy, Inaccuracy);
                Vector2 velocity1 = randSpeed * new Vector2(velocity.X, velocity.Y).RotatedBy(randAngle);
                Projectile p = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), spawnPos, velocity1, type, damage, Item.knockBack, player.whoAmI);
                pair[i] = p;
            }

            // Pair the swords up so they home in on each other
            pair[0].ai[1] = pair[1].whoAmI + 1f;
            pair[1].ai[1] = pair[0].whoAmI + 1f;
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SkyFracture);
            recipe.AddIngredient(ItemID.LunarFlareBook);
            recipe.AddIngredient(ModContent.ItemType<WrathoftheAncients>());
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
