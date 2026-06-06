using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Materials;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Typeless
{
    public class Aestheticus : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aestheticus");
/*
            Tooltip.SetDefault("Fires crystals that explode and slow enemies down\n" +
                "This weapon scales with all your damage stats at once");
*/
		}

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.damage = 8;
            Item.rare = 3;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item109;
            Item.autoReuse = true;
            Item.height = 58;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.shoot = ModContent.ProjectileType<CursorProj>();
            Item.shootSpeed = 5f;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        // Aestheticus scales off of all damage types simultaneously (meaning it scales 5x from universal damage boosts).
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float formula = 5f * (player.GetDamage(DamageClass.Generic).Multiplicative - 1f);
            formula += player.GetDamage(DamageClass.Melee).Additive - 1f;
            formula += player.GetDamage(DamageClass.Ranged).Additive - 1f;
            formula += player.GetDamage(DamageClass.Magic).Additive - 1f;
            formula += player.GetDamage(DamageClass.Summon).Additive - 1f;
            formula += player.Calamity().throwingDamage - 1f;
            damage *= formula;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(ItemID.MeteoriteBar, 10);
			recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 5);
            recipe.AddIngredient(ItemID.Glass, 20);
            recipe.AddIngredient(ItemID.Gel, 15);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Vaporfied>(), 180);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<Vaporfied>(), 180);
        }
    }
}
