using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class BlazingStar : RogueWeapon
    {
        public const float Speed = 13f;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Blazing Star");
/*
            Tooltip.SetDefault("Stacks up to 3\n" +
                               "Stealth strikes release all stars at once with infinite piercing");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 92;
            Item.crit = 4;
            Item.Calamity().rogue = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.width = 1;
            Item.height = 1;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 4;
            Item.UseSound = SoundID.Item1;
            Item.maxStack = 3;

            Item.shootSpeed = Speed;
            Item.shoot = ModContent.ProjectileType<BlazingStarProj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                if (Item.stack != 1)
                {
                    for (int i = 0; i < Item.stack; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-MathHelper.ToRadians(8f), MathHelper.ToRadians(8f), i / (float)(Item.stack - 1)));
                        Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, perturbedSpeed, type, damage, Item.knockBack, player.whoAmI, 0f, 0f).Calamity().stealthStrike = true;

                        Projectile projectile = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, perturbedSpeed, type, damage, Item.knockBack, player.whoAmI, 0f);
                        projectile.penetrate = -1;
                        projectile.Calamity().stealthStrike = true;

                    }
                    return false;
                }
            }
            return true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < Item.stack;
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Glaive>(), 1);
            recipe.AddIngredient(ItemID.HellstoneBar, 3);
            recipe.AddIngredient(ModContent.ItemType<EssenceofChaos>(), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
