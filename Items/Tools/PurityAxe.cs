using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
    public class PurityAxe : ModItem
    {
        private static int AxePower = 25;
        private static float PowderSpeed = 21f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Axe of Purity");
/*
            Tooltip.SetDefault("Left click to use as a tool\n" +
                "Right click to cleanse evil");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 43;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 58;
            Item.height = 46;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useTurn = true;
            Item.axe = AxePower;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int powderDamage = (int)(0.85f * damage);
            int idx = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, powderDamage, Item.knockBack, player.whoAmI, 0f, 0f);
            Main.projectile[idx].DamageType = DamageClass.Melee;
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.axe = 0;
                Item.shoot = ProjectileID.PurificationPowder;
                Item.shootSpeed = PowderSpeed;
            }
            else
            {
                Item.axe = AxePower;
                Item.shoot = ProjectileID.None;
                Item.shootSpeed = 0f;
            }
            return base.CanUseItem(player);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 58);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FellerofEvergreens>());
            recipe.AddIngredient(ItemID.PurificationPowder, 20);
            recipe.AddIngredient(ItemID.PixieDust, 20);
            recipe.AddIngredient(ItemID.CrystalShard, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
