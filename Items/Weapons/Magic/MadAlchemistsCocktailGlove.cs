using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class MadAlchemistsCocktailGlove : ModItem
    {
        private int FlaskType = 0;
        private int BaseDamage = 200;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mad Alchemist's Cocktail Glove");
/*
            Tooltip.SetDefault("Fires a variety of high-velocity flasks that have various effects\n" +
                "Right click to throw a flask that inflicts a variety of debuffs");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Magic;
            Item.noUseGraphic = true;
            Item.mana = 12;
            Item.width = 26;
            Item.height = 36;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item106;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MadAlchemistsCocktailRed>();
            Item.shootSpeed = 12f;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player) => base.CanUseItem(player);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<MadAlchemistsCocktailAlt>();
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, (int)(damage * 1.5f), Item.knockBack, player.whoAmI, 0f, 0f);
            }
            else
            {
                switch (FlaskType)
                {
                    case 0:
                        type = ModContent.ProjectileType<MadAlchemistsCocktailRed>();
                        break;
                    case 1:
                        type = ModContent.ProjectileType<MadAlchemistsCocktailBlue>();
                        break;
                    case 2:
                        type = ModContent.ProjectileType<MadAlchemistsCocktailGreen>();
                        break;
                    case 3:
                        type = ModContent.ProjectileType<MadAlchemistsCocktailPurple>();
                        break;
                    default:
                        break;
                }

                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);

                FlaskType++;
                if (FlaskType > 3)
                    FlaskType = 0;
            }

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ToxicFlask);
            recipe.AddIngredient(ItemID.BottledWater, 15);
            recipe.AddIngredient(ItemID.Leather, 5);
            recipe.AddIngredient(ModContent.ItemType<EffulgentFeather>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CoreofEleum>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CoreofCinder>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CoreofChaos>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CoreofCalamity>());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
        }
    }
}
