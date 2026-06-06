using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TrueBloodyEdge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("True Bloody Edge");
/*
            Tooltip.SetDefault("Chance to heal the player on enemy hits\n" +
                "Fires a bloody blade");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 48;
            Item.damage = 70;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 24;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 64;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<BloodyBlade>();
            Item.shootSpeed = 11f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BloodyEdge>());
            recipe.AddIngredient(ItemID.BrokenHeroSword);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type == NPCID.TargetDummy || player.moonLeech)
            {
                return;
            }
            int healAmount = Main.rand.Next(6) + 1;
            if (Main.rand.NextBool(2))
            {
                player.statLife += healAmount;
                player.HealEffect(healAmount);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
			if (player.moonLeech)
				return;
            int healAmount = Main.rand.Next(6) + 1;
            if (Main.rand.NextBool(2))
            {
                player.statLife += healAmount;
                player.HealEffect(healAmount);
            }
        }
    }
}
