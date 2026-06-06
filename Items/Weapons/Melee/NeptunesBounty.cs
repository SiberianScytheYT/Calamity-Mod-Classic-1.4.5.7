using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class NeptunesBounty : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Neptune's Bounty");
/*
            Tooltip.SetDefault("Fires a trident that rains additional tridents as it travels\n" +
                "Hitting enemies will inflict the crush depth debuff\n" +
                "The lower the enemies' defense, the more damage they take from this debuff");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 122;
            Item.height = 122;
            Item.damage = 380;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<NeptuneOrb>();
            Item.shootSpeed = 12f;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AbyssBlade>());
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Phantoplasm>(), 5);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 15);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 15);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 33);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 600);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 600);
        }
    }
}
