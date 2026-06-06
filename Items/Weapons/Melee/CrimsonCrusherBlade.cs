using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class CrimsonCrusherBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Crimson Crusher Blade");
/*
            Tooltip.SetDefault("Inflicts ichor and critical hits lower enemy contact damage");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 38;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 70;
            Item.height = 80;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(7))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Ichor, 120);
            if (target.damage > 0 && hit.Crit && !CalamityPlayer.areThereAnyDamnBosses)
            {
                target.damage = target.defDamage - 5;
				if (target.damage < 1)
					target.damage = 1;
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Ichor, 120);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EbonianGel>(), 15);
            recipe.AddIngredient(ItemID.CrimstoneBlock, 50);
            recipe.AddIngredient(ItemID.TissueSample, 5);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 4);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
