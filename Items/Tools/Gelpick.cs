using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
    public class Gelpick : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Gelpick");
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 16;
            Item.useAnimation = 20;
            Item.pick = 100;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2.5f;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.rare = 4;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 15);
            recipe.AddIngredient(ItemID.Gel, 30);
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddTile(ModContent.TileType<StaticRefiner>());
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 20);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slimed, 180);
        }
    }
}
