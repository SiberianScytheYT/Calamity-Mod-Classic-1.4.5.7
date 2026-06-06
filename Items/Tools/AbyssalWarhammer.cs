using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
    public class AbyssalWarhammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Abyssal Warhammer");
        }

        public override void SetDefaults()
        {
            Item.damage = 42;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 70;
            Item.height = 70;
            Item.useTime = 14;
            Item.useAnimation = 38;
            Item.useTurn = true;
            Item.hammer = 110;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 29);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 120);
        }
    }
}
