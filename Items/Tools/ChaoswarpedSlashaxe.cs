using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
    public class ChaoswarpedSlashaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tectonic Truncator");
        }

        public override void SetDefaults()
        {
            Item.damage = 68;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 6;
            Item.useAnimation = 31;
            Item.useTurn = true;
            Item.axe = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 9);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, Main.rand.NextBool(3) ? 16 : 127);
            }
            if (Main.rand.NextBool(5))
            {
				int smoke = Gore.NewGore(Entity.GetSource_FromThis(),new Vector2(hitbox.X, hitbox.Y), default, Main.rand.Next(375, 378), 0.75f);
				Main.gore[smoke].behindTiles = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
    }
}
