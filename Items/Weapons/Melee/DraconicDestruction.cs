using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class DraconicDestruction : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Draconic Destruction");
/*
            Tooltip.SetDefault("Fires a draconic sword beam that explodes into additional beams\n" +
                "Additional beams fly up and down to shred enemies");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 94;
            Item.damage = 350;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 24;
            Item.useTurn = true;
            Item.knockBack = 7.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 94;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<DracoBeam>();
            Item.shootSpeed = 14f;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CoreofCinder>(), 3);
            recipe.AddIngredient(ModContent.ItemType<CoreofEleum>(), 3);
            recipe.AddIngredient(ItemID.FragmentSolar, 10);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 35);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 600);
        }
    }
}
